using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace paintRacer
{
    //ATTENTION!!! copy - paste!!! by Ludwig from Tuan
    class Menu : IGameStateElements
    {
        // texture to show selected menuentry
        Texture2D selectionPointerTexture;
        // array of rectangle to show menuentries
        Texture2D[] menuEntrieTexture;
        Rectangle[] menuEntryRectangles;
        //bgcolor
        Texture2D backgound;
        // index of selected entry
        int selectedEntry;
        // time between keypress (max = Config.TIME_BETWEEN_SAME_EVENT))
        TimeSpan timeBetweenKeyPress;

        TimeSpan mouseInactiveTime;

        Point mousePosition;

        // constants___________________________________________
        const int SELECTIONPOINTERSIZE_X = 100;    //size of marker
        const int SELECTIONPOINTERSIZE_Y = 50;    //size of marker
        const int MENUENTRYNUM = 5;

        // variables used as constants
        static readonly Point MENUSTARTPOS = new Point(50, Config.BIG_BUTTON_SPACE);   //"pointer" on first menuentry
        static readonly Color MENUENTRYCOLOR = Color.White;  //menuentrycolor
        static readonly Color MENUSELECTIONPOINTERCOLOR = Color.Green;

        const Keys SELECT_UP = Keys.Up;
        const Keys SELECT_DOWN = Keys.Down;
        const Keys SELECT_ENTRY = Keys.Enter;

        //_____________________________________________________

        public Menu()
        {
            menuEntryRectangles = new Rectangle[5];
            Point tmpPoint = MENUSTARTPOS;
            for (int i = 0; i < menuEntryRectangles.Length; ++i)
            {
                menuEntryRectangles[i] = new Rectangle(tmpPoint.X, tmpPoint.Y, Config.BIG_BUTTON_X, Config.BIG_BUTTON_Y);
                tmpPoint.Y += Config.BIG_BUTTON_Y + Config.BIG_BUTTON_SPACE;
            }
            mouseInactiveTime = new TimeSpan(0, 0, 1);
        }

        public void Load(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            // geneart all rectangles
            backgound = Helper.loadImage("Content/start.png");
            selectionPointerTexture = Helper.loadImage("Content/menu/car.png", new Rectangle(0, 0, SELECTIONPOINTERSIZE_X, SELECTIONPOINTERSIZE_Y));
            menuEntrieTexture = new Texture2D[5];                                                                                                              //array-size shuld be 5
            menuEntrieTexture[0] = Helper.loadImage(@"Content\menu\singlePlayer.png", new Rectangle(0, 0, (int)Config.BIG_BUTTON.X, (int)Config.BIG_BUTTON.Y));//first menuentry
            menuEntrieTexture[1] = Helper.loadImage(@"Content\menu\multyPlayer.png", new Rectangle(0, 0, (int)Config.BIG_BUTTON.X, (int)Config.BIG_BUTTON.Y)); //2nd menuentry
            menuEntrieTexture[2] = Helper.loadImage(@"Content\menu\highscore.png", new Rectangle(0, 0, (int)Config.BIG_BUTTON.X, (int)Config.BIG_BUTTON.Y));   //3rd menuentry
            menuEntrieTexture[3] = Helper.loadImage(@"Content\menu\credits.png", new Rectangle(0, 0, (int)Config.BIG_BUTTON.X, (int)Config.BIG_BUTTON.Y));     //4th menuentry
            menuEntrieTexture[4] = Helper.loadImage(@"Content\menu\beenden.png", new Rectangle(0, 0, (int)Config.BIG_BUTTON.X, (int)Config.BIG_BUTTON.Y));     //5th menuentry

            //initialize timeBetweenKeyPress
            timeBetweenKeyPress = Config.TIME_BETWEEN_SAME_EVENT;

            MouseState mouseState = Mouse.GetState();
            mousePosition = new Point(mouseState.X, mouseState.Y);
            for (int i = 0; i < menuEntryRectangles.Length; ++i)
            {
                if (menuEntryRectangles[i].Contains(mousePosition))
                {
                    selectedEntry = i;
                    break;
                }
            }
        }

        public EGameStates Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            Point newMousePosition = new Point(mouseState.X, mouseState.Y);

            if (timeBetweenKeyPress >= Config.TIME_BETWEEN_SAME_EVENT)
            {

                KeyboardState keyboardState = Keyboard.GetState();
                if (keyboardState.IsKeyDown(SELECT_UP))
                {
                    selectedEntry = (selectedEntry + MENUENTRYNUM - 1) % MENUENTRYNUM;
                    timeBetweenKeyPress = new TimeSpan();
                }
                if (keyboardState.IsKeyDown(SELECT_DOWN))
                {
                    selectedEntry = (selectedEntry + 1) % MENUENTRYNUM;
                    timeBetweenKeyPress = new TimeSpan();
                }

                if (keyboardState.IsKeyDown(SELECT_ENTRY))
                {
                    return getNextGamestate(selectedEntry);
                }
            }
            else
            {
                timeBetweenKeyPress += gameTime.ElapsedGameTime;
            }

            if (mouseInactiveTime < new TimeSpan())
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    for (int i = 0; i < menuEntryRectangles.Length; ++i)
                    {
                        if (menuEntryRectangles[i].Contains(mousePosition))
                        {
                            selectedEntry = i;
                            return getNextGamestate(i);
                        }
                    }
                }
                else if (mousePosition != newMousePosition)
                {
                    for (int i = 0; i < menuEntryRectangles.Length; ++i)
                    {
                        if (menuEntryRectangles[i].Contains(newMousePosition))
                        {
                            selectedEntry = i;
                            break;
                        }
                    }
                }
            }
            else
            {
                mouseInactiveTime -= gameTime.ElapsedGameTime;
            }
            mousePosition = newMousePosition;

            return EGameStates.Menue;
        }

        private EGameStates getNextGamestate(int index)
        {
            switch (selectedEntry)
            {
                case 0:
                    //Singleplayer
                    return EGameStates.LoadMenuSinglePlayer;
                case 1:
                    //Multiplayer
                    return EGameStates.LoadMenuMultiplayer;
                case 2:
                    //Highscore
                    return EGameStates.HightScore;
                case 3:
                    //Credits
                    return EGameStates.Credits;
                case 4:
                    //Exit
                    return EGameStates.Close;
                default:
                    throw new Exception("FOO!! This is bad. \ndefault in Menu-switch-case");
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(backgound, new Rectangle(0, -150, 800, 641), Color.White);

            Vector2 tmpVect = new Vector2(MENUSTARTPOS.X, MENUSTARTPOS.Y);
            // draw entries
            for (int i = 0; i < MENUENTRYNUM; i++)
            {
                spriteBatch.Draw(menuEntrieTexture[i], tmpVect, MENUENTRYCOLOR);
                tmpVect.Y += (int)Config.BIG_BUTTON.Y + Config.BIG_BUTTON_SPACE;
            }

            // draw menuentrypointer
            tmpVect.X += (int)Config.BIG_BUTTON.X + Config.BIG_BUTTON_SPACE - 5;
            tmpVect.Y = MENUSTARTPOS.Y + (Config.BIG_BUTTON_SPACE + (int)Config.BIG_BUTTON.Y) * selectedEntry + ((int)Config.BIG_BUTTON.Y - SELECTIONPOINTERSIZE_Y) / 2;
            spriteBatch.Draw(selectionPointerTexture, tmpVect, MENUSELECTIONPOINTERCOLOR);

            spriteBatch.End();
        }

        public void Unload()
        {

        }


        public EGameStates getGameState()
        {
            return EGameStates.Menue;
        }
    }
}
