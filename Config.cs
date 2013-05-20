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
        private static Keys keyUp;
        private static Keys keyDown;
        private static Keys keyLeft;
        private static Keys keyRight;
        private static Keys keyPause;
        private static Keys keySelect;
        private static Keys keysBack;
    }
}
