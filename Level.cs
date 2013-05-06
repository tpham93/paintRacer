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
            //TODO: Get Player Position and Rotation

            float rotation = 0.0f;

            //Shortening the Draw call
            int width=texture.Bounds.Width;
            int height=texture.Bounds.Height;

            spriteBatch.Begin();
            spriteBatch.Draw(texture, new Rectangle(0, 0, width, height), null, Color.White, rotation, new Vector2(width / 2, height / 2), SpriteEffects.None, 0);
            spriteBatch.End();
        }
    }
}
