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

        //Must be in Loadfunction
        String MAP_PIC = "test2.png";
        String MAP_PIC_SW = "test2SW.png";
        Vector2 start_pos = new Vector2(1535, 550);
        Vector2[] check_points = new Vector2[1];

        //Member of Scene.cs
        Map level;
        private Player[] players;
        Viewport defaultView;
        Viewport[] viewports;

        Scene scene;

        float rotation = 0.0f;

        GraphicsDevice graphicsDevice;

        public Multiplayer(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;

            //Initializes Level and Player with test textures
            level = new Map(MAP_PIC_SW, MAP_PIC);
            players = new Player[2];
            players[0] = new Player(Helper.loadImage("testcar.png"), Color.Blue);
            players[1] = new Player(Helper.loadImage("testcar.png"), Color.Red);
            players[0].setPosition(new Vector2(start_pos.X - 40, start_pos.Y));
            players[1].setPosition(new Vector2(start_pos.X + 40, start_pos.Y));
        }
        // constructor for the game, if finished the LoadWindow
        public Multiplayer(GraphicsDevice graphicsDevice, Player[] players, Map map)
        {
            this.graphicsDevice = graphicsDevice;
        }

        public void Load(ContentManager content)
        {
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
