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

        //Test Level and Player
        Level level;
        Player player;
        Player player2;

        //Test Viewports to be put someplace else in the future
        Viewport defaultView;
        Viewport lView;
        Viewport rView;

        float rotation = 0;

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
            level = new Level("test.png");
            player = new Player("testcar.png", Color.Blue);
            player2 = new Player("testcar.png", Color.Red);
            player2.setPosition(new Vector2(0, 0));
            player2.setRotation(MathHelper.ToRadians(90));

            //Initializes the Viewports
            defaultView = GraphicsDevice.Viewport;
            lView = defaultView;
            rView = defaultView;
            lView.Width /= 2;
            rView.Width /= 2;
            rView.X = lView.Width;


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
            level.Draw(spriteBatch, GraphicsDevice, lView, player);
            player2.Draw(spriteBatch, GraphicsDevice, lView, player);
            player.Draw(spriteBatch, GraphicsDevice, lView);
            
            level.Draw(spriteBatch, GraphicsDevice, rView, player2);
            player.Draw(spriteBatch, GraphicsDevice, rView, player2);
            player2.Draw(spriteBatch, GraphicsDevice, rView);

            rotation += (float)0.0001;
            player2.setRotation(rotation);

            base.Draw(gameTime);
        }
    }
}
