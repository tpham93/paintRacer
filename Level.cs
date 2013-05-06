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

        public Level(Texture2D texture)
        {
            this.texture = texture;
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //TODO: Get Player Position, Rotation and Window Resolution
            Vector2 position=new Vector2(0, 0);
            float rotation = 0.0f;
            Vector2 resolution=new Vector2(800, 480);

            //Shortening the Draw call
            int width=texture.Bounds.Width;
            int height=texture.Bounds.Height;

            spriteBatch.Begin();
            //Positions texture in the middle of the screen with the Player Rotation set appropriately and the Player Position set as its origin
            spriteBatch.Draw(texture, new Rectangle((int)(resolution.X / 2), (int)(resolution.Y / 2), width, height), null, Color.White, MathHelper.ToRadians(rotation), position, SpriteEffects.None, 0);
            spriteBatch.End();
        }
    }
}
