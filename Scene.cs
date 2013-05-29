using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace paintRacer
{
    class Scene
    {
        private Level level;
        private Player[] players;
        private Viewport[] viewports;
        private Keys[,] keys;
        private int playerCount;

        public Scene(Level level, Player[] players, Viewport defaultView, Keys[,] keys)
        {
            this.level = level;

            playerCount = Math.Min(players.Length, keys.GetLength(0));            

            if (playerCount == 0)
            {
                throw new Exception("Couldn't create Scene, playerCount is 0.");
            }
            else
            {
                this.players = new Player[playerCount];
                this.viewports = new Viewport[playerCount];
                viewports = Helper.createViewports(playerCount, defaultView);
                this.keys = new Keys[playerCount, keys.GetLength(1)];
                for (int i = 0; i < playerCount; i++)
                {
                    this.players[i] = players[i];
                    this.viewports[i] = viewports[i];
                    for (int j = 0; j < keys.GetLength(1); j++)
                    {
                        this.keys[i, j] = keys[i, j];
                    }
                }
            }
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            
            for (int i = 0; i < playerCount; i++)
            {
                //Splits up keys Array into separate Arrays for each player which contain whether a key was pressed or not (this is done to simplify the Update of each player)
                bool[] pressedKeys = new bool[keys.GetLength(1)];
                for (int j = 0; j < keys.GetLength(1); j++)
                {
                    pressedKeys[j] = keyboardState.IsKeyDown(keys[i, j]);
                }
                players[i].Update(gameTime, pressedKeys);
            }
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice)
        {
            for (int i = 0; i < players.Length; i++)
            {
                level.Draw(spriteBatch, GraphicsDevice, viewports[i], players[i]);
                // draw cars
                for (int j = 0; j < players.Length; j++)
                {
                    if (i == j)
                    {
                        // draw the i-th players car
                        players[j].Draw(spriteBatch, spriteBatch.GraphicsDevice, viewports[i]);
                    }
                    else
                    {
                        // draw the other car on the viewport
                        players[j].Draw(spriteBatch, spriteBatch.GraphicsDevice, viewports[i], players[i]);
                    }
                }
            }
        }
    }
}
