﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;


namespace paintRacer
{
    class Physic
    {
        //const VAR
        private const int MASS = 800; //mass of the car in kg
        private const int MAX_FORCE_ACCELERAT = 1000; //max force of the car in N = kg*m/s²
        private const int MAX_FORCE_BARK = -2000; //max force by barking in N = kg*m/s²
        private const float WHEEL_RADIUS = 0.25f; //radius of wheels in m
        private const float STEARING = 1f; //if you stear left or right

        private const float ROLL_FRICTION_STREET = 0.00002f; //roll-friction stops the car in m
        private const float ROLL_FRICTION_GRASS = 0.00006f; //roll-friction stops the car in m
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
        public static Vector2 calculateNextPos(Vector2 pos, Vector2 speed)
        {
            return new Vector2(pos.X + speed.X, pos.Y + speed.Y);
        }

        /**
         * returns the updated speed of the car
         * 
         * speed - Vector2 :
         * speed of the car
         * 
         * driverInput - Vector2 :
         * direction witch is givven by the driver
         * x: left(&st;0) strate on(0) right(&gt;0)
         * y: accelorate(&gt;0) roll(0) bark(&st;0)
         * 
         * returns :
         * new Vector2
         */
        public static Vector2 calculateSpeed(Vector2 speed, Vector2 driverInput)
        {
            Vector2 accelaration = new Vector2(speed.X, speed.Y);
            accelaration.Normalize();
            
            Vector2 sideMove;
            if (driverInput.X < 0)
            {
                sideMove = new Vector2(-accelaration.Y, accelaration.X);
                sideMove.Normalize();
            }
            else if (driverInput.X > 0)
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

            //set acceleration force
            int accelerationForce = 0;
            if (driverInput.Y > 0)
                accelerationForce = MAX_FORCE_ACCELERAT;
            else if (driverInput.Y < 0)
                accelerationForce = MAX_FORCE_BARK;

            //enrgie of car (1/2 *  m   *         v²                   ) + ((     F           -  Fr             )* v/t    {t=1} )
            float energie = (1/2 * MASS * speed.Length()*speed.Length()) + ((accelerationForce-rollFrictionForce)*speed.Length());
            if (energie < 0)
                energie = 0;
            //       |v|   =      WURZEL(      2  *  E     *   m  )
            float absSpeed = (float)Math.Sqrt((2 * energie * MASS));

            //calculate new speed
            Vector2 newSpeed = new Vector2(speed.X + sideMove.X*STEARING, speed.Y + sideMove.Y*STEARING); 
            newSpeed.Normalize();
            newSpeed = new Vector2(newSpeed.X*absSpeed, newSpeed.Y*absSpeed);

            return newSpeed;
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
        private static bool hasCollision(Vector2 position ,bool[,] playerCollisionData, float rotation, short[,] mapData)
        {
            // save cos & sin of rotation, to save redundance calculations
            double rotationCos = Math.Cos(-rotation);
            double rotationSin = Math.Cos(-rotation);

            // width & height of the player's car
            int playerWidth = playerCollisionData.GetUpperBound(0);
            int playerHeight = playerCollisionData.GetUpperBound(1);

            // middle of the car
            Vector2 middlePoint = new Vector2(playerWidth / 2f, playerHeight / 2f);

            //Vector of the current position
            Vector2 tmpVect = Vector2.Zero;

            for (int x = 0; x < playerWidth; x++)
            {
                for (int y = 0; y < playerHeight; y++)
                {
                    if (playerCollisionData[x, y])
                    {
                        // calculate the pos on the map
                        Vector2 mapPosition = Helper.rotateVector2(tmpVect, rotationCos, rotationSin);
                        if (mapData[(int)mapPosition.X, (int)mapPosition.Y] == -1)
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
