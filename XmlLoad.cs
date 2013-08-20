using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace paintRacer
{
    class XmlLoad
    {
        public static Map parseMapConfig(string filepath)
        {
            Texture2D image = null;
            EMapStates[,] mapData = null;
            Vector2[] checkPoints = null;
            Vector2 startpoint = new Vector2(-1,-1);
            float rotation = 0f;

            XmlTextReader reader = new XmlTextReader(filepath);
            while (reader.Read())
            {
                if (reader.NodeType != XmlNodeType.EndElement)
                {
                    switch (reader.Name)
                    {
                        case "ImageLocation":
                            image = Helper.loadImage(reader.GetAttribute("Address"));
                            break;
                        case "SWImageLocation":
                            mapData = MapReader.createDataFromSWImage(Helper.loadImage(reader.GetAttribute("Address")));
                            break;
                        case "Rotation":
                            rotation = Convert.ToSingle(reader.GetAttribute("Value"));
                            break;
                        case "Checkpoints":
                            checkPoints = parseCheckpoints(reader);
                            break;
                    }
                }
            }
            if (image == null || mapData == null || checkPoints == null || startpoint == new Vector2(-1, -1))
            {
                throw new Exception(filepath + " is an Invalid Config Data.");
            }
            return new Map(image, mapData, checkPoints, startpoint, rotation);
        }

        private static Vector2[] parseCheckpoints(XmlTextReader reader)
        {
            Vector2[] checkpoints = new Vector2[2 * Convert.ToInt32(reader.GetAttribute("Number"))];
            int index = 0;
            while (reader.Read())
            {
                if (reader.NodeType != XmlNodeType.EndElement)
                {
                    if (reader.Name == "CheckPoints")
                        break;
                }
                else
                {
                    switch (reader.Name)
                    {
                        case "CheckPoint":
                            Vector2[] c = parseCheckpoint(reader);
                            checkpoints[index++] = c[0];
                            checkpoints[index++] = c[1];
                            break;
                    }
                }
            }
            return null;
        }


        private static Vector2[] parseCheckpoint(XmlTextReader reader)
        {
            Vector2[] checkpoint = new Vector2[2];
            int index = 0;
            while (reader.Read())
            {
                if (reader.NodeType != XmlNodeType.EndElement)
                {
                    if (reader.Name == "CheckPoint")
                        break;
                }
                else
                {
                    switch (reader.Name)
                    {
                        case "Point":
                            checkpoint[index++] = parsePoint(reader);
                            break;
                    }
                }
            }
            return checkpoint;
        }

        private static Vector2 parsePoint(XmlTextReader reader)
        {
            int x;
            int y;
            x = Convert.ToInt32(reader.GetAttribute("X"));
            y = Convert.ToInt32(reader.GetAttribute("Y"));
            return new Vector2(x, y);
        }
    }
}
