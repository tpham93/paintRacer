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
        private Texture2D texture;

        public Level(String filename)
        {
            texture = Helper.loadImage(filename);
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice, Viewport viewport)
        {
            //TODO: Get Player Position, Rotation and Window Resolution
            Vector2 position=new Vector2(0, 0);
            float rotation = 0;

            //Shortening the Draw call
            int width=texture.Bounds.Width;
            int height=texture.Bounds.Height;

            //Assumes GraphicsDevice previously contained default Viewport
            Viewport defaultView = GraphicsDevice.Viewport;

            //Switches to the given Viewport
            GraphicsDevice.Viewport = viewport;

            spriteBatch.Begin();
            //Positions texture in the middle of the screen with the Player Rotation set appropriately and the Player Position set as its origin
            spriteBatch.Draw(texture, new Rectangle((int)(viewport.Width / 2), (int)(viewport.Height / 2), width, height), null, Color.White, rotation, position, SpriteEffects.None, 0);
            spriteBatch.End();

            //Restores previous Viewport
            GraphicsDevice.Viewport = defaultView;
        }
    }
}
