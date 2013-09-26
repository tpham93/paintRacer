using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace paintRacer
{
    class Multiplayer : IGameStateElements
    {
        //Member of Scene.cs
        Map level;
        private Player[] players;

        Scene scene;

        //float rotation = 0.0f;

        GraphicsDevice graphicsDevice;

        bool finished;
        TimeSpan finishWaitTime;

        // constructor for the game, if finished the LoadWindow
        public Multiplayer(GraphicsDevice graphicsDevice, Player[] players, Map map)
        {
            this.players = players;

            level = map;

            finished = false;

            this.graphicsDevice = graphicsDevice;
        }

        public void Load(ContentManager content)
        {
            const int distance = 40;
            Vector2 offset = new Vector2((float)(distance * Math.Cos(level.StartRotation)), -(float)(distance * Math.Sin(level.StartRotation)));

            foreach (Player p in players)
            {
                p.reset();
                p.setSpeed(new Speed());
                p.setRotation(level.StartRotation);
            }

            players[0].setPosition(level.Start - offset);
            players[1].setPosition(level.Start + offset);
            scene = new Scene(level, players, graphicsDevice.Viewport, Config.getKeys(), content);

            finished = false;
        }

        public EGameStates Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            Keys[,] keys = Config.getKeys();
            if (keyboardState.IsKeyDown(keys[0,(int)Config.controlKeys.Pause]) || keyboardState.IsKeyDown(keys[1,(int)Config.controlKeys.Pause]))
            {
                return EGameStates.Pause;
            }
            scene.Update(gameTime, Keyboard.GetState());

            if (!finished && scene.isFinished())
            {
                finished = true;
                Global.evaluationData = new EvaluationData(scene.getLevel().Highscore,scene.getFinishedPlayer().getName(),scene.raceTime);
                finishWaitTime = new TimeSpan(0, 0, 3);
            }
            else if (finished && finishWaitTime > new TimeSpan())
            {
                finishWaitTime -= gameTime.ElapsedGameTime;
            }
            else if (finished && finishWaitTime < new TimeSpan())
            {
                return EGameStates.Evaluation;
            }

            return EGameStates.MultiPlayer;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            scene.Draw(spriteBatch, graphicsDevice);
        }

        public void Unload()
        {

        }


        public EGameStates getGameState()
        {
            return EGameStates.MultiPlayer;
        }
    }
}
