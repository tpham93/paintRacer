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

        }

        
        // constructor for the game, if finished the LoadWindow
        public Singleplayer(GraphicsDevice graphicsDevice, Player[] players, Map map)
        {
            this.players = players;

            players[0].setPosition(map.Start);

            players[0].setRotation(map.StartRotation);

            level = map;

            this.graphicsDevice = graphicsDevice;
        }

        public void Load(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            scene = new Scene(level, players, graphicsDevice.Viewport, Config.getKeys());
        }

        public EGameStates Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
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
    }
}
