using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;


namespace paintRacer
{
    class Physic
    {
        public static Vector2 calculateVelocity(Vector2 pos, float speed);
        public static Vector2 calculateVelocity(bool[,] playerCollisionData, short[,] mapData);
    }
}
