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
        // spritefont to draw text
        //SpriteFont spriteFont;    //not used here
        // texture to show selected menuentry
        Texture2D selectionPointerTexture;
        // array of rectangle to show menuentries
        Texture2D[] menuEntrieTexture;
        // index of selected entry
        int selectedEntry;
        // time between keypress (max = MINTIMEKEYPRESS))
        int timeBetweenKeyPress;

        // constants___________________________________________
        const int SELECTIONPOINTERSIZE = 20;    //size of marker
        const int MENUENTRYSIZE_X = 125;
        const int MENUENTRYSIZE_Y = 50;
        const int MENUENTRYSPACE = 30;
        const int MENUENTRYNUM = 5;

        // variables used as constants
        Vector2 MENUSTARTPOS = new Vector2(500 - MENUENTRYSIZE_X / 2, MENUENTRYSPACE);   //"pointer" on first menuentry
        Color MENUENTRYCOLOR = Color.Gray;  //menuentrycolor

        const int MINTIMEKEYPRESS = 150;

        const Keys SELECT_UP = Keys.Up;
        const Keys SELECT_DOWN = Keys.Down;
        const Keys SELECT_ENTRY = Keys.Enter;

        //_____________________________________________________
        public void Load(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            // geneart all rectangles
            selectionPointerTexture = Helper.loadImage("testcar.png", new Rectangle(0,0,20,40));
            menuEntrieTexture = new Texture2D[5];                            //array-size shuld be 5
            menuEntrieTexture[0] = Helper.loadImage("menu/singlePlayer.png", new Rectangle(0, 0, MENUENTRYSIZE_X, MENUENTRYSIZE_Y));//first menuentry
            menuEntrieTexture[1] = Helper.loadImage("menu/multyPlayer.png", new Rectangle(0, 0, MENUENTRYSIZE_X, MENUENTRYSIZE_Y)); //2nd menuentry
            menuEntrieTexture[2] = Helper.loadImage("menu/highscore.png", new Rectangle(0, 0, MENUENTRYSIZE_X, MENUENTRYSIZE_Y));   //3rd menuentry
            menuEntrieTexture[3] = Helper.loadImage("menu/credits.png", new Rectangle(0, 0, MENUENTRYSIZE_X, MENUENTRYSIZE_Y));     //4th menuentry
            menuEntrieTexture[4] = Helper.loadImage("menu/beenden.png", new Rectangle(0, 0, MENUENTRYSIZE_X, MENUENTRYSIZE_Y));     //5th menuentry
            //load font
            //spriteFont = content.Load<SpriteFont>("Arial");
        }

        public EGameStates Update(GameTime gameTime)
        {
            if (timeBetweenKeyPress >= MINTIMEKEYPRESS)
            {
                KeyboardState keyboardState = Keyboard.GetState();
                if (keyboardState.IsKeyDown(SELECT_UP))
                {
                    selectedEntry = (selectedEntry + MENUENTRYNUM -1) % MENUENTRYNUM;
                    timeBetweenKeyPress = 0;
                }
                if (keyboardState.IsKeyDown(SELECT_DOWN))
                {
                    selectedEntry = (selectedEntry + 1) % MENUENTRYNUM;
                    timeBetweenKeyPress = 0;
                }
                if (keyboardState.IsKeyDown(SELECT_ENTRY))
                {
                    return EGameStates.SinglePlayer;
                }
            }
            else
            {
                timeBetweenKeyPress += gameTime.ElapsedGameTime.Milliseconds;
            }
            return EGameStates.Menue;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            
            Vector2 tmpVect = new Vector2(MENUSTARTPOS.X, MENUSTARTPOS.Y);
            // draw entries
            for (int i = 0; i < MENUENTRYNUM; i++)
            {
                spriteBatch.Draw(menuEntrieTexture[i], tmpVect, MENUENTRYCOLOR);
                tmpVect.Y += MENUENTRYSIZE_Y + MENUENTRYSPACE;
            }

            // draw menuentrypointer
            tmpVect.X -= MENUENTRYSPACE + SELECTIONPOINTERSIZE;
            tmpVect.Y = MENUSTARTPOS.Y + (MENUENTRYSPACE + MENUENTRYSIZE_Y) * selectedEntry;
            spriteBatch.Draw(selectionPointerTexture, tmpVect, Color.Red);

            // draw caption
            //spriteBatch.DrawString(spriteFont, "Menue", Vector2.Zero, Color.Black);
            //spriteBatch.DrawString(spriteFont, "Arrows to change selection, Enter to select", new Vector2(0, 20), Color.Black);

            spriteBatch.End();
        }

        public void Unload()
        {

        }
    }
}
