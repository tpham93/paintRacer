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
        private const float STEARING = 0.0003f; //if you stear left or right

        private const float ROLL_FRICTION = 400f; //roll-friction stops the car in m
        private const float OFFROAD_MAX_ENERGIE = 5000000f;
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
            int playerPosX = (int)player.getPosition().X;
            int playerPosY = (int)player.getPosition().Y;

            Speed speed = player.getSpeed();

            if (playerPosX < 0 || playerPosY < 0 || playerPosX > mapdata.GetUpperBound(0) || playerPosY > mapdata.GetUpperBound(1))
                return new Speed(speed.direction, 0f);
            EMapStates mapState = mapdata[playerPosX, playerPosY];

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
            float rollFrictionForce = (ROLL_FRICTION * G / WHEEL_RADIUS);

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

            //rollFrictionForce = 0;
            //enrgie of car ( 1/2  *  m   *             v²           ) - (      F           *     v       *      t                                      )
            float energie = (1f / 2f * MASS * absNewSpeed * absNewSpeed) - (rollFrictionForce * Math.Abs(absNewSpeed) * (float)gameTime.ElapsedGameTime.TotalSeconds);

            if (energie < 0f)
                energie = 0f;
            if (mapState == EMapStates.Offroad && energie > OFFROAD_MAX_ENERGIE)
                energie = OFFROAD_MAX_ENERGIE;

            //save direction of speed
            int richtung = 1;
            if (absNewSpeed < 0)
                richtung = -1;
            // |v|      =                      WURZEL( 2 *  E     *   m  )
            absNewSpeed = richtung * (float)Math.Sqrt((2 * energie / MASS));

            //calculate new direction
            float totalStearingFactor = (float)(richtung * gameTime.ElapsedGameTime.Milliseconds * STEARING * Math.Log(Math.Abs(absNewSpeed) + 1));
            Vector2 newDirection = new Vector2(speed.direction.X + sideMove.X * totalStearingFactor, speed.direction.Y + sideMove.Y * totalStearingFactor);
            newDirection.Normalize();

            Speed res = new Speed(newDirection, absNewSpeed);

            if (hasCollision(calculateNextPos(gameTime, player.getPosition(), res), player.getCollisionData(), calculateRotation(res.direction), mapdata))
            {
                return new Speed(newDirection, -0.5f * absNewSpeed);
            }

            return res;
        }

        /**
         * returns the roation in radiant by the direction of the car
         **/
        public static float calculateRotation(Vector2 direction)
        {
            return (float)Math.Atan2(direction.X, -direction.Y);
        }

        ///**
        // * returns if the car passed a checkpoint
        // * 
        // * pointA & pointB - Vector2 :
        // * left and right of the checkpoint, the line will be between both
        // * 
        // * mid - Vector2 :
        // * middelpoint of the player
        // */
        //public static bool checkPoint(Vector2 pointA, Vector2 pointB, Vector2 mid)
        //{
        //    const float diff = 1.000005f;

        //    Vector2 ab = new Vector2(Math.Abs(pointA.X - pointB.X), Math.Abs(pointA.Y - pointB.Y));
        //    Vector2 am = new Vector2(Math.Abs(pointA.X - mid.X), Math.Abs(pointA.Y - mid.Y));
        //    Vector2 bm = new Vector2(Math.Abs(mid.X - pointB.X), Math.Abs(mid.Y - pointB.Y));

        //    return (ab.LengthSquared() * diff >= am.LengthSquared() + bm.LengthSquared());
        //}

        public static Vector2 getCutPoint(Line l1, Line l2)
        {
            bool l1Const = l1.isConstant();
            bool l2Const = l2.isConstant();
            float x = 0f;
            if (l1Const)
            {
                x = l1.x;
            }
            else if (l2Const)
            {
                return getCutPoint(l2, l1);
            }
            else
            {
                x = (l2.c - l1.c) / (l1.m - l2.m);
            }
            float y = l2.calculate(x);
            return new Vector2(x, y);
        }

        public static bool vectorCut(Vector2 c1, Vector2 c2, Vector2 p1, Vector2 p2)
        {
            Line l1 = new Line(c1, c2);
            Line l2 = new Line(p1, p2);

            if (l1.m == l2.m)
            {
                return false;
            }

            if (l1.isConstant() && l2.isConstant())
            {
                return l1.x == l2.x;
            }
            Vector2 cutPoint = getCutPoint(l1, l2);
            return (cutPoint - p1).LengthSquared() <= (p2 - p1).LengthSquared();
        }


        ///**
        // * returns a ROUNDED integer from a double (or float)
        // * 
        // * value - double :
        // * cast this value to integer by rounding
        // */
        //public static int roundCast(double value)
        //{
        //    return (value > 0) ? ((int)(value * 10 + 5) / 10) : ((int)(value * 10 - 5) / 10);
        //}


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
                        tmpVect.X = x - middlePoint.X;
                        tmpVect.Y = y - middlePoint.Y;
                        mapPosition = Helper.rotateVector2(tmpVect, rotationCos, rotationSin) + position;
                        if (mapPosition.X >= 0 && mapPosition.X < mapData.GetUpperBound(0) && mapPosition.Y >= 0 && mapPosition.Y < mapData.GetUpperBound(1))
                        {
                            if (mapData[(int)mapPosition.X, (int)mapPosition.Y] == EMapStates.Object)
                            {
                                return true;
                            }
                        }
                        else return true;
                    }
                }
            }
            return false;
        }

        public static bool hasCollision(GameTime gameTime, Player[] players, Vector2[] driverInputs, EMapStates[,] mapdata)
        {
            Speed[] playerSpeeds = new Speed[2];
            Vector2[] newPositions = new Vector2[2];
            float[] newRotation = new float[2];

            for (int i = 0; i < players.Length; ++i)
            {
                playerSpeeds[i] = calculateSpeed(gameTime, players[i], driverInputs[i], mapdata);
                newPositions[i] = calculateNextPos(gameTime, players[i].getPosition(), playerSpeeds[i]);
                newRotation[i] = calculateRotation(playerSpeeds[i].direction);
            }
            return hasCollision(players[0].getCollisionData(), newPositions[0], newPositions[1], newRotation[0], newRotation[1]);
        }

        public static bool hasCollision(Player[] players)
        {

            if (players.Length <= 1)
                return false;

            Vector2 player1Position = players[0].getPosition();
            Vector2 player2Position = players[1].getPosition();

            bool[,] playerCollisionData = players[0].getCollisionData();
            return hasCollision(playerCollisionData,player1Position, player2Position, players[0].getRotation(),players[1].getRotation());
        }

        public static bool hasCollision(bool[,] playerCollisionData, Vector2 player1Position, Vector2 player2Position, float rotationPlayer1, float rotationPlayer2)
        {

            // save cos & sin of rotation, to save redundance calculations
            double rotationPlayer1Cos = Math.Cos(rotationPlayer1);
            double rotationPlayer1Sin = Math.Sin(rotationPlayer1);

            double invertRotationPlayer2Cos = Math.Cos(-rotationPlayer2);
            double invertRotationPlayer2Sin = Math.Sin(-rotationPlayer2);

            // width & height of the player's car
            int playerWidth = playerCollisionData.GetUpperBound(0);
            int playerHeight = playerCollisionData.GetUpperBound(1);

            // middle of the car
            Vector2 middlePoint = new Vector2(playerWidth / 2f, playerHeight / 2f);

            //Vector of the current position
            Vector2 tmpVect = Vector2.Zero;
            Vector2 mapPlayer1Position = Vector2.Zero;
            Vector2 relativePlayer1Position = Vector2.Zero;
            Vector2 rotatedRelativePlayer1Position = Vector2.Zero;
            for (int x = 0; x < playerWidth; x++)
            {
                for (int y = 0; y < playerHeight; y++)
                {
                    if (playerCollisionData[x, y])
                    {
                        tmpVect.X = x - middlePoint.X;
                        tmpVect.Y = y - middlePoint.Y;
                        mapPlayer1Position = Helper.rotateVector2(tmpVect, rotationPlayer1Cos, rotationPlayer1Sin) + player1Position;

                        relativePlayer1Position = mapPlayer1Position - player2Position;
                        rotatedRelativePlayer1Position = Helper.rotateVector2(relativePlayer1Position, invertRotationPlayer2Cos, invertRotationPlayer2Sin) + middlePoint;
                        if (rotatedRelativePlayer1Position.X >= 0 && rotatedRelativePlayer1Position.X < playerWidth && rotatedRelativePlayer1Position.Y >= 0 && rotatedRelativePlayer1Position.Y < playerHeight)
                        {
                            if (playerCollisionData[(int)rotatedRelativePlayer1Position.X, (int)rotatedRelativePlayer1Position.Y])
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }


        public static void CarKonflikt(Player[] players)
        {
            //max diff between the speed vector of players[0] and the vector from player[0] mid to player[1] mid
            float maxRotationDiff = (float)(0.1 * Math.PI);
            Vector2 car0direction = players[0].getSpeed().direction;
            car0direction = (players[0].getSpeed().abs < 0) ? new Vector2(-car0direction.X, -car0direction.Y) : car0direction;
            Vector2 car1direction = players[1].getSpeed().direction;
            car1direction = (players[1].getSpeed().abs < 1) ? new Vector2(-car1direction.X, -car1direction.Y) : car1direction;
            Vector2 carMidds_01 = new Vector2(players[1].getPosition().X - players[0].getPosition().X, players[1].getPosition().Y - players[0].getPosition().Y);
            
            if (Math.Abs(calculateRotation(car1direction) - calculateRotation(car0direction)) < maxRotationDiff)
            {
                if (Math.Abs(calculateRotation(carMidds_01) - calculateRotation(car0direction)) > maxRotationDiff)
                {
                    //car 1 crashed in car 0
                    players[1].setSpeed(new Speed(players[1].getSpeed().direction, 0.8f * ((players[0].getSpeed().abs > players[1].getSpeed().abs) ? players[1].getSpeed().abs : players[0].getSpeed().abs)));
                }
                else
                {
                    //car 0 crashed in car 1
                    players[0].setSpeed(new Speed(players[0].getSpeed().direction, 0.8f * ((players[1].getSpeed().abs > players[0].getSpeed().abs) ? players[0].getSpeed().abs : players[1].getSpeed().abs)));
                }
            }

            if (Math.Abs(calculateRotation(carMidds_01) - calculateRotation(car0direction)) > maxRotationDiff)
            {
                //car 1 crashed in car 0
                players[1].setSpeed(new Speed(players[1].getSpeed().direction, -0.3f * players[1].getSpeed().abs));
                players[0].setSpeed(new Speed(players[0].getSpeed().direction, 0.8f * players[0].getSpeed().abs));
            }
            else
            {
                //car 0 crashed in car 1
                players[1].setSpeed(new Speed(players[1].getSpeed().direction, 0.8f * players[1].getSpeed().abs));
                players[0].setSpeed(new Speed(players[0].getSpeed().direction, -0.3f * players[0].getSpeed().abs));
            }
        }
    }
}
