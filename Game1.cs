using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace paintRacer
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Player and Viewports to be organized in a Collection (same as the keys in the Config)

        //Future member of Scene.cs
        Level level;
        private Player[] allPlayers;
        Viewport defaultView;
        Viewport[] viewPorts;

        float rotation = 0.0f;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //Removes FPS lock and VSync
            graphics.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = false;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            //set static graphicsdevice in Helper
            Helper.Init(GraphicsDevice);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Initializes Level and Player with test textures
            level = new Level("test.png",Level.MapType.rawImage);
            allPlayers = new Player[2];
            allPlayers[0] = new Player(Helper.loadImage("testcar.png"), Color.Blue);
            allPlayers[1] = new Player(Helper.loadImage("testcar.png"), Color.Red);
            allPlayers[1].setPosition(new Vector2(0.0f, 128.0f));

            //Initializes the Viewports
            defaultView = GraphicsDevice.Viewport;
            viewPorts = new Viewport[2];
            viewPorts[0] = defaultView;
            viewPorts[1] = defaultView;
            viewPorts[0].Width /= 2;
            viewPorts[1].Width /= 2;
            viewPorts[1].X = viewPorts[0].Width;


            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            //Writes FPS to title
            this.Window.Title = "" + (int)(1 / (gameTime.ElapsedGameTime.TotalSeconds));

            level.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //Drawing everything on the left and the right screen (rotation is currently displayed by the players not the level, we will need to calculate the other player's position/rotation differently to do so)
            level.Draw(spriteBatch, GraphicsDevice, viewPorts[0], allPlayers[0]);
            allPlayers[0].Draw(spriteBatch, GraphicsDevice, viewPorts[0]);
            allPlayers[1].Draw(spriteBatch, GraphicsDevice, viewPorts[0], allPlayers[0]);

            level.Draw(spriteBatch, GraphicsDevice, viewPorts[1], allPlayers[1]);
            allPlayers[1].Draw(spriteBatch, GraphicsDevice, viewPorts[1]);
            allPlayers[0].Draw(spriteBatch, GraphicsDevice, viewPorts[1], allPlayers[1]);

            rotation += (float)0.0001;
            allPlayers[1].setRotation(rotation);

            base.Draw(gameTime);
        }
    }
}
