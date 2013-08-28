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
        //// enum for special values to save collision and air
        //public enum specialEventValues
        //{
        //    nothing = -1,
        //    collision = -2
        //}

        // enum to select of which type to load
        public enum MapType
        {
            rawImage,
            customMap
        }
        private Texture2D texture;
        private EMapStates[,] mapData;


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
                    mapData = MapReader.createDataFromSWImage(texture);
                    break;
                // load a custom map
                case MapType.customMap:
                    break;
                default:
                    break;
            }
            this.mapType = mapType;
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice, Viewport viewport, Player player)
        {
            //Shortening the Draw call
            int width = texture.Bounds.Width;
            int height = texture.Bounds.Height;

            //Assumes GraphicsDevice previously contained default Viewport
            Viewport defaultView = GraphicsDevice.Viewport;

            //Switches to the given Viewport
            GraphicsDevice.Viewport = viewport;

            spriteBatch.Begin();
            //Positions texture in the middle of the screen with the Player Rotation set appropriately and the Player Position set as its origin
            spriteBatch.Draw(texture, new Rectangle((int)(viewport.Width / 2), (int)(viewport.Height / 2), width, height), null, Color.White, -player.getRotation(), player.getPosition(), SpriteEffects.None, 0);
            spriteBatch.End();

            //Restores previous Viewport
            GraphicsDevice.Viewport = defaultView;
        }

        public EMapStates[,] getMapData()
        {
            return mapData;
        }
    }
}
