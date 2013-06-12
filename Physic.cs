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
        private const float MASS = 800; //mass of the car in kg
        private const float MAX_FORCE_ACCELERAT = 50000f; //max force of the car in N = kg*m/s²
        private const float MAX_FORCE_BARK = -2f * MAX_FORCE_ACCELERAT; //max force by barking in N = kg*m/s²
        private const float WHEEL_RADIUS = 0.25f; //radius of wheels in m
        private const float STEARING = 0.01f; //if you stear left or right
        private const float MAX_STEARING =0.001f; //the function is doing funny things for spacial values ;)

        private const float ROLL_FRICTION_STREET = 200f; //roll-friction stops the car in m
        private const float ROLL_FRICTION_GRASS = 600f; //roll-friction stops the car in m
        private const float STATIC_FRICTION_STREET = 0.9f; //helps the car to stay on road in 1
        private const float STATIC_FRICTION_GRASS = 0.6f; //helps the car to stay on road in 1
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

        /**
         * returns the updated speed of the car
         * 
         * speed - Vector2 :
         * speed of the car
         * 
         * driverInput - Vector2 :
         * direction witch is given by the driver
         * x: left(&st;0) straight on(0) right(&gt;0)
         * y: accelerate(&gt;0) roll(0) bark(&st;0)
         * 
         * returns :
         * new Vector2
         */
        public static Speed calculateSpeed(GameTime gameTime, Speed speed, Vector2 driverInput)
        {
            //Console.WriteLine("driverInput: [" + driverInput.X + ";" + driverInput.Y + "]"); 
            
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
            float rollFrictionForce = (ROLL_FRICTION_STREET * G / WHEEL_RADIUS);

            //konvergenzfactor for speed
            float konvergenzfactor = 2.5f;

            //set acceleration force
            float accelerationForce = 0f;
            if ((driverInput.Y > 0) && (speed.abs >= 0))
                accelerationForce = MAX_FORCE_ACCELERAT / (float)(Math.Pow(speed.abs/100, konvergenzfactor) + 1);
            else if ((driverInput.Y > 0) && (speed.abs < 0))
                accelerationForce = -MAX_FORCE_BARK;
            else if ((driverInput.Y < 0) && (speed.abs > 0))
                accelerationForce = MAX_FORCE_BARK;
            else if ((driverInput.Y < 0) && (speed.abs <= 0))
                accelerationForce = -MAX_FORCE_ACCELERAT / (float)(Math.Pow(speed.abs / 100, konvergenzfactor) + 1);

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
            float totalStearingFactor = (float)(STEARING * (-0.0000000005 * Math.Pow(absNewSpeed / richtung, 5) + 0.0000002602 * Math.Pow(absNewSpeed, 4) - 0.0000468578 * Math.Pow(absNewSpeed / richtung, 3) + 0.0031319378 * Math.Pow(absNewSpeed, 2) - 0.0529897925 * absNewSpeed / richtung));
            totalStearingFactor = Math.Min(totalStearingFactor, MAX_STEARING);
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
            //return (float) (Math.Atan(direction.Y/direction.X) + Math.PI/2);
            return (float) Math.Atan2(direction.X,-direction.Y);
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
        public static bool hasCollision(Vector2 position, bool[,] playerCollisionData, float rotation, sbyte[,] mapData)
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
                        if (mapPosition.X >= 0 && mapPosition.Y <= mapData.GetUpperBound(0) && mapPosition.Y >= 0 && mapPosition.Y <= mapData.GetUpperBound(1))
                        if (mapData[(int)mapPosition.X, (int)mapPosition.Y] == -2)
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
