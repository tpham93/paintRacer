using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;


namespace paintRacer
{
    class Physic
    {
        /*returns the new position of the car
         * 
         * pos - Vector2 :
         * position of the car
         * 
         * speed - Vector2 :
         * speed of the car
         * 
         * acceleration - Vector2 :
         * acceleration of the car
         */
        public static Vector2 calculateNextPos(Vector2 pos, Vector2 speed, Vector2 acceloration);

        /*returns if there is an collosion
         * 
         * playerCollisionData - bool[,] :
         * defines were in the box are parts of the car
         * 
         * mapData - short[,] :
         * defines the type of the Points in the map
         */
        private static bool hasCollision(bool[,] playerCollisionData, short[,] mapData);
    }
}
