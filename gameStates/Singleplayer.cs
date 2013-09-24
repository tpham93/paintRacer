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
        Viewport defaultView;
        Viewport[] viewports;

        Scene scene;

        float rotation = 0.0f;

        GraphicsDevice graphicsDevice;

        public Singleplayer(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
        }


        // constructor for the game, if finished the LoadWindow
        public Singleplayer(GraphicsDevice graphicsDevice, Player[] players, Map map)
        {
            this.players = players;

            level = map;

            this.graphicsDevice = graphicsDevice;
        }

        public void Load(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            players[0].setPosition(level.Start);
            players[0].setSpeed(new Speed());
            players[0].setRotation(level.StartRotation);
            scene = new Scene(level, players, graphicsDevice.Viewport, Config.getKeys(), content);
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
