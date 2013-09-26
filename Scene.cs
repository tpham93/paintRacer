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
        public TimeSpan startTime, raceTime, lightTime;

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
            startTime = new TimeSpan();
            raceTime = new TimeSpan();
            lightTime = new TimeSpan();
            raceState = ERaceState.LightOff;

            lightOff = Helper.loadImage(@"Content\light\Ampel_aus.png");
            Red_I = Helper.loadImage(@"Content\light\Ampel_RI.png");
            Red_II = Helper.loadImage(@"Content\light\Ampel_RII.png");
            Red_III = Helper.loadImage(@"Content\light\Ampel_RIII.png");
            Red_IV = Helper.loadImage(@"Content\light\Ampel_RIV.png");
            Red_V = Helper.loadImage(@"Content\light\Ampel_RV.png");
            Green = Helper.loadImage(@"Content\light\Ampel_Go.png");
            Yellow = Helper.loadImage(@"Content\light\Ampel_End.png");

            pixel = Helper.loadImage(@"Content\OneWithePixel.png");
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
            int lightTakt = 700;
            Vector2[] driverInputs = new Vector2[playerCount];

            //before start
            if (raceState == ERaceState.LightOff || raceState == ERaceState.Ready_I || raceState == ERaceState.Ready_II || raceState == ERaceState.Ready_III || raceState == ERaceState.Ready_IV || raceState == ERaceState.Ready_V || raceState == ERaceState.Go)
            {
                startTime += gameTime.ElapsedGameTime;

                if (startTime > new TimeSpan(0, 0, 0, 0, ((int)raceState + 1) * lightTakt))
                {
                    ++raceState;
                }
            }

            //in race
            if (raceState == ERaceState.EndOff || raceState == ERaceState.EndOn || ((players[0].lap > Config.MAXLAPCOUNT) || (players.Length > 1 && players[1].lap > Config.MAXLAPCOUNT)))
            {
                lightTime += gameTime.ElapsedGameTime;
                if (((int)lightTime.TotalMilliseconds / lightTakt) % 2 == 0)
                    raceState = ERaceState.EndOff;
                if (((int)lightTime.TotalMilliseconds / lightTakt) % 2 > 0)
                    raceState = ERaceState.EndOn;
            }
            if (raceState == ERaceState.Go || raceState == ERaceState.Race)
            {
                raceTime += gameTime.ElapsedGameTime;
            }

            //collision between cars
            if (Physic.hasCollision(gameTime, players, driverInputs, level.Data))
                Physic.CarKonflikt(players);

            for (int i = 0; i < playerCount; ++i)
            {
                //Splits up keys Array into separate Arrays for each player which contain whether a key was pressed or not (this is done to simplify the Update of each player)
                bool[] pressedKeys = new bool[keys.GetLength(1)];
                for (int j = 0; j < keys.GetLength(1); j++)
                {
                    pressedKeys[j] = keyboardState.IsKeyDown(keys[i, j]);
                }
                driverInputs[i] = Player.getDriverInput(pressedKeys);
            }

            for (int i = 0; i < playerCount; i++)
            {
                if (raceState == ERaceState.Go || raceState == ERaceState.Race)
                    players[i].Update(gameTime, driverInputs[i]);
            }
        }

        public bool isFinished()
        {
            for (int i = 0; i < players.Length; ++i)
            {
                if (players[i].lap > Config.MAXLAPCOUNT)
                {
                    return true;
                }
            }
            return false;
        }

        public Player getFinishedPlayer()
        {
            for (int i = 0; i < players.Length; ++i)
            {
                if (players[i].lap > Config.MAXLAPCOUNT)
                {
                    return players[i];
                }
            }
            return null;
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
                spriteBatch.Draw(pixel, new Rectangle(0, 0, 800, 50), Color.LightGray);
                int RTmin = raceTime.Minutes;
                int RTsek = raceTime.Seconds;
                int RTmsek = raceTime.Milliseconds / 10;
                int minTime = 0;
                for (int j = 0; j < players[0].lap - 1; ++j)
                    minTime = Math.Min((minTime == 0 ? 1000000 : minTime), (int)players[0].times[j].TotalMilliseconds);
                int LTsek = minTime / 1000;
                int LTmsek = (minTime % 1000) / 10;
                spriteBatch.DrawString(Font, "Lap: " + Math.Min(players[0].lap, Config.MAXLAPCOUNT) + " / " + Config.MAXLAPCOUNT + "   " + RTmin + ":" + RTsek + "," + RTmsek + "  \nfastest Lap: " + LTsek + "," + LTmsek + "sek", new Vector2(5, 0), Color.Black);
                if (players.Length > 1)
                {
                    minTime = 0;
                    for (int j = 0; j < players[1].lap - 1; ++j)
                        minTime = Math.Min((minTime == 0 ? 1000000 : minTime), (int)players[1].times[j].TotalMilliseconds);
                    LTsek = minTime / 1000;
                    LTmsek = (minTime % 1000) / 10;
                    spriteBatch.DrawString(Font, "Lap: " + Math.Min(players[1].lap, Config.MAXLAPCOUNT) + " / " + Config.MAXLAPCOUNT + "   " + RTmin + ":" + RTsek + "," + RTmsek + "  \nfastest Lap: " + LTsek + "," + LTmsek + "sek", new Vector2(405, 0), Color.Black);
                }

                //light
                Vector2 pos = new Vector2((800 - 250) / 2, 50);
                switch (raceState)
                {
                    case ERaceState.LightOff:
                        spriteBatch.Draw(lightOff, pos, Color.White);
                        break;
                    case ERaceState.Ready_I:
                        spriteBatch.Draw(Red_I, pos, Color.White);
                        break;
                    case ERaceState.Ready_II:
                        spriteBatch.Draw(Red_II, pos, Color.White);
                        break;
                    case ERaceState.Ready_III:
                        spriteBatch.Draw(Red_III, pos, Color.White);
                        break;
                    case ERaceState.Ready_IV:
                        spriteBatch.Draw(Red_IV, pos, Color.White);
                        break;
                    case ERaceState.Ready_V:
                        spriteBatch.Draw(Red_V, pos, Color.White);
                        break;
                    case ERaceState.Go:
                        spriteBatch.Draw(Green, pos, Color.White);
                        break;
                    case ERaceState.EndOff:
                        spriteBatch.Draw(lightOff, pos, Color.White);
                        break;
                    case ERaceState.EndOn:
                        spriteBatch.Draw(Yellow, pos, Color.White);
                        break;
                    default:
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
