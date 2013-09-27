using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace paintRacer
{
    class Highscore : IGameStateElements
    {
        HighscoreData highscoreData;
        Viewport nameViewport;
        Rectangle timeRectangle;

        string[] mapNames;
        Viewport mapListViewport;
        Vector2 mapNameOffset;

        Rectangle highscoreViewRectangle;

        Texture2D backButtonTexture;
        Rectangle backButtonRectangle;

        Texture2D backgroundAreaTexture;
        Texture2D backgroundImage;
        Texture2D bgPic;

        SpriteFont spriteFont;
        int fontHeight;

        int scrollValue;

        public Highscore()
        {
            const int mapListWidth = 300;
            const int mapListHeight = 300;

            const int highscoreNameListWidth = 225;
            const int highscoreNameListHeight = 380;
            const int highscoreTimeListWidth = 120;
            const int highscoreTimeListHeight = 380;

            mapListViewport = new Viewport(50, 50, mapListWidth, mapListHeight);
            nameViewport = new Viewport(mapListViewport.Bounds.Right + 50, 50, highscoreNameListWidth, highscoreNameListHeight);
            timeRectangle = new Rectangle(800 - 50 - highscoreTimeListWidth, 50, highscoreTimeListWidth, highscoreTimeListHeight);
            highscoreViewRectangle = new Rectangle(nameViewport.Bounds.Left, nameViewport.Bounds.Top, timeRectangle.Right - nameViewport.Bounds.Left, timeRectangle.Height);
            backButtonRectangle = new Rectangle(50, 430 - Config.SMALL_BUTTON_Y, Config.SMALL_BUTTON_X, Config.SMALL_BUTTON_Y);

            mapNameOffset = Vector2.Zero;

            mapNames = Directory.GetDirectories(@"saved_maps\").Where(file => File.Exists(file+@"\highscore")).ToArray<string>();
            for (int i = 0; i < mapNames.Length; ++i)
            {
                string[] addressParts = mapNames[i].Split(new char[] { '\\', '/'});
                mapNames[i] = addressParts[addressParts.Length - 1];
            }

        }

        public void Load(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            backgroundAreaTexture = Helper.genRectangleTexture(1, 1, Config.TEXT_BOX_COLOR);
            backButtonTexture = Helper.loadImage(@"Content\Buttons\Back.png", new Rectangle(0, 0, Config.SMALL_BUTTON_X, Config.SMALL_BUTTON_Y));
            bgPic = Helper.loadImage(@"Content\Backgrounds\podest.png");
            spriteFont = content.Load<SpriteFont>("font");
            fontHeight = (int)spriteFont.MeasureString("0").Y;
        }

        public EGameStates Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            int newScrollValue = mouseState.ScrollWheelValue;
            if (newScrollValue != scrollValue)
            {
                int difference = newScrollValue - scrollValue;
                mapNameOffset.Y += 10 * (difference / Math.Abs(difference));
                int minOffsetY = -Math.Max(mapNames.Length * (Config.TEXTFIELD_BORDER + fontHeight) - mapListViewport.Bounds.Height, 0);
                mapNameOffset.Y = Math.Max(minOffsetY, Math.Min(mapNameOffset.Y, 0));

                scrollValue = mouseState.ScrollWheelValue;
            }

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                Point mouseposition = new Point((int)mouseState.X, (int)mouseState.Y);

                if (mapListViewport.Bounds.Contains(mouseposition))
                {
                    int relativeHeight = mouseposition.Y - mapListViewport.Bounds.Y - (int)mapNameOffset.Y;
                    int possiblePosition = (int)(relativeHeight / (double)(Config.TEXTFIELD_BORDER + fontHeight));
                    if (possiblePosition >= 0 && possiblePosition < mapNames.Length)
                    {
                        highscoreData = new HighscoreData(@"saved_maps\"+mapNames[possiblePosition] + @"\highscore");
                    }
                }
                else if (backButtonRectangle.Contains(mouseposition))
                {
                    return EGameStates.Menue;
                }
            }

            return EGameStates.HightScore;
        }

        public void Draw(Microsoft.Xna.Framework.GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            {
                spriteBatch.Draw(bgPic, new Vector2(0, 0), Color.White);
                spriteBatch.Draw(backgroundAreaTexture, mapListViewport.Bounds, Color.White);
                spriteBatch.Draw(backgroundAreaTexture, highscoreViewRectangle, Color.White);
                spriteBatch.Draw(backButtonTexture, backButtonRectangle, Color.White);
            }
            spriteBatch.End();


            Viewport tmp = spriteBatch.GraphicsDevice.Viewport;

            spriteBatch.GraphicsDevice.Viewport = mapListViewport;

            Vector2 tmpPos = mapNameOffset;
            spriteBatch.Begin();
            {
                for (int i = 0; i < mapNames.Length; ++i)
                {
                    string s = mapNames[i];
                    spriteBatch.DrawString(spriteFont, s, tmpPos, Config.TEXT_COLOR);
                    tmpPos.Y += spriteFont.MeasureString(s).Y + Config.TEXTFIELD_BORDER;
                }
            }
            spriteBatch.End();

            spriteBatch.GraphicsDevice.Viewport = tmp;

            if (highscoreData != null)
            {
                List<HighscoreElement> highscoreElements = highscoreData.HighscoreEntries;
                tmpPos = new Vector2(timeRectangle.Left, timeRectangle.Top);
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
            }

            spriteBatch.GraphicsDevice.Viewport = tmp;
        }

        public void Unload()
        {

        }


        public EGameStates getGameState()
        {
            return EGameStates.HightScore;
        }
    }
}
