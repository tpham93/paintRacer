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
        private int playercount;

        public Scene(Level level, Player[] players, Viewport[] viewports/*, Keys[,] keys*/)
        {
            this.level = level;

            playercount = Math.Min(players.Length, viewports.Length);
            //playercount = Math.Min(playercount, keys.GetLength(0));

            if (playercount == 0)
            {

            }
            else
            {
                this.players = new Player[playercount];
                this.viewports = new Viewport[playercount];
                //this.keys = new Keys[playercount, keys.GetLength(1)];
                for (int i = 0; i < playercount; i++)
                {
                    this.players[i] = players[i];
                    this.viewports[i] = viewports[i];
                    /*for (int j = 0; j < keys.GetLength(1); j++)
                    {
                        this.keys[i, j] = keys[i, j];
                    }*/
                }
            }
        }

        public void Update(GameTime gameTime)
        {

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
