using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace paintRacer
{
    class XMLSave
    {
        public static string getDirectoryName(string mapName)
        {
            string mapPath = @"saved_maps\" + mapName;
            int i = 0;
            while (Directory.Exists(mapPath))
            {
                mapPath = @"saved_maps\" + mapName + "_" + ++i;
            }
            return mapName + "_" + i;
        }

        public static void saveMap(string directoryPath, Map map)
        {
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
            writeXML(directoryPath, map);

            saveImages(directoryPath, map);
        }

        public static void updateMapFile(string directoryPath, Map map)
        {
            writeXML(directoryPath, map);
        }

        private static void writeXML(string mapPath, Map map)
        {
            TextWriter fs = new StreamWriter(mapPath + @"\map.xml");
            Vector2 start = map.Start;
            Vector2[] checkPoints = map.CheckPoints;
            fs.WriteLine("<Map>");

            fs.WriteLine("\t<ImageLocation Address=\"image.png\"/>");
            fs.WriteLine("\t<SWImageLocation Address=\"imageSW.png\"/>");
            fs.WriteLine("\t<Highscore Address=\"highscore\"/>");
            fs.WriteLine("\t<Rotation Value=\"" + map.StartRotation + "\"/>");
            fs.WriteLine("\t<Startpoint X=\"" + start.X + "\" Y=\"" + start.Y + "\" />");
            fs.WriteLine("\t<Checkpoints Number=\"" + checkPoints.Length / 2 + "\">");

            for (int i = 0; i < checkPoints.Length / 2; ++i)
            {
                Vector2 checkPoint1 = checkPoints[2 * i];
                Vector2 checkPoint2 = checkPoints[2 * i + 1];
                fs.WriteLine("\t\t<Checkpoint>");
                fs.WriteLine("\t\t\t<Point X=\"" + checkPoint1.X + "\" Y=\"" + checkPoint1.Y + "\" />");
                fs.WriteLine("\t\t\t<Point X=\"" + checkPoint2.X + "\" Y=\"" + checkPoint2.Y + "\" />");
                fs.WriteLine("\t\t</Checkpoint>");
            }

            fs.WriteLine("\t</Checkpoints>");
            fs.WriteLine("</Map>");

            fs.Close();
        }

        private static void saveImages(string mapPath, Map map)
        {
            FileStream fsImage = new FileStream(mapPath + @"\image.png", FileMode.CreateNew);
            Texture2D image = map.Image;
            image.SaveAsPng(fsImage, image.Width, image.Height);
            fsImage.Close();
            FileStream fsImageSW = new FileStream(mapPath + @"\imageSW.png", FileMode.CreateNew);
            Texture2D imageSW = map.SwImage;
            imageSW.SaveAsPng(fsImageSW, imageSW.Width, imageSW.Height);
            fsImageSW.Close();
        }
    }
}
