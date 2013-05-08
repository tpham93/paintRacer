using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;


namespace paintRacer
{
    class Physic
    {
        public static Vector2 calculateAcceleration(Vector2 pos, Vector2 speed);
        public static Vector2 hasCollision(bool[,] playerCollisionData, short[,] mapData);
    }
}
