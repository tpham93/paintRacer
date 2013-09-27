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
                           "     > commons.wikimedia.org/wiki/", "             11_Renault_JP.jpg",
                           "     > commons.wikimedia.org/wiki/", "             Toyota_F1_Canada_2006.jpg",
                           "     > commons.wikimedia.org/wiki/", "             Fale_F1_Monza_2004_176.jpg",
                           "     > commons.wikimedia.org/wiki/", "             Grid_girls_F1.jpg",
                           "     > commons.wikimedia.org/wiki/", "             23_Renault_JP.jpg", "",
                           "Designed in an Acagamics lecture"};

        int scrollvalue;

        public void Load(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            bgPic = Helper.loadImage(@"Content\Backgrounds\credits.png");
            pixel = Helper.genRectangleTexture(1,1,Color.White);
            back = Helper.loadImage(@"Content\Buttons\back.png", new Rectangle(0, 0, (int)Config.SMALL_BUTTON.X, (int)Config.SMALL_BUTTON.Y));

            font = content.Load<SpriteFont>(@"font");

            timeSpace = new TimeSpan();

            scrollvalue = Mouse.GetState().ScrollWheelValue;
        }

        public EGameStates Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            timeSpace += gameTime.ElapsedGameTime;

            int newScrollValue = Mouse.GetState().ScrollWheelValue;

            if (newScrollValue > scrollvalue)
            {
                scroll -= 3;
                scroll = Math.Max(0, scroll);
            }
            else if (newScrollValue < scrollvalue)
            {
                scroll += 3;
                scroll = Math.Min(output.Length - 20, scroll);
            }

            scrollvalue = newScrollValue;

            MouseState mouse = Mouse.GetState();
            if (mouse.LeftButton == ButtonState.Pressed && timeSpace > Config.TIME_BETWEEN_SAME_EVENT && (mouse.X > backPos.X) && (mouse.X < backPos.X + (int)Config.SMALL_BUTTON.X) && (mouse.Y > backPos.Y) && (mouse.Y < backPos.Y + (int)Config.SMALL_BUTTON.Y))
            {
                return EGameStates.Menue;
            }

            if (timeSpace > Config.TIME_BETWEEN_SAME_EVENT)
            {
                KeyboardState keyboartState = Keyboard.GetState();
                if (keyboartState.IsKeyDown(Keys.Down))
                {
                    ++scroll;
                    scroll = scroll >= output.Length -20 ? output.Length - 20 : scroll;
                    timeSpace = new TimeSpan();
                }
                else if (keyboartState.IsKeyDown(Keys.Up))
                {
                    --scroll;
                    scroll = scroll < 0 ? 0 : scroll;
                    timeSpace = new TimeSpan();
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
                textpos.Y += Config.LINE_SIZE + Config.SMALL_LINE_SPACE;
            }

            backPos = new Vector2(50, 410);
            spriteBatch.Draw(back, backPos, Color.White);

            spriteBatch.End();
        }

        public void Unload()
        {
        }


        public EGameStates getGameState()
        {
            return EGameStates.Credits;
        }
    }
}
