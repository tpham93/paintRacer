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
        private static Point screenResolution;

        // available Keys (Restructure as enum/map? Make more abstract to fit multiplayer?)

        enum controlKeys
        {
            Up      =0,
            Down    =1,
            Left    =2,
            Right   =3,
            Pause   =4,
            Select  =5,
            Back    =6
        }

        // 1st dimension -> playerid , 2nd dimension -> controlKeys value
        Keys[,] playerKeys;
    }
}
