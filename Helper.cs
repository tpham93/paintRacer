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

        public static Texture2D loadImage(String filename)
        {
            //Might throw FileNotFoundException
            FileStream fileStream = new FileStream(filename, FileMode.Open);
            Texture2D res = Texture2D.FromStream(graphicsDevice, fileStream);
            fileStream.Close();
            return res;
        }

        // rotates vector
        public static Vector2 rotateVector2(Vector2 vector, float rotation)
        {
            // multplicate vector with rotationmatrix
            // (cos(a) -sin(a))
            // (sin(a)  cos(a))
            //return new Vector2((int)(0.5f + vector.X * Math.Cos(rotation) + vector.Y * -Math.Sin(rotation)), (int)(0.5f + vector.X * Math.Sin(rotation) + vector.Y * Math.Cos(rotation)));
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
