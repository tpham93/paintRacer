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
        private static Vector2 ROAD_COLOR = new Vector2(200, 255);
        private static Vector2 OFFROAD_COLOR = new Vector2(100, 199);
        private static Vector2 OBJECT_COLOR = new Vector2(0, 99);

        /// <summary>
        /// create a 2D-Array of EMapStates from an sw image
        /// </summary>
        /// 
        /// <param name="image">
        /// an sw image to read the data
        /// </param>
        /// 
        /// <returns>
        /// an 2D-Array of EMapStates
        /// </returns>
        /// 
        public static EMapStates[,] createDataFromSWImage(Texture2D image)
        {
            int width = image.Width;
            int height = image.Height;
            EMapStates[,] result = new EMapStates[width, height];

            Color[] colorData = new Color[image.Width * image.Height];
            image.GetData<Color>(colorData);

            for (int i = 0; i < colorData.Length; i++)
            {
                int d_color = (colorData[i].B + colorData[i].G + colorData[i].R) / 3;
                if (d_color <= ROAD_COLOR.Y && d_color >= ROAD_COLOR.X)
                    result[i % width, i / width] = EMapStates.Road;
                else if (d_color <= OFFROAD_COLOR.Y && d_color >= OFFROAD_COLOR.X)
                    result[i % width, i / width] = EMapStates.Offroad;
                else if (d_color <= OBJECT_COLOR.Y && d_color >= OBJECT_COLOR.X)
                    result[i % width, i / width] = EMapStates.Object;
                else result[i % width, i / width] = EMapStates.Default;
            }
            return result;
        }
    }
}
