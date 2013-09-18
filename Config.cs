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
