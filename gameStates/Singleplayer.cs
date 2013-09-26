using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace paintRacer
{
    class Singleplayer : IGameStateElements
    {
        //Member of Scene.cs
        Map level;
        private Player[] players;

        Scene scene;

        GraphicsDevice graphicsDevice;

        bool finished;
        TimeSpan finishWaitTime;


        // constructor for the game, if finished the LoadWindow
        public Singleplayer(GraphicsDevice graphicsDevice, Player[] players, Map map)
        {
            this.players = players;

            level = map;

            this.graphicsDevice = graphicsDevice;
        }

        public void Load(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            foreach (Player p in players)
            {
                p.reset();
                p.setSpeed(new Speed());
                p.setPosition(level.Start);
                p.setRotation(level.StartRotation);
            }
            scene = new Scene(level, players, graphicsDevice.Viewport, Config.getKeys(), content);
            finished = false;
        }

        public EGameStates Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            Keys[,] keys = Config.getKeys();
            if (keyboardState.IsKeyDown(keys[0, (int)Config.controlKeys.Pause]) || keyboardState.IsKeyDown(keys[1, (int)Config.controlKeys.Pause]))
            {
                return EGameStates.Pause;
            }

            scene.Update(gameTime, Keyboard.GetState());

            if (!finished && scene.isFinished())
            {
                finished = true;
                Global.evaluationData = new EvaluationData(scene.getLevel().Highscore, scene.getFinishedPlayer(), scene.raceTime);
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

            return EGameStates.SinglePlayer;
        }

        public void Draw(Microsoft.Xna.Framework.GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            scene.Draw(spriteBatch, graphicsDevice);
        }

        public void Unload()
        {

        }


        public EGameStates getGameState()
        {
            return EGameStates.SinglePlayer;
        }
    }
}
