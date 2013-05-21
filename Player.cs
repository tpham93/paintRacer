using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace paintRacer
{
    class Player
    {
        private bool[,] collisiondata;
        private EventManager eventManager;

        private Vector2 position;
        //In radian
        private float rotation;
        private Vector2 speed;
        //Player texture
        private Texture2D texture;

        //What is the point of this Rectangle?! (Player is supposed to be drawn in the middle of the screen anyways, only width and height are required)
        //Commented out for the time being
        //private Rectangle textureRectangle;

        //Going to be used to identify player in splitscreen/multiplayer play
        private Color color;

        public Player(String filename, /*Rectangle textureRectangle,*/Color color)
        {
            this.texture = Helper.loadImage(filename);
            //this.textureRectangle = textureRectangle;
            this.color = color;
        }
        //public static void Init()
        //public void Update(GameTime gameTime)

        //Splitscreen: Draw calls have to depend on viewports and whether this is the main player in the current viewport
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(texture, new Rectangle(0, 0, texture.Width, texture.Height), color);
            spriteBatch.End();
        }

    }
}
