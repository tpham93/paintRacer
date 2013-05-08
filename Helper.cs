using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace paintRacer
{
    class Helper
    {
        public static Texture2D loadImage(String filename, GraphicsDevice graphicsDevice)
        {
            //Might throw FileNotFoundException
            FileStream fileStream = new FileStream(filename, FileMode.Open);
            Texture2D res = Texture2D.FromStream(graphicsDevice, fileStream);
            fileStream.Close();
            return res;
        }
    }
}
