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
         * returns :
         * new Vector2
         */
        public static Vector2 calculateNextPos(Vector2 pos, Vector2 speed)
        {
            return new Vector2(pos.X + speed.X, pos.Y + speed.Y);
        }

        /*returns the updated speed of the car
         * 
         * speed - Vector2 :
         * speed of the car
         * 
         * driverInput - Vector2 :
         * direction witch is givven by the driver
         * x: left(-1) strate on(0) right(1)
         * y: accelorate(1) roll(0) bark(-1)
         * 
         * returns :
         * new Vector2
         */
        //public static Vector2 calculateSpeed(Vector2 speed, Vector2 dirverInput)

        /*returns if there is an collosion
         * 
         * playerCollisionData - bool[,] :
         * defines were in the box are parts of the car
         * 
         * rotation - float :
         * defines the rotation of the car
         * !IN RADIANT!
         * 
         * mapData - short[,] :
         * defines the type of the Points in the map
         * 
         * returns :
         * bool
         */
        //private static bool hasCollision(bool[,] playerCollisionData, float rotation, short[,] mapData)
    }
}
