using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace paintRacer
{
    class Scene
    {
        private enum ERaceState
        {
            LightOff,
            Ready_I,
            Ready_II,
            Ready_III,
            Ready_IV,
            Ready_V,
            Go,
            Race,
            EndOn,
            EndOff
        }

        private ERaceState raceState;
        private int startTime, raceTime;

        private Map level;
        private Player[] players;
        private Viewport[] viewports;
        private Keys[,] keys;
        private int playerCount;

        private Texture2D pixel;
        private SpriteFont Font;

        private Texture2D lightOff;
        private Texture2D Red_I;
        private Texture2D Red_II;
        private Texture2D Red_III;
        private Texture2D Red_IV;
        private Texture2D Red_V;
        private Texture2D Green;
        private Texture2D Yellow;

        public Scene(Map level, Player[] players, Viewport defaultView, Keys[,] keys, Microsoft.Xna.Framework.Content.ContentManager content)
        {
            startTime = 0;
            raceTime = 0;
            raceState = ERaceState.LightOff;

            lightOff = Helper.loadImage("Ampel_aus.png");
            Red_I = Helper.loadImage("Ampel_RI.png");
            Red_II = Helper.loadImage("Ampel_RII.png");
            Red_III = Helper.loadImage("Ampel_RIII.png");
            Red_IV = Helper.loadImage("Ampel_RIV.png");
            Red_V = Helper.loadImage("Ampel_RV.png");
            Green = Helper.loadImage("Ampel_Go.png");
            Yellow = Helper.loadImage("Ampel_End.png");

            pixel = Helper.loadImage("OneWithePixel.png");
            Font = content.Load<SpriteFont>(@"font");
            
            this.level = level;

            playerCount = Math.Min(players.Length, keys.GetLength(0));

            if (playerCount == 0)
            {
                throw new Exception("Couldn't create Scene, playerCount is 0.");
            }
            else
            {
                this.players = new Player[playerCount];
                this.viewports = new Viewport[playerCount];
                viewports = Helper.createViewports(playerCount, defaultView);
                this.keys = new Keys[playerCount, keys.GetLength(1)];
                for (int i = 0; i < playerCount; i++)
                {
                    this.players[i] = players[i];
                    this.players[i].setScene(this);
                    this.viewports[i] = viewports[i];
                    for (int j = 0; j < keys.GetLength(1); j++)
                    {
                        this.keys[i, j] = keys[i, j];
                    }
                }
            }
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            int lightTackt = 1000;

            //before start
            if (raceState == ERaceState.LightOff || raceState == ERaceState.Ready_I || raceState == ERaceState.Ready_II || raceState == ERaceState.Ready_III || raceState == ERaceState.Ready_IV || raceState == ERaceState.Ready_V || raceState == ERaceState.Go)
            {
                startTime += gameTime.ElapsedGameTime.Milliseconds;
                if (startTime < 1 * lightTackt)
                    raceState = ERaceState.LightOff;
                else if (startTime < 2 * lightTackt)
                    raceState = ERaceState.Ready_I;
                else if (startTime < 3 * lightTackt)
                    raceState = ERaceState.Ready_II;
                else if (startTime < 4 * lightTackt)
                    raceState = ERaceState.Ready_III;
                else if (startTime < 5 * lightTackt)
                    raceState = ERaceState.Ready_IV;
                else if (startTime < 6 * lightTackt)
                    raceState = ERaceState.Ready_V;
                else if (startTime < 7 * lightTackt)
                    raceState = ERaceState.Go;
                else if (startTime < 8 * lightTackt)
                    raceState = ERaceState.Race;
            }

            //in race
            if (raceState == ERaceState.Go || raceState == ERaceState.Race || raceState == ERaceState.EndOff || raceState == ERaceState.EndOn)
            {
                raceTime += gameTime.ElapsedGameTime.Milliseconds;
                if (((players[0].lap > Multiplayer.LAPS) || (players.Length > 1 && players[1].lap > Multiplayer.LAPS)) && (raceTime / lightTackt) % 2 == 0)
                    raceState = ERaceState.EndOff;
                if (((players[0].lap > Multiplayer.LAPS) || (players.Length > 1 && players[1].lap > Multiplayer.LAPS)) && (raceTime / lightTackt) % 2 > 0)
                    raceState = ERaceState.EndOn;
            }


            for (int i = 0; i < playerCount; i++)
            {
                //Splits up keys Array into separate Arrays for each player which contain whether a key was pressed or not (this is done to simplify the Update of each player)
                bool[] pressedKeys = new bool[keys.GetLength(1)];
                for (int j = 0; j < keys.GetLength(1); j++)
                {
                    pressedKeys[j] = keyboardState.IsKeyDown(keys[i, j]);
                    //Console.WriteLine("pressedKeys " + j + " " + pressedKeys[j]); cecked
                }
                if (raceState == ERaceState.Go || raceState == ERaceState.Race)
                    players[i].Update(gameTime, pressedKeys);
                //Physic.hasCollision(players[i].getPosition(), players[i].getCollisionData(), players[i].getRotation(), level.getMapData());
            }
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice)
        {
            for (int i = 0; i < players.Length; i++)
            {
                level.Draw(spriteBatch, GraphicsDevice, viewports[i], players[i]);
                // draw cars
                for (int j = 0; j < players.Length; j++)
                {
                    if (i != j)
                    {
                        // draw the other car on the viewport
                        players[j].Draw(spriteBatch, spriteBatch.GraphicsDevice, viewports[i], players[i]);
                    }
                }
                // draw the protaginist on top
                players[i].Draw(spriteBatch, spriteBatch.GraphicsDevice, viewports[i], players[i]);
                //Physic.hasCollision(players[i].getPosition(), players[i].getCollisionData(), players[i].getRotation(), level.getMapData());
                
                //draw infos
                spriteBatch.Begin();

                //laps and time
                spriteBatch.Draw(pixel, new Rectangle(0,0,800,50), Color.LightGray);
                spriteBatch.DrawString(Font, "Lap: " + players[0].lap + " / " + Multiplayer.LAPS, new Vector2(10, 0), Color.Black);
                if (players.Length > 1)
                    spriteBatch.DrawString(Font, "Lap: " + players[1].lap + " / " + Multiplayer.LAPS, new Vector2(410, 0), Color.Black);

                //light
                Vector2 pos = new Vector2 ((800-370)/2, 100);
                switch (raceState)
                {
                    case ERaceState.LightOff :
                        spriteBatch.Draw(lightOff, pos, Color.White);
                        break;
                    case ERaceState.Ready_I :
                        spriteBatch.Draw(Red_I, pos, Color.White);
                        break;
                    case ERaceState.Ready_II :
                        spriteBatch.Draw(Red_II, pos, Color.White);
                        break;
                    case ERaceState.Ready_III :
                        spriteBatch.Draw(Red_III, pos, Color.White);
                        break;
                    case ERaceState.Ready_IV :
                        spriteBatch.Draw(Red_IV, pos, Color.White);
                        break;
                    case ERaceState.Ready_V :
                        spriteBatch.Draw(Red_V, pos, Color.White);
                        break;
                    case ERaceState.Go :
                        spriteBatch.Draw(Green, pos, Color.White);
                        break;
                    case ERaceState.EndOff :
                        spriteBatch.Draw(lightOff, pos, Color.White);
                        break;
                    case ERaceState.EndOn :
                        spriteBatch.Draw(Yellow, pos, Color.White);
                        break;
                    default :
                        break;
                }

                spriteBatch.End();
            }
        }

        public Map getLevel()
        {
            return level;
        }
    }
}
