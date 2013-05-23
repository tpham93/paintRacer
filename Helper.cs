using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace paintRacer
{
    class Helper
    {
        private static GraphicsDevice graphicsDevice;

        // set graphicsDevice
        public static void Init(GraphicsDevice graphicsDevice)
        {
            Helper.graphicsDevice = graphicsDevice;
        }


        public static Texture2D loadImage(String filename,Rectangle rect = new Rectangle(), Color color =new Color())
        {
            //Might throw FileNotFoundException
            FileStream fileStream = new FileStream(filename, FileMode.Open);
            Texture2D texture = Texture2D.FromStream(graphicsDevice, fileStream);
            fileStream.Close();
            // returns original car if the rectangle has no size
            if (rect.X <= 0 || rect.Y <= 0)
            {
                return texture;
            }
            else
            {
                // rendertarget to save the resized image
                RenderTarget2D renderTarget = new RenderTarget2D(graphicsDevice,rect.Width,rect.Height);
                // create  spriteBatch to draw the resized image
                SpriteBatch spriteBatch = new SpriteBatch(graphicsDevice);
                // save the earlier rendertargets
                RenderTargetBinding[] tmpRenderTargets = graphicsDevice.GetRenderTargets();
                graphicsDevice.SetRenderTarget(renderTarget);
                // draw the resized image on the rendertarget
                spriteBatch.Begin();
                if (color.Equals(new Color()))
                {
                    spriteBatch.Draw(texture, rect, Color.White);
                }
                else
                {
                    spriteBatch.Draw(texture, rect, color);
                }
                spriteBatch.End();
                // set back the saved rendertargets
                graphicsDevice.SetRenderTargets(tmpRenderTargets);
                // return the resized drawn Iimage
                return renderTarget;
            }
        }

        // rotates vector
        public static Vector2 rotateVector2(Vector2 vector, float rotation)
        {
            // multplicate vector with rotationmatrix
            // (cos(a) -sin(a))
            // (sin(a)  cos(a))
            return rotateVector2(vector, Math.Cos(rotation), Math.Sin(rotation));
        }
        // rotates vector
        public static Vector2 rotateVector2(Vector2 vector, double rotationCos, double rotationSin)
        {
            // multplicate vector with rotationmatrix
            // (cos(a) -sin(a))
            // (sin(a)  cos(a))
            return new Vector2((int)(0.5f + vector.X * rotationCos + vector.Y * -rotationSin), (int)(0.5f + vector.X * rotationSin + vector.Y * rotationCos));
        }
    }
}
