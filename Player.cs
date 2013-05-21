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

        public Player(String filename, Color color)
        {
            position = new Vector2(0, 0);
            rotation = 0.0f;

            this.texture = Helper.loadImage(filename);
            this.color = color;
        }
        //public static void Init()
        //public void Update(GameTime gameTime)

        //If player is left null we consider this to be the protagonist of the current viewport, otherwise player is the position this is to be aligned on
        public void Draw(SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice, Viewport viewport, Player player=null)
        {
            //Assumes GraphicsDevice previously contained default Viewport
            Viewport defaultView = GraphicsDevice.Viewport;

            //Switches to the given Viewport
            GraphicsDevice.Viewport = viewport;

            spriteBatch.Begin();
            
            //Player's rectangle size based on his texture size and origin currently in the center
            if(player==null)
            {
                spriteBatch.Draw(texture, new Rectangle(viewport.Width / 2, viewport.Height / 2, texture.Width, texture.Height), null, color, rotation, new Vector2((float)texture.Width/2, (float)texture.Height/2), SpriteEffects.None, 0);
                //spriteBatch.Draw(texture, new Rectangle(viewport.Width / 2, viewport.Height / 2, texture.Width, texture.Height), null, color, 0.0f, new Vector2((float)texture.Width / 2, (float)texture.Height / 2), SpriteEffects.None, 0);
            }
            else
            {
                //Player's draw position: center of the screen - other player's position (this is the point 0,0 on the map) + own position
                spriteBatch.Draw(texture, new Rectangle((int)(viewport.Width / 2 - player.getPosition().X + position.X), (int)(viewport.Height / 2 - player.getPosition().Y + position.Y), texture.Width, texture.Height), null, color, rotation, new Vector2((float)texture.Width / 2, (float)texture.Height / 2), SpriteEffects.None, 0);
                //spriteBatch.Draw(texture, new Rectangle((int)(viewport.Width / 2 - player.getPosition().X + position.X), (int)(viewport.Height / 2 - player.getPosition().Y + position.Y), texture.Width, texture.Height), null, color, 0.0f, new Vector2((float)texture.Width / 2, (float)texture.Height / 2), SpriteEffects.None, 0);
            }

            spriteBatch.End();

            //Restores previous Viewport
            GraphicsDevice.Viewport = defaultView;
        }

        public Vector2 getPosition()
        {
            return position;
        }

        public void setPosition(Vector2 position)
        {
            this.position=position;
        }

        public float getRotation()
        {
            return rotation;
        }

        public void setRotation(float rotation)
        {
            this.rotation=rotation;
        }
    }
}
