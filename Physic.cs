using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;


namespace paintRacer
{
    class Physic
    {
        //const VAR
        private const float MASS = 750; //mass of the car in kg
        private const float MAX_FORCE_ACCELERAT = 150000f; //max force of the car in N = kg*m/s²
        private const float MAX_FORCE_BARK = -1.5f * MAX_FORCE_ACCELERAT; //max force by barking in N = kg*m/s²
        private const float WHEEL_RADIUS = 0.25f; //radius of wheels in m
        private const float STEARING = 0.0015f; //if you stear left or right

        private const float ROLL_FRICTION_STREET = 400f; //roll-friction stops the car in m
        private const float ROLL_FRICTION_GRASS = 1400f; //roll-friction stops the car in m
        //private const float STATIC_FRICTION_STREET = 0.9f; //helps the car to stay on road in 1
        //private const float STATIC_FRICTION_GRASS = 0.6f; //helps the car to stay on road in 1
        private const float G = 9.81f; //gravitational acceleration in m/s²

        /**
         * returns the new position of the car
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
        public static Vector2 calculateNextPos(GameTime gameTime, Vector2 pos, Speed speed)
        {
            speed.direction.Normalize();
            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;
            return new Vector2(pos.X + (speed.direction.X) * speed.abs * time, pos.Y + (speed.direction.Y) * speed.abs * time);
        }

        
        /// <summary>
        /// returns the updated speed of the car
        /// </summary>
        ///
        /// <param name="gameTime"> - GameTime :
        /// the game time
        /// </param>
        ///
        /// <param name="speed"> - Vector2 :
        /// speed of the car
        /// </param>
        ///
        /// <param name="driverInput"> - Vector2 :
        /// direction witch is given by the driver
        /// x: left(&lt;0) straight on(0) right(&gt;0)
        /// y: accelerate(&gt;0) roll(0) bark(&lt;0)
        /// </param>
        ///
        /// <param name="mapState"> - EMapStates :
        /// the type of undergound the mid of the car is on
        /// </param>
        ///
        /// <returns> new Vector2
        /// </returns>
        public static Speed calculateSpeed(GameTime gameTime, Player player, Vector2 driverInput, EMapStates[,] mapdata)
        {
            //Console.WriteLine("driverInput: [" + driverInput.X + ";" + driverInput.Y + "]"); 
            EMapStates mapState = mapdata[(int)player.getPosition().X, (int)player.getPosition().Y];
            Speed speed = player.getSpeed();

            if (hasCollision(player.getPosition(), player.getCollisionData(), player.getRotation(), mapdata) 
                return new Speed(speed.direction, 0f);

            Vector2 accelaration = new Vector2(speed.direction.X, speed.direction.Y);
            accelaration.Normalize();

            Vector2 sideMove;
            if (driverInput.X > 0)
            {
                sideMove = new Vector2(-accelaration.Y, accelaration.X);
                sideMove.Normalize();
            }
            else if (driverInput.X < 0)
            {
                sideMove = new Vector2(accelaration.Y, -accelaration.X);
                sideMove.Normalize();
            }
            else
            {
                sideMove = new Vector2(0f, 0f);
            }

            //frictions Fr =          µrr           * g  /     r
            float rollFrictionForce = (((mapState == EMapStates.Road) ? ROLL_FRICTION_STREET : ROLL_FRICTION_GRASS) * G / WHEEL_RADIUS);

            //set acceleration force
            float accelerationForce = 0f;
            if ((driverInput.Y > 0) && (speed.abs >= 0))
                accelerationForce = MAX_FORCE_ACCELERAT / (float)((Math.Abs(speed.abs) / 100) + 1);
            else if ((driverInput.Y > 0) && (speed.abs < 0))
                accelerationForce = -MAX_FORCE_BARK;
            else if ((driverInput.Y < 0) && (speed.abs > 0))
                accelerationForce = MAX_FORCE_BARK;
            else if ((driverInput.Y < 0) && (speed.abs <= 0))
                accelerationForce = -MAX_FORCE_ACCELERAT / (float)((Math.Abs(speed.abs) / 100) + 1);

            //                                         a
            //        v       =  v        +  ((      F         / m  )*                    t                       )
            float absNewSpeed = speed.abs + ((accelerationForce / MASS) * (float)gameTime.ElapsedGameTime.TotalSeconds);

            //Console.WriteLine("speed: " +speed.abs);
            //Console.WriteLine("speed+: " + ((accelerationForce/MASS)*(float)gameTime.ElapsedGameTime.TotalSeconds));
            //Console.WriteLine("Newspeed: " + absNewSpeed);

            //rollFrictionForce = 0;
            //enrgie of car ( 1/2  *  m   *             v²           ) - (      F           *     v       *      t                                      )
            float energie = (1f / 2f * MASS * absNewSpeed * absNewSpeed) -(rollFrictionForce * Math.Abs(absNewSpeed) * (float)gameTime.ElapsedGameTime.TotalSeconds);
            //Console.WriteLine("energie: " + energie);

            if (energie < 0f)
                energie = 0f;

            //save direction of speed
            int richtung = 1;
                if (absNewSpeed < 0)
                    richtung = -1;
            // |v|      =                      WURZEL( 2 *  E     *   m  )
            absNewSpeed = richtung * (float)Math.Sqrt((2 * energie / MASS));

            //calculate new direction
            float totalStearingFactor = (float)(richtung * STEARING * Math.Log(Math.Abs(absNewSpeed)+1));
            Vector2 newDirection = new Vector2(speed.direction.X + sideMove.X * totalStearingFactor, speed.direction.Y + sideMove.Y * totalStearingFactor);
            newDirection.Normalize();

            //Console.WriteLine(absNewSpeed);

            return new Speed(newDirection, absNewSpeed);
        }

        /**
         * returns the roation in radiant by the direction of the car
         **/
        public static float calculateRotation(Vector2 direction)
        {
            return (float) Math.Atan2(direction.X,-direction.Y);
        }

