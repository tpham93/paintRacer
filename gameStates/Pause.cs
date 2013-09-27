using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace paintRacer
{
    class Pause : IGameStateElements
    {

        enum ScreenButton
        {
            Continue,
            New_Game,
            Main_Menu,
            Unidentified
        }

        Texture2D[] buttonTextures;
        Rectangle[] buttonRectangles;
        Texture2D backgroundTexture;

        Point mousePosition;
        IGameStateElements previousGamestateElement;
        ContentManager content;

        public Pause(IGameStateElements previousGamestateElement, Rectangle screenRectangle)
        {
            this.previousGamestateElement = previousGamestateElement;
            buttonTextures = new Texture2D[(int)ScreenButton.Unidentified];
            buttonRectangles = new Rectangle[(int)ScreenButton.Unidentified];
            Point tmpPosition = new Point(screenRectangle.Width / 2 - Config.BIG_BUTTON_X / 2, screenRectangle.Height / 4);
            for (int i = 0; i < buttonRectangles.Length; ++i)
            {
                buttonRectangles[i] = new Rectangle(tmpPosition.X, tmpPosition.Y, Config.BIG_BUTTON_X, Config.BIG_BUTTON_Y);
                tmpPosition.Y += Config.BIG_BUTTON_Y + Config.BIG_BUTTON_SPACE;
            }
        }

        public void Load(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            this.content = content;
            backgroundTexture = Helper.genRectangleTexture(1, 1, Color.Black * 0.75f);
            buttonTextures[(int)ScreenButton.Continue] = Helper.loadImage(@"Content\Buttons\Continue.png");
            buttonTextures[(int)ScreenButton.New_Game] = Helper.loadImage(@"Content\Buttons\Restart.png");
            buttonTextures[(int)ScreenButton.Main_Menu] = Helper.loadImage(@"Content\Buttons\Exit.png");
        }

        public EGameStates Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            Point newMousePosition = new Point(mouseState.X, mouseState.Y); ;

            if (mouseState.LeftButton == ButtonState.Pressed && mousePosition != newMousePosition)
            {
                mousePosition = newMousePosition;
                for (int i = 0; i < (int)ScreenButton.Unidentified; ++i)
                {
                    if (buttonRectangles[i].Contains(mousePosition))
                    {
                        switch ((ScreenButton)i)
                        {
                            case ScreenButton.Continue:
                                return previousGamestateElement.getGameState();
                            case ScreenButton.New_Game:
                                previousGamestateElement.Load(content);
                                return previousGamestateElement.getGameState();
                            case ScreenButton.Main_Menu:
                                return EGameStates.Menue;

                        }
                    }
                }
            }
            return EGameStates.Pause;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            previousGamestateElement.Draw(gameTime, spriteBatch);
            spriteBatch.Begin();  
            spriteBatch.Draw(backgroundTexture, spriteBatch.GraphicsDevice.Viewport.Bounds, Color.White);
            for (int i = 0; i < buttonTextures.Length; ++i)
            {
                spriteBatch.Draw(buttonTextures[i],buttonRectangles[i],Color.White);
            }

            spriteBatch.End();
        }

        public void Unload()
        {

        }


        public EGameStates getGameState()
        {
            return EGameStates.Pause;
        }
    }
}
