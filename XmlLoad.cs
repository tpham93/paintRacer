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
        public static Map parseMapConfig(string directoryPath)
        {
            string filepath = directoryPath + @"\map.xml";

            bool hasToBeUpdated = false;

            Texture2D image = null;
            Texture2D swImage = null;
            HighscoreData highscore = null;
            Vector2[] checkPoints = null;
            Vector2 startpoint = new Vector2(-1, -1);
            float rotation = 0f;

            XmlTextReader reader = new XmlTextReader(filepath);
            while (reader.Read())
            {
                if (reader.NodeType != XmlNodeType.EndElement)
                {
                    switch (reader.Name)
                    {
                        case "ImageLocation":
                            image = Helper.loadImage(directoryPath + '\\' + reader.GetAttribute("Address"));
                            break;
                        case "SWImageLocation":
                            swImage = Helper.loadImage(directoryPath + '\\' + reader.GetAttribute("Address"));
                            break;
                        case "Highscore":
                            highscore = new HighscoreData(directoryPath + '\\' + reader.GetAttribute("Address"));
                            break;
                        case "Rotation":
                            rotation = Convert.ToSingle(reader.GetAttribute("Value").Replace('.', ','));
                            break;
                        case "Startpoint":
                            startpoint = parsePoint(reader);
                            break;
                        case "Checkpoints":
                            checkPoints = parseCheckpoints(reader);
                            break;
                    }
                }
            }

            reader.Close();

            if (checkPoints == null)
            {
                checkPoints = new Vector2[0];
                hasToBeUpdated = true;
            }

            if (highscore == null)
            {
                highscore = new HighscoreData(directoryPath + @"\highscore");
                hasToBeUpdated = true;
            }

            if (image == null || swImage == null || startpoint == new Vector2(-1, -1))
            {
                throw new Exception(filepath + " is an Invalid Config Data.");
            }

            Map map = new Map(image, swImage, highscore, checkPoints, startpoint, rotation);
            if (hasToBeUpdated)
                XMLSave.updateMapFile(directoryPath, map);

            return map;
        }

        private static Vector2[] parseCheckpoints(XmlTextReader reader)
        {
            Vector2[] checkpoints = new Vector2[2 * Convert.ToInt32(reader.GetAttribute("Number"))];
            int index = 0;
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.EndElement)
                {
                    if (reader.Name == "Checkpoints")
                        break;
                }
                else
                {
                    switch (reader.Name)
                    {
                        case "Checkpoint":
                            Vector2[] c = parseCheckpoint(reader);
                            checkpoints[index++] = c[0];
                            checkpoints[index++] = c[1];
                            break;
                    }
                }
            }
            return checkpoints;
        }


        private static Vector2[] parseCheckpoint(XmlTextReader reader)
        {
            Vector2[] checkpoint = new Vector2[2];
            int index = 0;
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.EndElement)
                {
                    if (reader.Name == "Checkpoint")
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
