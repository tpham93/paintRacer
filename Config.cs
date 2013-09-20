using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
namespace paintRacer
{
    class Config
    {
        //consts for game and GUI
        public const Color TEXT_BOX_COLOR = Color.Moccasin;
        public const int TEXTFIELD_BORDER = 5;
        public const Color TEXT_COLOR = Color.Black;
        public const Vector2 BIG_BUTTON = new Vector2(187, 75);
        public const Vector2 SMALL_BUTTON = new Vector2(125, 50);
        public const int BIG_BUTTON_SPACE = 15;
        public const int SMALL_BUTTON_SPACE = 5;
        public const int LINE_SIZE = 20;
        public const int BIG_LINE_SPACE = 5;
        public const int SMALL_LINE_SPACE = 2;
        public const TimeSpan TIME_BETWEEN_SAME_EVENT = new TimeSpan(0, 0, 0, 0, 150);
        
        
        // available Keys (Restructure as enum/map? Make more abstract to fit multiplayer?)

        public enum controlKeys
        {
            Up      =0,
            Down    =1,
            Left    =2,
            Right   =3,
            Pause   =4,
            Select  =5,
            Back    =6
        }

        // List -> playerid , 2nd dimension -> controlKeys value
        private static Keys[,] playerKeys;

        public static Keys[,] getKeys()
        {
            return playerKeys;
        }

        public static void setKeys(Keys[,] keys)
        {
            playerKeys=new Keys[keys.GetLength(0), keys.GetLength(1)];
            for (int i = 0; i < keys.GetLength(0); i++)
            {
                for (int j = 0; j < keys.GetLength(1); j++)
                {
                    playerKeys[i, j] = keys[i, j];
                }
            }
        }
    }
}
