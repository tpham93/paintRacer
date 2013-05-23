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
       
        private Player[] allPlayers;

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
                    mapData = mapReader.getRawImageInformation(texture, Color.Black);
                    break;
                // load a custom map
                case MapType.customMap:
                    break;
                default:
                    break;
            }
            this.mapType = mapType;
        }

        public void setPlayers(string[] filename, Color[] playerColor)
        {
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
        }

        public void Update(GameTime gameTime)
        {
            // change the rotations (test)
            for (int i = 0; i < allPlayers.Length; i++)
            {
                allPlayers[i].setRotation(allPlayers[i].getRotation() + (i + 1) * 0.0002f);
            }
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice, Viewport[] viewports/*, Player player*/)
        {
            // check the viewport's number
            if (viewports.Length != allPlayers.Length)
            {
                throw new Exception("number of viewports is too "+((viewports.Length < allPlayers.Length)?"low":"high")+"!");
            }
            //Shortening the Draw call
            int width = texture.Bounds.Width;
            int height = texture.Bounds.Height;

            //Assumes GraphicsDevice previously contained default Viewport
            Viewport defaultView = GraphicsDevice.Viewport;
            // draw the level
            for (int i = 0; i < allPlayers.Length; i++)
            {
                spriteBatch.Begin();
                // set viewport
                GraphicsDevice.Viewport = viewports[i];

                //Positions texture in the middle of the screen with the Player Rotation set appropriately and the Player Position set as its origin
                spriteBatch.Draw(texture, new Rectangle((int)(viewports[i].Width / 2), (int)(viewports[i].Height / 2), width, height), null, Color.White, -allPlayers[i].getRotation(), allPlayers[i].getPosition(), SpriteEffects.None, 0);

                spriteBatch.End();

                // draw cars
                for (int j = 0; j < allPlayers.Length; j++)
                {
                    if (j != i)
                        // draw the other car on the viewport
                        allPlayers[j].Draw(spriteBatch, spriteBatch.GraphicsDevice, viewports[i], allPlayers[i]);
                }
                // draw the i-th players car
                allPlayers[i].Draw(spriteBatch, spriteBatch.GraphicsDevice, viewports[i]);
            }
            //Restores previous Viewport
            GraphicsDevice.Viewport = defaultView;
        }
    }
}
