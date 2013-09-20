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
using paintRacer.gameStates;

namespace paintRacer
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        EGameStates gameState;

        IGameStateElements gameStateElement;
        IGameStateElements tmpGameStateElement;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //Removes FPS lock and VSync
            graphics.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = false;
            IsMouseVisible = true;
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
            Console.Out.WriteLine(Window.ClientBounds);
            GameState = EGameStates.Menue;
            //Initializes Level and Player with test textures
            //level = new Level(MAP_PIC, Level.MapType.rawImage);
            //players = new Player[2];
            //players[0] = new Player(Helper.loadImage("testcar.png"), Color.Blue);
            //players[1] = new Player(Helper.loadImage("testcar.png"), Color.Red);
            //players[0].setPosition(new Vector2(START_POS.X - 40, START_POS.Y));
            //players[1].setPosition(new Vector2(START_POS.X + 40, START_POS.Y));

            Keys[,] keys = { { Keys.W, Keys.S, Keys.A, Keys.D, Keys.Q, Keys.E, Keys.F }, { Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.Escape, Keys.Enter, Keys.Back } };
            Config.setKeys(keys);

            //scene = new Scene(level, players, GraphicsDevice.Viewport, Config.getKeys());

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

            ///*
            GameState = gameStateElement.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary> 
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            //scene.Draw(spriteBatch, GraphicsDevice);

            gameStateElement.Draw(gameTime, spriteBatch);

            base.Draw(gameTime);
        }

        

        internal EGameStates GameState
        {
            get { return gameState; }
            set
            {
                // just change if the actual gamestate is being switched
                if (gameState != value)
                {
                    // changes value of gameState and changes type/object of gameStateElement
                    switch (value)
                    {
                        case EGameStates.Menue:
                            gameStateElement = new Menu();
                            tmpGameStateElement = null;
                            gameStateElement.Load(Content);
                            break;
                        case EGameStates.SinglePlayer:
                            // if gemstate was EGameStates.Pause then load paused game, otherwise create a new one
                            if (gameState == EGameStates.Pause)
                            {
                                gameStateElement = tmpGameStateElement;
                            }
                            else
                            {
                                gameStateElement = new Singleplayer(GraphicsDevice, Global.players, Global.map);
                                gameStateElement.Load(Content);
                            }
                            tmpGameStateElement = null;
                            break;
                        case EGameStates.MultiPlayer:
                            // if gemstate was EGameStates.Pause then load paused game, otherwise create a new one
                            if (gameState == EGameStates.Pause)
                            {
                                gameStateElement = tmpGameStateElement;
                            }
                            else
                            {
                                gameStateElement = new Multiplayer(GraphicsDevice,Global.players,Global.map);
                                gameStateElement.Load(Content);
                            }
                            tmpGameStateElement = null;
                            break;
                        case EGameStates.Pause:
                            tmpGameStateElement = gameStateElement;
                            //gameStateElement = new Pause();
                            //gameStateElement.Load(Content);
                            break;
                        case EGameStates.Close:
                            Exit();
                            break;
                        case EGameStates.LoadMenu:
                            // not required if LoadMenuSinglePlayer or LoadMenuMultiplayer is set before
                            if (gameState != EGameStates.LoadMenuMultiplayer && gameState != EGameStates.LoadMenuSinglePlayer)
                            {
                                throw new Exception("impossible state reached!");
                            }
                            break;
                        case EGameStates.LoadMenuSinglePlayer:
                            gameStateElement = new LoadWindow(GraphicsDevice, EGameStates.CarSelectionSingleplayer);
                            tmpGameStateElement = null;
                            gameStateElement.Load(Content);
                            break;
                        case EGameStates.LoadMenuMultiplayer:
                            gameStateElement = new LoadWindow(GraphicsDevice, EGameStates.CarSelectionMultiplayer);
                            tmpGameStateElement = null;
                            gameStateElement.Load(Content);
                            break;
                        case EGameStates.Credits:
                            gameStateElement = new Credits();
                            gameStateElement.Load(Content);
                            break;
                        case EGameStates.CarSelection:
                            // not required if CarSelectionSingleplayer or CarSelectionMultiplayer is set before
                            if (gameState != EGameStates.CarSelectionSingleplayer && gameState != EGameStates.CarSelectionMultiplayer)
                            {
                                throw new Exception("impossible state reached!");
                            }
                            break;
                        case EGameStates.CarSelectionSingleplayer:
                            gameStateElement = new CarSelection(EGameStates.SinglePlayer);
                            tmpGameStateElement = null;
                            gameStateElement.Load(Content);
                            break;
                        case EGameStates.CarSelectionMultiplayer:
                            gameStateElement = new CarSelection(EGameStates.MultiPlayer);
                            tmpGameStateElement = null;
                            gameStateElement.Load(Content);
                            break;
                    }
                    gameState = value;
                }
            }
        }
    }
}