        /**
         * returns if the car passed a checkpoint
         * 
         * pointA & pointB - Vector2 :
         * left and right of the checkpoint, the line will be between both
         * 
         * mid - Vector2 :
         * middelpoint of the player
         */
        public static bool checkPoint(Vector2 pointA, Vector2 pointB, Vector2 mid)
        {
            float diff = 1.5f;

            Vector2 ab = new Vector2(Math.Abs(pointA.X - pointB.X), Math.Abs(pointA.Y - pointB.Y));
            Vector2 am = new Vector2(Math.Abs(pointA.X - mid.X), Math.Abs(pointA.Y - mid.Y));
            Vector2 bm = new Vector2(Math.Abs(mid.X - pointB.X), Math.Abs(mid.Y - pointB.Y));

            if (ab.LengthSquared() * diff < am.LengthSquared() + bm.LengthSquared())
                return false;
            return true;
        }

        /**
         * returns a ROUNDED integer from a double (or float)
         * 
         * value - double :
         * cast this value to integer by rounding
         */
        public static int roundCast(double value)
        {
            return (value > 0) ? ((int)(value * 10 + 5) / 10) : ((int)(value * 10 - 5) / 10);
        }


        /**
         * returns if there is an collosion
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
        public static bool hasCollision(Vector2 position, bool[,] playerCollisionData, float rotation, EMapStates[,] mapData)
        {
            // save cos & sin of rotation, to save redundance calculations
            double rotationCos = Math.Cos(rotation);
            double rotationSin = Math.Sin(rotation);

            // width & height of the player's car
            int playerWidth = playerCollisionData.GetUpperBound(0);
            int playerHeight = playerCollisionData.GetUpperBound(1);

            // middle of the car
            Vector2 middlePoint = new Vector2(playerWidth / 2f, playerHeight / 2f);

            //Vector of the current position
            Vector2 tmpVect = Vector2.Zero;
            Vector2 mapPosition = Vector2.Zero;
            for (int x = 0; x < playerWidth; x++)
            {
                for (int y = 0; y < playerHeight; y++)
                {
                    if (playerCollisionData[x, y])
                    {
                        // calculate the pos on the map
                        tmpVect.X = x-middlePoint.X;
                        tmpVect.Y = y-middlePoint.Y;
                        mapPosition = Helper.rotateVector2(tmpVect, rotationCos, rotationSin) + position;
                        if (mapPosition.X >= 0 && mapPosition.Y < mapData.GetUpperBound(0) && mapPosition.Y >= 0 && mapPosition.Y < mapData.GetUpperBound(1))
                        if (mapData[(int)mapPosition.X, (int)mapPosition.Y] == EMapStates.Object)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}
