using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace paintRacer
{
    class CarSelection : IGameStateElements
    {
        EGameStates nextGamestate;
        SpriteFont spriteFont;

        const int SPACE_Y = 5;
        int fontHeight;

        Player[] cars;

        string[] names;
        Viewport nameViewport;
        Texture2D nameBackground;
        Rectangle[] nameRectangles;

        Texture2D selectedTexture;
        Vector2 selectedTextureBackgroundMiddlePosition;
        Texture2D selectedTextureBackground;
        Rectangle selectedTextureBackgroundRectangle;

        int scrollValue;
        string[] files;
        Viewport fileListViewport;
        Texture2D fileListBackground;
        Rectangle fileListRectangle;
        Vector2 fileOffset;

        Texture2D finishButtonTexture;
        Rectangle finishButtonRectangle;

        const int MENUENTRYSIZE_X = Config.SMALL_BUTTON_X;
        const int MENUENTRYSIZE_Y = Config.SMALL_BUTTON_Y;

        readonly Color[] playerColors;

        TimeSpan timeBetweenKeyPress;

        int nameIndex;

        Texture2D bgPic;

        public CarSelection(EGameStates nextGamestate)
        {
            const int fileListWidth = 300;
            const int fileListHeight = 200;

            const int selectedRectangleSize = 400;

            const int nameWidth = 200;
            const int nameHeight = 25;

            switch (nextGamestate)
            {
                case EGameStates.SinglePlayer:
                    cars = new Player[1];
                    names = new string[1];
                    break;
                case EGameStates.MultiPlayer:
                    cars = new Player[2];
                    names = new string[2];
                    break;
                default:
                    cars = new Player[0];
                    break;
            }


            fileOffset = new Vector2();
            selectedTexture = null;

            this.nextGamestate = nextGamestate;
            files = Directory.GetFiles(@"cars\").Where(file => file.ToLower().EndsWith(".png") || file.ToLower().EndsWith(".jpg") || file.ToLower().EndsWith(".gif")).ToArray<string>();
            for (int i = 0; i < files.Length; ++i)
            {
                files[i] = Path.GetFileName(files[i]);
            }
            scrollValue = Mouse.GetState().ScrollWheelValue;
            fileListRectangle = new Rectangle(50, 130, fileListWidth, fileListHeight);
            selectedTextureBackgroundRectangle = new Rectangle(375, 40, selectedRectangleSize, selectedRectangleSize);
            fileListBackground = Helper.genRectangleTexture(fileListWidth, fileListHeight, Config.TEXT_BOX_COLOR);

            selectedTextureBackground = Helper.genRectangleTexture(selectedRectangleSize, selectedRectangleSize, Color.White * 0.5f);
            Point selectedTextureCenterPoint = selectedTextureBackgroundRectangle.Center;
            selectedTextureBackgroundMiddlePosition = new Vector2(selectedTextureCenterPoint.X, selectedTextureCenterPoint.Y);
            fileListViewport = new Viewport(fileListRectangle);
            finishButtonRectangle = new Rectangle(50, 390, MENUENTRYSIZE_X, MENUENTRYSIZE_Y);

            playerColors = new Color[] { Color.Blue, Color.Red };

            nameIndex = -1;
            nameRectangles = new Rectangle[names.Length];
            nameBackground = Helper.genRectangleTexture(nameWidth, nameHeight, Config.TEXT_BOX_COLOR);
        }

        public void Load(ContentManager content)
        {
            bgPic = Helper.loadImage(@"Content\car_box.png");
            finishButtonTexture = Helper.loadImage(@"Content\loadMenu\Finish.png", new Rectangle(0, 0, MENUENTRYSIZE_X, MENUENTRYSIZE_Y));
            spriteFont = content.Load<SpriteFont>(@"font");
            fontHeight = (int)spriteFont.MeasureString("0").Y;
            for (int i = 0; i < nameRectangles.Length; ++i)
            {
                names[i] = "";
                nameRectangles[i] = new Rectangle(150, 40 + i * (SPACE_Y + fontHeight), nameBackground.Width, nameBackground.Height);
            }
            nameViewport = new Viewport(50, 40, 300, (nameRectangles.Length - 1) * (SPACE_Y + fontHeight) + 40);
        }

        public EGameStates Update(GameTime gameTime)
        {
            if (timeBetweenKeyPress < Config.TIME_BETWEEN_SAME_EVENT)
                timeBetweenKeyPress += gameTime.ElapsedGameTime;



            MouseState mouseState = Mouse.GetState();
            int newScrollValue = mouseState.ScrollWheelValue;
            if (newScrollValue != scrollValue)
            {
                int difference = newScrollValue - scrollValue;
                fileOffset.Y += 10 * (difference / Math.Abs(difference));
                int minOffsetY = -Math.Max(files.Length * (SPACE_Y + fontHeight) - fileListRectangle.Height, 0);
                fileOffset.Y = Math.Max(minOffsetY, Math.Min(fileOffset.Y, 0));

                scrollValue = mouseState.ScrollWheelValue;
            }

            if (nameIndex != -1)
            {
                KeyboardState keyboardState = Keyboard.GetState();
                string input = Helper.KeyToChar(Keyboard.GetState(), gameTime);
                if (timeBetweenKeyPress > Config.TIME_BETWEEN_SAME_EVENT)
                {
                    if (keyboardState.IsKeyDown(Keys.Back) && names[nameIndex].Length > 0)
                    {
                        names[nameIndex] = names[nameIndex].Substring(0, names[nameIndex].Length - 1);
                        timeBetweenKeyPress = new TimeSpan();
                    }
                    if (!input.Equals(""))
                    {
                        names[nameIndex] += input;
                        timeBetweenKeyPress = new TimeSpan();
                    }
                }
            }

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                Point mouseposition = new Point((int)mouseState.X, (int)mouseState.Y);

                nameIndex = -1;

                for (int i = 0; i < nameRectangles.Length; ++i)
                {
                    if (nameRectangles[i].Contains(mouseposition))
                    {
                        nameIndex = i;
                    }
                }

                if (fileListViewport.Bounds.Contains(mouseposition))
                {
                    int relativeHeight = mouseposition.Y - fileListRectangle.Y - (int)fileOffset.Y;
                    int possiblePosition = (int)(relativeHeight / (double)(SPACE_Y + fontHeight));
                    if (possiblePosition >= 0 && possiblePosition < files.Length)
                    {
                        selectedTexture = Helper.loadImage(@"cars\" + files[possiblePosition]);
                    }
                }
                else if (finishButtonRectangle.Contains(mouseposition))
                {
                    if (selectedTexture != null && names.Count<string>(name => name == "") == 0)
                    {
                        for (int i = 0; i < cars.Length; ++i)
                        {
                            cars[i] = new Player(names[i], selectedTexture, playerColors[i], Global.map.CheckPoints.Length / 2);
                        }
                        Global.players = cars;
                        return nextGamestate;
                    }
                }
            }

            return EGameStates.CarSelection;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            spriteBatch.Begin();

            spriteBatch.Draw(bgPic, new Vector2(0, 0), Color.White);

            {
                spriteBatch.Draw(fileListBackground, fileListRectangle, Color.White);
                spriteBatch.Draw(selectedTextureBackground, selectedTextureBackgroundRectangle, Color.White);
                spriteBatch.Draw(finishButtonTexture, finishButtonRectangle, Color.White);
                if (selectedTexture != null)
                {
                    spriteBatch.Draw(selectedTexture, selectedTextureBackgroundMiddlePosition, null, Color.White, 0, new Vector2(selectedTexture.Bounds.Center.X, selectedTexture.Bounds.Center.Y), 1, SpriteEffects.None, 0);
                }
            }
            spriteBatch.End();
            GraphicsDevice graphicsDevice = spriteBatch.GraphicsDevice;
            Viewport tmp = graphicsDevice.Viewport;

            graphicsDevice.Viewport = nameViewport;
            spriteBatch.Begin();
            {
                for (int i = 0; i < nameRectangles.Length; ++i)
                {
                    spriteBatch.Draw(nameBackground, new Rectangle(nameRectangles[i].X - nameViewport.X, nameRectangles[i].Y - nameViewport.Y, nameRectangles[i].Width, nameRectangles[i].Height), Color.White);
                    spriteBatch.DrawString(spriteFont, "Player " + (i + 1) + ":", new Vector2(0, nameRectangles[i].Y - nameViewport.Y), Config.TEXT_COLOR);
                    spriteBatch.DrawString(spriteFont, names[i], new Vector2(nameRectangles[i].X - nameViewport.X, nameRectangles[i].Y - nameViewport.Y), Config.TEXT_COLOR);
                }
            }
            spriteBatch.End();

            graphicsDevice.Viewport = fileListViewport;

            Vector2 tmpPos = fileOffset;
            spriteBatch.Begin();
            {
                for (int i = 0; i < files.Length; ++i)
                {
                    string s = files[i];
                    spriteBatch.DrawString(spriteFont, s, tmpPos, Config.TEXT_COLOR);
                    tmpPos.Y += spriteFont.MeasureString(s).Y + SPACE_Y;
                }
            }
            spriteBatch.End();
            graphicsDevice.Viewport = tmp;
        }

        public void Unload()
        {
        }
    }
}
