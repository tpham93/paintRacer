using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace paintRacer
{
    class LoadWindow : IGameStateElements
    {
        /// <summary>
        /// Enum for the tasks of the LoadMenu
        /// </summary>
        private enum EMainState
        {
            /// <summary>
            /// choose between loading a knowen map or loading a new map
            /// </summary>
            MainMenu,
            /// <summary>
            /// load a knowen map
            /// </summary>
            LoadSavedMap,
            /// <summary>
            /// load a new map
            /// </summary>
            LoadNewMap
        }

        private enum ECreatState
        {
            /// <summary>
            /// default
            /// </summary>
            Nothing,
            /// <summary>
            /// edit the FileName for the color-picture
            /// </summary>
            EditFileNameColor,
            /// <summary>
            /// edit the FileName for the sw-picture
            /// </summary>
            EditFileNameSW,
            /// <summary>
            /// set a checkpoint (first part)
            /// </summary>
            SetCheckPoint_I,
            /// <summary>
            /// set a checkpoint (second part)
            /// </summary>
            SetCheckPoint_II,
            /// <summary>
            /// set the startpoint
            /// </summary>
            SetStartPoint_I,
            /// <summary>
            /// set the start-direction
            /// </summary>
            SetStartPoint_II,
            /// <summary>
            /// set the finish-line (first part)
            /// </summary>
            SetFinish_I,
            /// <summary>
            /// set the finish-line (second part)
            /// </summary>
            SetFinish_II
        }

        private EMainState mainState;
        private EGameStates nextState;
        private ECreatState createState;

        private string FileName = "";
        private string FileNameSW = "";

        //some textures and positions
        private Texture2D whitePixel;
        private Texture2D LoadNewMap;
        private Vector2 LoadNewMapPos;
        private Texture2D ChooseMap;
        private Vector2 ChooseMapPos;
        private Texture2D Back;
        private Vector2 BackPos;
        private Texture2D LoadMap;
        private Vector2 LoadMapPos;
        private Texture2D CheckPoint;
        private Vector2 CheckPointPos;
        private Texture2D Start;
        private Vector2 StartPos;
        private Texture2D Finish;
        private Vector2 FinishPos;
        private Texture2D remCheckPoint;
        private Vector2 remCheckPointPos;
        private Texture2D MapPic;
        private Texture2D MapPicSW;

        //const

        private const int MAPSIZE = 300;
        private const int MAPLEFTBOUND = 100;
        private const int MAINMENUBUTTONLEFTBOUND = 250;
        private const int LOADMENUBUTTONLEFTBOUND = 200;
        private const int CREATEMENUBUTTONLEFTBOUND = MAPSIZE + MAPLEFTBOUND + 100;
        private Color DEFAULT_COLOR = Color.Black;
        private SpriteFont DEFAULT_FONT;

        GraphicsDevice graphicsDevice;

        // constants___________________________________________
        const int MAINMENUENTRYSIZE_X = 187;
        const int MAINMENUENTRYSIZE_Y = 75;
        const int MAINMENUENTRYSPACE = 15;
        const int MAINMENUENTRYNUM = 3;

        const int MENUENTRYSIZE_X = 187;
        const int MENUENTRYSIZE_Y = 75;
        const int MENUENTRYSPACE = 15;
        const int LOADMENUENTRYNUM = 5;
        const int CHOOSEMENUENTRYNUM = 5;


        /// <summary>
        /// Constructor for the LoadWindow
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// the graphicsDevice
        /// <param name="next"></param>
        /// the next GameState (Multiplayer or Singleplayer)
        public LoadWindow(GraphicsDevice graphicsDevice, EGameStates next)
        {
            this.graphicsDevice = graphicsDevice;
            this.nextState = next;
        }

        /// <summary>
        /// Load Function
        /// </summary>
        /// <param name="content"></param>
        /// the ContentManager
        public void Load(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            mainState = EMainState.MainMenu;

            DEFAULT_FONT = content.Load<SpriteFont>(@"font");

            //load Textures
            whitePixel = Helper.loadImage("OneWithePixel.png");
            LoadNewMap = Helper.loadImage("loadMenu/LoadNewMap.png", new Rectangle(0, 0, MAINMENUENTRYSIZE_X, MAINMENUENTRYSIZE_Y));
            ChooseMap = Helper.loadImage("loadMenu/ChooseMap.png", new Rectangle(0, 0, MAINMENUENTRYSIZE_X, MAINMENUENTRYSIZE_Y));
            Back = Helper.loadImage("loadMenu/Back.png", new Rectangle(0, 0, MAINMENUENTRYSIZE_X, MAINMENUENTRYSIZE_Y));
            LoadMap = Helper.loadImage("loadMenu/Load.png", new Rectangle(0, 0, MENUENTRYSIZE_X, MENUENTRYSIZE_Y));
            CheckPoint = Helper.loadImage("loadMenu/Checkpoint.png", new Rectangle(0, 0, MENUENTRYSIZE_X, MENUENTRYSIZE_Y));
            Start = Helper.loadImage("loadMenu/Start.png", new Rectangle(0, 0, MENUENTRYSIZE_X, MENUENTRYSIZE_Y));
            Finish = Helper.loadImage("loadMenu/Finish.png", new Rectangle(0, 0, MENUENTRYSIZE_X, MENUENTRYSIZE_Y));
            remCheckPoint = Helper.loadImage("loadmenu/Remove.png", new Rectangle(0, 0, MENUENTRYSIZE_X, MENUENTRYSIZE_Y));
        }

        /// <summary>
        /// Update Function
        /// </summary>
        /// <param name="gameTime"></param>
        /// the gameTime
        /// <returns>the next gamestate - EGameState</returns>
        public EGameStates Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            switch (mainState) {
                case EMainState.LoadNewMap : 
                    return UpdateLoadNewMap(gameTime);
                case EMainState.LoadSavedMap :
                    return UpdateLoadSavedMap(gameTime);
                case EMainState.MainMenu :
                    return UpdateMainMenu(gameTime);
                default :
                    return UpdateMainMenu(gameTime);
            }

        }

        /// <summary>
        /// Update if load a saved map
        /// </summary>
        /// <param name="gameTime"></param>
        /// the gameTime
        /// <returns>the next gamestate - EGameState</returns>
        private EGameStates UpdateLoadSavedMap(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            FileName += Helper.KeyToChar(keyboardState);

            MouseState mouse = Mouse.GetState();
            if (mouse.LeftButton == ButtonState.Pressed) 
            {
                if ((mouse.X > BackPos.X) && (mouse.X < BackPos.X + MENUENTRYSIZE_X) && (mouse.Y > BackPos.Y) && (mouse.Y < BackPos.Y + MENUENTRYSIZE_Y))
                {
                    FileName = "";
                    mainState = EMainState.MainMenu;
                }

                if ((mouse.X > LoadMapPos.X) && (mouse.X < LoadMapPos.X + MENUENTRYSIZE_X) && (mouse.Y > LoadMapPos.Y) && (mouse.Y < LoadMapPos.Y + MENUENTRYSIZE_Y))
                {
                    try 
                    {
                        Global.map = XmlLoad.parseMapConfig(FileName);
                    }
                    catch (Exception e)
                    {
                        //return EGameStates.LoadMenu;
                    }

                    return nextState;
                }
            }

            return EGameStates.LoadMenu;
        }

        /// <summary>
        /// Update if in MainMenu
        /// </summary>
        /// <param name="gameTime"></param>
        /// the gameTime
        /// <returns>the next gamestate - EGameState</returns>
        private EGameStates UpdateMainMenu(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();

            if (mouse.LeftButton == ButtonState.Pressed)
            {
                if ((mouse.X > BackPos.X) && (mouse.X < BackPos.X + MENUENTRYSIZE_X) && (mouse.Y > BackPos.Y) && (mouse.Y < BackPos.Y + MENUENTRYSIZE_Y))
                {
                    return EGameStates.Menue;
                }

                else if ((mouse.X > LoadMapPos.X) && (mouse.X < LoadMapPos.X + MENUENTRYSIZE_X) && (mouse.Y > LoadMapPos.Y) && (mouse.Y < LoadMapPos.Y + MENUENTRYSIZE_Y))
                {
                    mainState = EMainState.LoadNewMap;
                }

                else if ((mouse.X > ChooseMapPos.X) && (mouse.X < ChooseMapPos.X + MENUENTRYSIZE_X) && (mouse.Y > ChooseMapPos.Y) && (mouse.Y < ChooseMapPos.Y + MENUENTRYSIZE_Y))
                {
                    mainState = EMainState.LoadSavedMap;
                }
            }
            
            return EGameStates.LoadMenu;
        }

        /// <summary>
        /// Update if load a new map
        /// </summary>
        /// <param name="gameTime"></param>
        /// the gameTime
        /// <returns>the next gamestate - EGameState</returns>
        private EGameStates UpdateLoadNewMap(GameTime gameTime)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Draw the Window
        /// </summary>
        /// <param name="gameTime"></param>
        /// the gameTime
        /// <param name="spriteBatch"></param>
        /// the spriteBatch to draw in
        public void Draw(Microsoft.Xna.Framework.GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            switch (mainState)
            {
                case EMainState.LoadNewMap:
                    DrawLoadNewMap(gameTime, spriteBatch);
                    break;
                case EMainState.LoadSavedMap:
                    DrawLoadSavedMap(gameTime, spriteBatch);
                    break;
                case EMainState.MainMenu:
                    DrawMainMenu(gameTime, spriteBatch);
                    break;
                default:
                    DrawMainMenu(gameTime, spriteBatch);
                    break;
            }
        }

        /// <summary>
        /// Draw function for Main Menu
        /// </summary>
        /// <param name="gameTime"></param>
        /// the gameTime
        /// <param name="spriteBatch"></param>
        /// the spriteBatch to draw in
        private void DrawMainMenu(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            Vector2 pos = new Vector2(MAINMENUBUTTONLEFTBOUND, MAINMENUENTRYSPACE);
            spriteBatch.Draw(ChooseMap, pos, Color.White);
            ChooseMapPos = new Vector2(pos.X, pos.Y);

            pos.Y += MAINMENUENTRYSPACE + MAINMENUENTRYSIZE_Y;
            spriteBatch.Draw(LoadNewMap, pos, Color.White);
            LoadMapPos = new Vector2(pos.X, pos.Y);

            pos.Y += MAINMENUENTRYSPACE + MAINMENUENTRYSIZE_Y;
            spriteBatch.Draw(Back, pos, Color.White);
            BackPos = new Vector2(pos.X, pos.Y);

            spriteBatch.End();
        }

        /// <summary>
        /// Draw function for the window to load a map
        /// </summary>
        /// <param name="gameTime"></param>
        /// the gameTime
        /// <param name="spriteBatch"></param>
        /// the spriteBatch to draw in
        private void DrawLoadSavedMap(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            Vector2 pos = new Vector2(LOADMENUBUTTONLEFTBOUND, MENUENTRYSPACE);
            spriteBatch.DrawString(DEFAULT_FONT, FileName, pos, DEFAULT_COLOR);

            pos.Y += MENUENTRYSPACE + MENUENTRYSIZE_Y;
            spriteBatch.Draw(LoadMap, pos, Color.White);
            LoadMapPos = new Vector2(pos.X, pos.Y);

            spriteBatch.End();
        }

        /// <summary>
        /// Draw function for the window to create a new map
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        private void DrawLoadNewMap(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            //Map
            if (MapPic != null)
            {
                int x = MapPic.Width;
                int y = MapPic.Height;

                int max = Math.Max(x, y);
                float scal = (float)MAPSIZE / (float)max;

                x = (int)(x * scal);
                y = (int)(y * scal);

                spriteBatch.Draw(MapPic, new Rectangle(MAPLEFTBOUND, MAPLEFTBOUND, x, y), Color.White);
            }
            else
            {
                spriteBatch.Draw(whitePixel, new Rectangle(MAPLEFTBOUND, MAPLEFTBOUND, MAPSIZE, MAPSIZE), Color.Moccasin);
            }

            //Text
            Vector2 pos = new Vector2(CREATEMENUBUTTONLEFTBOUND, MENUENTRYSPACE);
            spriteBatch.DrawString(DEFAULT_FONT, "Color-File: " + FileName, pos, DEFAULT_COLOR);

            pos.Y += MENUENTRYSIZE_Y + MENUENTRYSPACE;
            spriteBatch.DrawString(DEFAULT_FONT, "SW-File: " + FileNameSW, pos, DEFAULT_COLOR);

            //Buttons
            pos.Y += MENUENTRYSIZE_Y + MENUENTRYSPACE;
            spriteBatch.Draw(LoadMap, pos, Color.White);
            LoadMapPos = new Vector2(pos.X, pos.Y);

            pos.Y += MENUENTRYSIZE_Y + MENUENTRYSPACE;
            spriteBatch.Draw(CheckPoint, pos, Color.White);
            CheckPointPos = new Vector2(pos.X, pos.Y);

            remCheckPointPos = new Vector2(pos.X, pos.Y);
            remCheckPointPos.X += MENUENTRYSIZE_X + MENUENTRYSPACE;
            spriteBatch.Draw(remCheckPoint, remCheckPointPos, Color.White);

            pos.Y += MENUENTRYSIZE_Y + MENUENTRYSPACE;
            spriteBatch.Draw(Start, pos, Color.White);
            StartPos = new Vector2(pos.X, pos.Y);

            pos.Y += MENUENTRYSIZE_Y + MENUENTRYSPACE;
            spriteBatch.Draw(Finish, pos, Color.White);
            FinishPos = new Vector2(pos.X, pos.Y);

            pos.Y += MENUENTRYSIZE_Y + MENUENTRYSPACE;
            spriteBatch.DrawString(DEFAULT_FONT, Info(), pos, DEFAULT_COLOR);
        }

        private string Info()
        {
            switch (createState)
            {
                case ECreatState.Nothing :
                    return "Load a Map and its black-and-white-dublicate:\n" +
                            "black   - road\n" +
                            "greay   - offroad\n" +
                            "white   - object";
                case ECreatState.EditFileNameColor :
                    return "Enter the location of the color-picture.";
                case ECreatState.EditFileNameSW :
                    return "Enter the location of the black-and-white-picture:\n" +
                            "black   - road\n" +
                            "greay   - offroad\n" +
                            "white   - object";
                case ECreatState.SetCheckPoint_I :
                    return "Set a checkpoint:\n" +
                            "A checkpoint are two points on both sides of the road.\n" +
                            "Click in the map to set the first one.";
                case ECreatState.SetCheckPoint_II :
                    return "Set a checkpoint:\n" +
                            "A checkpoint are two points on both sides of the road.\n" +
                            "Click in the map to set the second one.";
                case ECreatState.SetStartPoint_I :
                    return "Click in the map to set the point, were the cars start.";
                case ECreatState.SetStartPoint_II :
                    return "Click in the map to set the direction, the cars drive in.";
                case ECreatState.SetFinish_I :
                    return "The finish is defind by two points on both sides of the road:\n" +
                            "Click in the map to set the first one.";
                case ECreatState.SetFinish_II :
                    return "The finish is defind by two points on both sides of the road:\n" +
                            "Click in the map to set the second one.";
                default :
                    return "I can't help you!";
            }
        }

        /// <summary>
        /// Unload
        /// </summary>
        public void Unload()
        {}
    }
}
