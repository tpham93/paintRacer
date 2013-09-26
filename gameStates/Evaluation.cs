﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace paintRacer
{
    class Evaluation : IGameStateElements
    {

        enum ScreenButton
        {
            Restart,
            Exit,
            Undefined
        }

        bool newHighScore;

        Player player;

        Texture2D[] buttonTextures;
        Rectangle[] buttonRectangles;

        Point mousePosition;
        Rectangle screenDimensions;
        Viewport nameViewport;
        Rectangle timeRectangle;

        Rectangle highscoreViewRectangle;

        Texture2D backgroundAreaTexture;

        Viewport gameInfoViewport;


        TimeSpan totalTime;
        HighscoreData highscoreData;
        SpriteFont spriteFont;
        int fontHeight;

        IGameStateElements previousGameStateElement;

        ContentManager content;

        public Evaluation(IGameStateElements previousGameStateElement, Rectangle screenDimensions, EvaluationData evaluationData)
        {
            this.player = evaluationData.player;
            this.highscoreData = evaluationData.highscore;
            this.totalTime = evaluationData.time;
            this.previousGameStateElement = previousGameStateElement;
            newHighScore = highscoreData.insertScore(new HighscoreElement(evaluationData.player.getName(), evaluationData.time));
            if (newHighScore)
                highscoreData.writeToFile();
            Console.Out.WriteLine(newHighScore);
            buttonTextures = new Texture2D[(int)ScreenButton.Undefined];
            buttonRectangles = new Rectangle[(int)ScreenButton.Undefined];

            Point tmpPoint = new Point(50, screenDimensions.Height - 50 - Config.SMALL_BUTTON_Y);
            for (int i = 0; i < buttonRectangles.Length; ++i)
            {
                buttonRectangles[i] = new Rectangle(tmpPoint.X, tmpPoint.Y, Config.SMALL_BUTTON_X, Config.SMALL_BUTTON_Y);
                tmpPoint.X += Config.SMALL_BUTTON_X + Config.SMALL_BUTTON_SPACE;
            }

            const int highscoreNameListWidth = 225;
            const int highscoreNameListHeight = 380;
            const int highscoreTimeListWidth = 120;
            const int highscoreTimeListHeight = 380;

            nameViewport = new Viewport(800 - highscoreNameListWidth - 50 - highscoreTimeListWidth, 50, highscoreNameListWidth, highscoreNameListHeight);
            timeRectangle = new Rectangle(800 - 50 - highscoreTimeListWidth, 50, highscoreTimeListWidth, highscoreTimeListHeight);
            highscoreViewRectangle = new Rectangle(nameViewport.Bounds.Left, nameViewport.Bounds.Top, timeRectangle.Right - nameViewport.Bounds.Left, timeRectangle.Height);
            gameInfoViewport = new Viewport(50, 50, 2 * Config.SMALL_BUTTON_X + Config.SMALL_BUTTON_SPACE, 200);
        }

        public void Load(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            this.content = content;
            spriteFont = content.Load<SpriteFont>("font");
            fontHeight = (int)spriteFont.MeasureString("0").Y;
            backgroundAreaTexture = Helper.genRectangleTexture(1, 1, Config.TEXT_BOX_COLOR);
            buttonTextures[(int)ScreenButton.Restart] = Helper.loadImage(@"Content\evaluation\Restart.png");
            buttonTextures[(int)ScreenButton.Exit] = Helper.loadImage(@"Content\evaluation\Exit.png");
        }

        public EGameStates Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            Point newMousePosition = new Point(mouseState.X, mouseState.Y); ;

            if (mouseState.LeftButton == ButtonState.Pressed && mousePosition != newMousePosition)
            {
                mousePosition = newMousePosition;
                for (int i = 0; i < (int)ScreenButton.Undefined; ++i)
                {
                    if (buttonRectangles[i].Contains(mousePosition))
                    {
                        switch ((ScreenButton)i)
                        {
                            case ScreenButton.Restart:
                                previousGameStateElement.Load(content);
                                return previousGameStateElement.getGameState();
                            case ScreenButton.Exit:
                                return EGameStates.Menue;
                        }
                    }
                }
            }

            return EGameStates.Evaluation;
        }

        public void Draw(Microsoft.Xna.Framework.GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            for (int i = 0; i < buttonTextures.Length; ++i)
            {
                spriteBatch.Draw(buttonTextures[i], buttonRectangles[i], Color.White);
                spriteBatch.Draw(backgroundAreaTexture, highscoreViewRectangle, Color.White);
                spriteBatch.Draw(backgroundAreaTexture, gameInfoViewport.Bounds, Color.White);
            }

            spriteBatch.End();

            Viewport tmp = spriteBatch.GraphicsDevice.Viewport;

            List<HighscoreElement> highscoreElements = highscoreData.HighscoreEntries;
            Vector2 tmpPos = new Vector2(timeRectangle.Left, timeRectangle.Top);

            spriteBatch.Begin();
                for (int i = 0; i < highscoreElements.Count; ++i)
                {
                    spriteBatch.DrawString(spriteFont, highscoreElements[i].getFormattedTimeString(), tmpPos, Config.TEXT_COLOR);
                    tmpPos.Y += fontHeight + Config.TEXTFIELD_BORDER;
                }
            spriteBatch.End();

            spriteBatch.GraphicsDevice.Viewport = nameViewport;
            tmpPos = Vector2.Zero;

            spriteBatch.Begin();
            for (int i = 0; i < highscoreElements.Count; ++i)
            {
                spriteBatch.DrawString(spriteFont, highscoreElements[i].Name, tmpPos, Config.TEXT_COLOR);
                tmpPos.Y += fontHeight + Config.TEXTFIELD_BORDER;
            }
            spriteBatch.End();

            spriteBatch.GraphicsDevice.Viewport = gameInfoViewport;
            tmpPos = Vector2.Zero;

            spriteBatch.Begin();
            spriteBatch.DrawString(spriteFont, "Name: " + player.getName(), tmpPos, Config.TEXT_COLOR);
            tmpPos.Y += Config.SMALL_LINE_SPACE + fontHeight;
            spriteBatch.DrawString(spriteFont, "Time: " + "     " + string.Format("{0:00}:{1:00}:{2:000}", (int)totalTime.TotalMinutes, totalTime.Seconds, totalTime.Milliseconds), tmpPos, Config.TEXT_COLOR);
            tmpPos.Y += Config.SMALL_LINE_SPACE + fontHeight;
            TimeSpan[] lapTimes = player.times;
            for (int i = 0; i < lapTimes.Length; ++i)
            {
                spriteBatch.DrawString(spriteFont, "    "+"Lap " + i + ": " + string.Format("{0:00}:{1:00}:{2:000}", (int)lapTimes[i].TotalMinutes, lapTimes[i].Seconds, lapTimes[i].Milliseconds), tmpPos, Config.TEXT_COLOR);
                tmpPos.Y += fontHeight /*+ Config.SMALL_LINE_SPACE*/;
            }
            spriteBatch.End();

            spriteBatch.GraphicsDevice.Viewport = tmp;
        }

        public void Unload()
        {

        }

        public EGameStates getGameState()
        {
            return EGameStates.Evaluation;
        }
    }
}