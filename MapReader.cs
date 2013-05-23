using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace paintRacer
{
    class MapReader
    {
        public static sbyte[,] getRawImageInformation(Texture2D map, Color collisionColour)
        {
            // initializes vector
            sbyte[,] mapData = new sbyte[map.Width, map.Height];
            //the pixels of the texture
            Color[] colorData = new Color[map.Width * map.Height];
            map.GetData<Color>(colorData);
            //go through the whole texture
            for (int x = 0, colordataCounter = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++, colordataCounter++)
                {
                    // check if it has collision
                    mapData[x, y] = (colorData[colordataCounter].Equals(collisionColour)) ? (sbyte)Level.specialEventValues.collision : (sbyte)Level.specialEventValues.nothing;
                }
            }
            return mapData;
        }
    }
}
