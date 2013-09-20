using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace paintRacer.gameStates
{
    class Credits : IGameStateElements
    {

        private Texture2D bgPic;
        private Texture2D pixel;
        private Texture2D back;
        private Vector2 backPos;
        private SpriteFont font;
        private int scroll = 0;
        private TimeSpan timeSpace;

        private const int LINENUM = 20;
        private const int LINESIZE = Config.LINE_SIZE + Config.SMALL_LINE_SPACE;
        private const int BUTTONSIZE_X = (int)Config.SMALL_BUTTON.X;
        private const int BUTTONSIZE_Y = (int)Config.SMALL_BUTTON.Y;

        string[] output = {"CREDITS", 
                           "=======", "",
                           "Code by:",
                           "--------",
                           "  - Tuan Pham Minh",
                           "  - Sebastian Fritz",
                           "  - Ludwig Bedau", "",
                           "Graphics by:",
                           "------------",
                           "  - Ludwig Bedau",
                           "  - external sources:",
                           "     > commons.wikimedia.org/wiki/...", "         File:11_Renault_JP.jpg",
                           "     > commons.wikimedia.org/wiki/...", "         File:Toyota_F1_Canada_2006.jpg",
                           "     > commons.wikimedia.org/wiki/...", "         File:Fale_F1_Monza_2004_176.jpg",
                           "     > commons.wikimedia.org/wiki/...", "         File:Grid_girls_F1.jpg", "",
                           "Dessigned in an Acagamics lecture"};

        public void Load(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            bgPic = Helper.loadImage("Content/credits.png");
            pixel = Helper.loadImage("Content/OneWithePixel.png");
            back = Helper.loadImage("Content/loadMenu/back.png", new Rectangle(0, 0, BUTTONSIZE_X, BUTTONSIZE_Y));

            font = content.Load<SpriteFont>(@"font");

            timeSpace = new TimeSpan();
        }

        public EGameStates Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            timeSpace += gameTime.ElapsedGameTime;

            MouseState mouse = Mouse.GetState();
            if (mouse.LeftButton == ButtonState.Pressed && timeSpace > Config.TIME_BETWEEN_SAME_EVENT && (mouse.X > backPos.X) && (mouse.X < backPos.X + BUTTONSIZE_X) && (mouse.Y > backPos.Y) && (mouse.Y < backPos.Y + BUTTONSIZE_Y))
            {
                return EGameStates.Menue;
            }

            if (timeSpace > Config.TIME_BETWEEN_SAME_EVENT)
            {
                timeSpace = new TimeSpan();
                KeyboardState keyboartState = Keyboard.GetState();
                if (keyboartState.IsKeyDown(Keys.Down))
                {
                    ++scroll;
                    scroll = scroll >= output.Length ? output.Length - 1 : scroll;
                }
                else if (keyboartState.IsKeyDown(Keys.Up))
                {
                    --scroll;
                    scroll = scroll < 0 ? 0 : scroll;
                }
            }

            return EGameStates.Credits;
        }

        public void Draw(Microsoft.Xna.Framework.GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(bgPic, new Vector2(0, 0), Color.White);
            spriteBatch.Draw(pixel, new Rectangle(300, 10, 490, 450), Config.TEXT_BOX_COLOR);
            Vector2 textpos = new Vector2(305, 15);

            for (int count = scroll; count-scroll < LINENUM && count < output.Length; ++count)
            {
                spriteBatch.DrawString(font, output[count], textpos, Config.TEXT_COLOR);
                textpos.Y += LINESIZE;
            }

            backPos = new Vector2(50, 410);
            spriteBatch.Draw(back, backPos, Color.White);

            spriteBatch.End();
        }

        public void Unload()
        {
        }
    }
}
