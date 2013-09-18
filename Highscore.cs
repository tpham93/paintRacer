﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace paintRacer
{
    class Highscore
    {
        const int MAXHIGHSCORE_COUNT = 10;
        List<HighscoreElement> highscoreEntries;
        string path;

        public Highscore(String path)
        {
            this.path = path;
            if (File.Exists(path))
            {
                String[] lines = File.ReadAllLines(path);
                highscoreEntries = new List<HighscoreElement>(MAXHIGHSCORE_COUNT);
                for (int i = 0; i < MAXHIGHSCORE_COUNT && i < lines.Length-1; ++i)
                {
                    highscoreEntries.Add(new HighscoreElement(lines[i]));
                }
                if (lines.Length > 0)
                {
                    int h = Convert.ToInt32(lines[lines.GetUpperBound(0)]);
                    if (h != hash())
                    {
                        throw new Exception("Corrupted Highscore");
                    }
                }
            }
            else
            {
                File.Create(path);
                highscoreEntries = new List<HighscoreElement>();
            }
        }

        private int hash()
        {
            int h = 1;
            for (int i = 0; i < MAXHIGHSCORE_COUNT && i < highscoreEntries.Count; ++i)
            {
                string highscoreString = highscoreEntries[i].ToString();
                int lineValue = 0;
                int sign = 1;
                for (int l = 0; l < highscoreString.Length; ++l)
                {
                    lineValue += sign * highscoreString[l];
                    sign *= -1;
                }
                h += highscoreString.Length * Math.Abs(lineValue);
            }
            return h;
        }

        public void writeToFile()
        {
            TextWriter textWriter = new StreamWriter(path, false);
            
            for (int i = 0; i < MAXHIGHSCORE_COUNT && i < highscoreEntries.Count; ++i)
            {
                textWriter.WriteLine(highscoreEntries[i].ToString());
            }
            textWriter.WriteLine(hash());
            textWriter.Close();
        }

        public bool isInHighscore(HighscoreElement highscoreElement)
        {
            return highscoreEntries.Count < MAXHIGHSCORE_COUNT - 1 || highscoreElement < highscoreEntries[highscoreEntries.Count - 1];
        }

        public void insertScore(HighscoreElement element)
        {
            highscoreEntries.Add(element);
            int index = highscoreEntries.Count - 1;
            while (index > 0 && highscoreEntries[index - 1] > element)
            {
                HighscoreElement tmp = highscoreEntries[index];
                highscoreEntries[index] = highscoreEntries[index - 1];
                highscoreEntries[index - 1] = tmp;
            }
            if (highscoreEntries.Count > MAXHIGHSCORE_COUNT)
            {
                highscoreEntries.RemoveRange(MAXHIGHSCORE_COUNT, highscoreEntries.Count - MAXHIGHSCORE_COUNT - 1);
            }
        }
    }
}