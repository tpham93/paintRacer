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

        public static sbyte[,] getRawImageInformation(Texture2D map, Color collisionColour)
        {
            // initializes vector
            sbyte[,] mapData = new sbyte[map.Width, map.Height];
            //the pixels of the texture
            Color[] colorData = new Color[map.Width * map.Height];
            map.GetData<Color>(colorData);
            //go through the whole texture
            for (int y = 0, colordataCounter = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++, colordataCounter++)
                {
                    // check if it has collision
                    mapData[x, y] = (colorData[colordataCounter].Equals(collisionColour)) ? (sbyte)Level.specialEventValues.collision : (sbyte)Level.specialEventValues.nothing;

                }
                Console.WriteLine();
            }
            return mapData;
        }

        /// <summary>
        /// static function to load new map
        /// </summary>
        /// 
        /// <param name="imageFile">
        /// the file of the showen map
        /// </param>
        /// 
        /// <param name="swImageForData">
        /// the file of the map to readout data
        /// </param>
        /// 
        /// <param name="checkPoints">
        /// the checkpoints selected by the user
        /// </param>
        /// 
        /// <param name="startPoint">
        /// the point were the cars start
        /// </param>
        /// 
        /// <param name="startDirection">
        /// the direction (as vector) in which the cars start
        /// </param>
        /// 
        /// <returns>
        /// a new map to play on
        /// </returns>
        /// 
        public static Map createMap(string imageFile, string swImageForData, Vector2[] checkPoints, Vector2 startPoint, Vector2 startDirection)
        {
            Texture2D image = Helper.loadImage(imageFile);
            EMapStates[,] data = createDataFromSWImage(Helper.loadImage(swImageForData));
            float rotation = Physic.calculateRotation(startDirection);
            return new Map(image, data, checkPoints, startPoint, rotation);
        }

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
                    result[i % height, i / height] = EMapStates.Road;
                else if (d_color <= OFFROAD_COLOR.Y && d_color >= OFFROAD_COLOR.X)
                    result[i % height, i / height] = EMapStates.Offroad;
                else if (d_color <= OBJECT_COLOR.Y && d_color >= OBJECT_COLOR.X)
                    result[i % height, i / height] = EMapStates.Object;
                else result[i % height, i / height] = EMapStates.Default;
            }
            return result;
        }

        /// TODO: implement a function to load maps from Files
        public Map readMapFromFile(string filepath)
        {
            return XmlLoad.parseMapConfig(filepath);
        }

        /// TODO: implement a function to save maps in Files
        /*public void saveMapInFile(string filepath, Map map)
        {
        }*/
    }
}
