﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace paintRacer
{
    class ActualMultiplayer:IGameStateElements
    {

        //Must be in Loadfunction
        String MAP_PIC = "test2.png";
        Vector2 START_POS = new Vector2(1535, 550);

        //Member of Scene.cs
        Level level;
        private Player[] players;
        Viewport defaultView;
        Viewport[] viewports;

        Scene scene;

        float rotation = 0.0f;

        GraphicsDevice graphicsDevice;

        public ActualMultiplayer(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
        }

        public void Load(ContentManager content)
        {
            //Initializes Level and Player with test textures
            level = new Level(MAP_PIC, Level.MapType.rawImage);
            players = new Player[2];
            players[0] = new Player(Helper.loadImage("testcar.png"), Color.Blue);
            players[1] = new Player(Helper.loadImage("testcar.png"), Color.Red);
            players[0].setPosition(new Vector2(START_POS.X - 40, START_POS.Y));
            players[1].setPosition(new Vector2(START_POS.X + 40, START_POS.Y));

            scene = new Scene(level, players, graphicsDevice.Viewport, Config.getKeys());
        }

        public EGameStates Update(GameTime gameTime)
        {
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
    }
}
