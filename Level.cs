using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace paintRacer
{
    class Level
    {
        // enum for special values to save collision and air
        public enum specialEventValues
        {
            nothing = -1,
            collision = -2
        }

        // enum to select of which type to load
        public enum MapType
        {
            rawImage,
            customMap
        }
        private Texture2D texture;
        private sbyte[,] mapData;


        // shows whether this map is a custom map or just an image
        private MapType mapType;

        public Level(String filename, MapType mapType)
        {
            //look if the input is just an image or an customMap
            switch (mapType)
            {
                // load a raw image
                case MapType.rawImage:
                    texture = Helper.loadImage(filename);
                    mapData = MapReader.getRawImageInformation(texture, Color.Black);
                    break;
                // load a custom map
                case MapType.customMap:
                    break;
                default:
                    break;
            }
            this.mapType = mapType;
        }

        //Future member of Scene.cs
        /*public void setPlayers(string[] filename, Color[] playerColor)
        {
            //Alternative solution: create arrays with smallest size

            // throw exception if the number of arrays aren't the same
            if (filename.Length != playerColor.Length)
            {
                throw new Exception("wrong number of texturefilepathes and colors");
            }

            // initialize the array for the player
            allPlayers = new Player[playerColor.Length];
            for (int i = 0; i < allPlayers.Length; i++)
            {
                // create all players with their specific color
                allPlayers[i] = new Player(Helper.loadImage(filename[i]), playerColor[i], this);
            }
        }*/

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice, Viewport viewport, Player player)
        {
            //About to be checked on Scene creation (see setPlayers())
            /*if (viewports.Length != allPlayers.Length)
            {
                throw new Exception("number of viewports is too " + ((viewports.Length < allPlayers.Length) ? "low" : "high") + "!");
            }*/

            //Shortening the Draw call
            int width = texture.Bounds.Width;
            int height = texture.Bounds.Height;

            //Assumes GraphicsDevice previously contained default Viewport
            Viewport defaultView = GraphicsDevice.Viewport;

            //Switches to the given Viewport
            GraphicsDevice.Viewport = viewport;

            //Part of Scene
            /*for (int i = 0; i < allPlayers.Length; i++)
            {
                // draw cars
                for (int j = 0; j < allPlayers.Length; j++)
                {
                    if (j != i)
                        // draw the other car on the viewport
                        allPlayers[j].Draw(spriteBatch, spriteBatch.GraphicsDevice, viewports[i], allPlayers[i]);
                }
                // draw the i-th players car
                allPlayers[i].Draw(spriteBatch, spriteBatch.GraphicsDevice, viewports[i]);
            }*/

            spriteBatch.Begin();
            //Positions texture in the middle of the screen with the Player Rotation set appropriately and the Player Position set as its origin
            spriteBatch.Draw(texture, new Rectangle((int)(viewport.Width / 2), (int)(viewport.Height / 2), width, height), null, Color.White, -player.getRotation(), player.getPosition(), SpriteEffects.None, 0);
            spriteBatch.End();

            //Restores previous Viewport
            GraphicsDevice.Viewport = defaultView;
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice, Viewport[] viewports)
        {
            
        }
    }
}
