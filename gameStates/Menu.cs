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
        const int SELECTIONPOINTERSIZE = 20;
        const int MENUENTRYSIZE_X = 100;
        const int MENUENTRYSIZE_Y = 40;
        const int MENUENTRYSPACE = 10;
        const int MENUENTRYNUM = 3;

        // variables used as constants
        Vector2 MENUSTARTPOS = new Vector2(400 - MENUENTRYSIZE_X / 2, 240 - MENUENTRYSIZE_Y);   //"pointer" on first menuentry
        Color MENUENTRYCOLOR = Color.Gray;  //menuentrycolor

        const int MINTIMEKEYPRESS = 150;

        const Keys SELECT_UP = Keys.Up;
        const Keys SELECT_DOWN = Keys.Down;
        const Keys SELECT_ENTRY = Keys.Enter;

        //_____________________________________________________
        public void Load(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            // geneart all rectangles
            selectionPointerTexture = Helper.loadImage("testcar.png");
            menuEntrieTexture = new Texture2D[1];                       //array-size shuld be 5
            menuEntrieTexture[0] = Helper.loadImage("test.png");        //first menuentry
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
                    selectedEntry = (selectedEntry + 2) % MENUENTRYNUM;
                    timeBetweenKeyPress = 0;
                }
                if (keyboardState.IsKeyDown(SELECT_DOWN))
                {
                    selectedEntry = ++selectedEntry % MENUENTRYNUM;
                    timeBetweenKeyPress = 0;
                }
                if (keyboardState.IsKeyDown(SELECT_ENTRY))
                {
                    return EGameStates.InGame;
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
            Vector2 tmpVect = new Vector2(MENUSTARTPOS.X, MENUSTARTPOS.Y);
            // draw entries
            for (int i = 0; i < MENUENTRYNUM; i++)
            {
                spriteBatch.Draw(menuEntrieTexture, tmpVect, MENUENTRYCOLOR);
                tmpVect.Y += MENUENTRYSIZE_Y + MENUENTRYSPACE;
            }

            // draw menuentrypointer
            tmpVect.X -= MENUENTRYSPACE + SELECTIONPOINTERSIZE;
            tmpVect.Y = MENUSTARTPOS.Y + (MENUENTRYSPACE + MENUENTRYSIZE_Y) * selectedEntry;
            spriteBatch.Draw(selectionPointerTexture, tmpVect, Color.White);

            // draw caption
            spriteBatch.DrawString(spriteFont, "Menue", Vector2.Zero, Color.Black);
            spriteBatch.DrawString(spriteFont, "Arrows to change selection, Enter to select", new Vector2(0, 20), Color.Black);
        }

        public void Unload()
        {

        }
    }
}
