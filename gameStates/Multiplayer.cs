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

        //public Multiplayer(GraphicsDevice graphicsDevice)
        //{
        //    this.graphicsDevice = graphicsDevice;

        //    //Initializes Level and Player with test textures
        //    //level = new Map(MAP_PIC_SW, MAP_PIC);
        //    level = Global.map;
        //    players = new Player[2];
        //    players[0] = new Player(Helper.loadImage("testcar.png"), Color.Blue, (level.CheckPoints.Length)/2);
        //    players[1] = new Player(Helper.loadImage("testcar.png"), Color.Red, (level.CheckPoints.Length)/2);
        //    players[0].setPosition(new Vector2(level.Start.X - 40, level.Start.Y));
        //    players[1].setPosition(new Vector2(level.Start.X + 40, level.Start.Y));
        //}
        // constructor for the game, if finished the LoadWindow
        public Multiplayer(GraphicsDevice graphicsDevice, Player[] players, Map map)
        {
            this.players = players;

            level = map;

            this.graphicsDevice = graphicsDevice;
        }

        public void Load(ContentManager content)
        {
            const int distance = 40;
            Vector2 offset = new Vector2((float)(distance * Math.Cos(level.StartRotation)), -(float)(distance * Math.Sin(level.StartRotation)));
            players[0].setPosition(level.Start - offset);
            players[1].setPosition(level.Start + offset);

            players[0].setSpeed(new Speed());
            players[1].setSpeed(new Speed());

            players[0].setRotation(level.StartRotation);
            players[1].setRotation(level.StartRotation);
            scene = new Scene(level, players, graphicsDevice.Viewport, Config.getKeys(), content);
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
