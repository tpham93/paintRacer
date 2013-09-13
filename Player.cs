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

        private Vector2 position;

        public Boolean[] checkPoints;
        public int lap;
        //In radian
        private float rotation;
        private Speed speed;
        //Player texture
        private Texture2D texture;

        private Scene scene;

        //What is the point of this Rectangle?! (Player is supposed to be drawn in the middle of the screen anyways, only width and height are required)
        //Commented out for the time being
        //      -> given size of the car specified by the map
        //private Rectangle textureRectangle;

        //Going to be used to identify player in splitscreen/multiplayer play
        private Color color;

        //Getter for Speed
        public Speed getSpeed()
        {
            return speed;
        }

        //Inconsistency with Level (create with string or texture?)
        public Player(Texture2D texture, Color color, int numCheckPoints)
        {
            checkPoints = new Boolean[numCheckPoints];
            for (int i = 0; i < checkPoints.Length; ++i)
                checkPoints[i] = false;
            lap = 1;

            position = Vector2.Zero;
            rotation = 0.0f;
            speed = new Speed(new Vector2(0f, -1f), 0f);

            this.texture = texture;
            this.color = color;

            collisiondata = getCollisionData(texture);
            scene = null;
        }
        //public static void Init()

        public void Update(GameTime gameTime, bool[] pressedKeys)
        {
            //Console.Out.WriteLine(getRotation());

            Vector2 driverInput=Vector2.Zero;
            if (pressedKeys[(int)Config.controlKeys.Up])
            {
                driverInput.Y++;
            }
            if (pressedKeys[(int)Config.controlKeys.Down])
            {
                driverInput.Y--;
            }
            if (pressedKeys[(int)Config.controlKeys.Left])
            {
                driverInput.X--;
            }
            if (pressedKeys[(int)Config.controlKeys.Right])
            {
                driverInput.X++;
            }
            //Console.WriteLine("Driver input: " +driverInput);
            //Console.WriteLine("Calculated speed: " +speed);
            //Console.WriteLine("Calculated Direction: " + direction);

            speed = Physic.calculateSpeed(gameTime, this, driverInput, scene.getLevel().Data);
            position = Physic.calculateNextPos(gameTime, position, speed);
            rotation = Physic.calculateRotation(speed.direction);

            int num;
            for (num = 0; num < checkPoints.Length && checkPoints[num] == true; ++num);
            Vector2[] pointarray = scene.getLevel().CheckPoints;
            if (num == checkPoints.Length)
            {
                for (int i = 0; i < checkPoints.Length; ++i)
                    checkPoints[i] = false;
                    ++lap;
            }
            else if (Physic.checkPoint(pointarray[2 * num], pointarray[2 * num + 1], this.position))
                checkPoints[num] = true;
        }

        /// get the pixels, which are used for the collision
        /// if they aren't transparent then they will be marked as true in the 2-dimensional bool array
        private static bool[,] getCollisionData(Texture2D texture)
        {
            // get the color information of the player
            Color[] pixels = new Color[texture.Width * texture.Height];
            texture.GetData<Color>(pixels);
            // create the 2-dimensional array for the collisionData
            bool[,] collisionData = new bool[texture.Width, texture.Height];

            // iterate through the whole color information
            for (int x = 0, pixelCounter = 0; x < texture.Width; x++)
            {
                for (int y = 0; y < texture.Height; y++, pixelCounter++)
                {
                    // set true if the color isn't fully transparent
                    collisionData[x, y] = pixels[pixelCounter].A != 0;
                }
            }
            return collisionData;
        }

        ///If player is left null we consider this to be the protagonist of the current viewport, otherwise player is the position this is to be aligned on
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
                spriteBatch.Draw(texture, new Rectangle(viewport.Width / 2, viewport.Height / 2, texture.Width, texture.Height), null, color, 0, new Vector2((float)texture.Width / 2, (float)texture.Height / 2), SpriteEffects.None, 0);
            }
            else
            {
                //Player's draw position: center of the screen - other player's position (this is the point 0,0 on the map) + own position

                //get the vector of the difference of bothe positions
                Vector2 drawPosition =position-player.position;
                //rotate the vector of the difference of both middlepoints by rotation of the other player
                drawPosition = Helper.rotateVector2(drawPosition, -player.getRotation());
                // set drawposition relativly to the middle of the viewport
                drawPosition += new Vector2(viewport.Width / 2, viewport.Height / 2);
                
                // draw in the viewport, relativley to the rotation of the player
                spriteBatch.Draw(texture, new Rectangle((int)drawPosition.X, (int)drawPosition.Y, texture.Width, texture.Height), null, color,rotation-player.getRotation(), new Vector2((float)texture.Width / 2, (float)texture.Height / 2), SpriteEffects.None, 0);
            }

            spriteBatch.End();

            //Restores previous Viewport
            GraphicsDevice.Viewport = defaultView;
        }

        public void setScene(Scene scene)
        {
            this.scene = scene;
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
            speed.direction = new Vector2((float)Math.Sin(rotation), (float)Math.Cos(rotation + MathHelper.Pi));
            this.rotation=rotation;
        }

        public bool[,] getCollisionData()
        {
            return collisiondata;
        }

    }
}
