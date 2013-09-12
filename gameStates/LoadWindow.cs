using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
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

        private string[] directoryarray;
        private string FileName = "";
        private string FileNameSW = "";

        private int scrollpos = 0;
        private int timeSpace = 0;

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
        private Texture2D Create;
        private Vector2 CreatePos;
        private Texture2D MapPic;
        private Texture2D MapPicSW;

        private Vector2 textArrayPos;
        private Vector2 fileNamePos;
        private Vector2 fileNameSWPos;

        private Vector2[] checkPoints;
        private Vector2[] StartPosDirection;

        //const

        private const int MAPSIZE = 350;
        private const int MAPLEFTBOUND = 25;
        private const int MAINMENUBUTTONLEFTBOUND = 100;
        private const int LOADMENUBUTTONLEFTBOUND = 200;
        private const int CREATEMENUBUTTONLEFTBOUND = MAPSIZE + MAPLEFTBOUND + 25;
        private Color DEFAULT_COLOR = Color.Black;
        private SpriteFont DEFAULT_FONT;

        GraphicsDevice graphicsDevice;

        // constants___________________________________________
        const int MAINMENUENTRYSIZE_X = 187;
        const int MAINMENUENTRYSIZE_Y = 75;
        const int MAINMENUENTRYSPACE = 15;
        const int MAINMENUENTRYNUM = 3;

        const int MENUENTRYSIZE_X = 125;
        const int MENUENTRYSIZE_Y = 50;
        const int MENUENTRYSPACE = 10;
        const int LOADMENUENTRYNUM = 5;
        const int CHOOSEMENUENTRYNUM = 5;

        const int MENULINESIZE = 20;
        const int MENULINESPACE = 5;

        const int TIME_BETWEEN_KEY_PRESS = 150;
        const int NUM_ENTRIES = 5;


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
            
            directoryarray = Directory.GetDirectories("saved_maps");

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
            Create = Helper.loadImage("loadmenu/Create.png", new Rectangle(0, 0, MENUENTRYSIZE_X, MENUENTRYSIZE_Y));
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
            MouseState mouseState = Mouse.GetState();

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if ((mouseState.X > BackPos.X) && (mouseState.X < BackPos.X + MENUENTRYSIZE_X) && (mouseState.Y > BackPos.Y) && (mouseState.Y < BackPos.Y + MENUENTRYSIZE_Y))
                {
                    mainState = EMainState.MainMenu;
                    scrollpos = 0;
                }
                for (int count = 0; count < NUM_ENTRIES; ++count)
                {
                    int offset = count * (MENULINESIZE + MENULINESPACE);
                    if ((mouseState.X > textArrayPos.X) && (mouseState.Y > textArrayPos.Y + offset) && (mouseState.Y < textArrayPos.Y + offset + MENULINESIZE))
                    {
                        //try
                        //{
                            Global.map = XmlLoad.parseMapConfig(directoryarray[count + scrollpos]);
                        //}
                        //catch
                        //{
                        //    //return EGameStates.LoadMenu;
                        //}
                        scrollpos = 0;
                        return nextState;
                    }
                }
            }

            timeSpace += gameTime.ElapsedGameTime.Milliseconds;

            if (timeSpace > TIME_BETWEEN_KEY_PRESS)
            {
                timeSpace = 0;
                KeyboardState keyboartState = Keyboard.GetState();
                if (keyboartState.IsKeyDown(Keys.Down))
                {
                    ++scrollpos;
                    scrollpos = scrollpos >= directoryarray.Length ? directoryarray.Length-1 : scrollpos;
                }
                else if (keyboartState.IsKeyDown(Keys.Up))
                {
                    --scrollpos;
                    scrollpos = scrollpos < 0 ? 0 : scrollpos;
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
            MouseState mouseState = Mouse.GetState();

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if ((mouseState.X > fileNamePos.X) && (mouseState.Y > fileNamePos.Y) && (mouseState.Y < fileNamePos.Y + MENULINESIZE))
                {
                    createState = ECreatState.EditFileNameColor;
                }
                else if ((mouseState.X > fileNameSWPos.X) && (mouseState.Y > fileNameSWPos.Y) && (mouseState.Y < fileNameSWPos.Y + MENULINESIZE))
                {
                    createState = ECreatState.EditFileNameSW;
                }
                else if ((mouseState.X > LoadMapPos.X) && (mouseState.X < LoadMapPos.X + MENUENTRYSIZE_X) && (mouseState.Y > LoadMapPos.Y) && (mouseState.Y < LoadMapPos.Y + MENUENTRYSIZE_Y))
                {
                    createState = ECreatState.Nothing;
                    try
                    {
                        MapPic = Helper.loadImage("map_pictures\\" + FileName);
                        MapPicSW = Helper.loadImage("map_pictures\\" + FileNameSW);
                    }
                    catch
                    {
                        return EGameStates.LoadMenu;
                    }
                }
                else if ((mouseState.X > CheckPointPos.X) && (mouseState.X < CheckPointPos.X + MENUENTRYSIZE_X) && (mouseState.Y > CheckPointPos.Y) && (mouseState.Y < CheckPointPos.Y + MENUENTRYSIZE_Y))
                {
                    createState = ECreatState.SetCheckPoint_I;
                }
                else if ((mouseState.X > remCheckPointPos.X) && (mouseState.X < remCheckPointPos.X + MENUENTRYSIZE_X) && (mouseState.Y > remCheckPointPos.Y) && (mouseState.Y < remCheckPointPos.Y + MENUENTRYSIZE_Y))
                {
                    createState = ECreatState.Nothing;
                }
                else if ((mouseState.X > StartPos.X) && (mouseState.X < StartPos.X + MENUENTRYSIZE_X) && (mouseState.Y > StartPos.Y) && (mouseState.Y < StartPos.Y + MENUENTRYSIZE_Y))
                {
                    createState = ECreatState.SetStartPoint_I;
                }
                else if ((mouseState.X > FinishPos.X) && (mouseState.X < FinishPos.X + MENUENTRYSIZE_X) && (mouseState.Y > FinishPos.Y) && (mouseState.Y < FinishPos.Y + MENUENTRYSIZE_Y))
                {
                    createState = ECreatState.SetFinish_I;
                }
                else if ((mouseState.X > CreatePos.X) && (mouseState.X < CreatePos.X + MENUENTRYSIZE_X) && (mouseState.Y > CreatePos.Y) && (mouseState.Y < CreatePos.Y + MENUENTRYSIZE_Y))
                {
                    Global.map = new Map(MapPic, MapReader.createDataFromSWImage(MapPicSW), checkPoints, StartPosDirection[0], Physic.calculateRotation(StartPosDirection[1]));
                    return nextState;
                }
            }

            return EGameStates.LoadMenu;
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
            textArrayPos = new Vector2(pos.X, pos.Y);

            if (directoryarray != null)
                for (int count = scrollpos; (count < scrollpos + NUM_ENTRIES) && (count < directoryarray.Length); count++)
                {
                    pos.Y = (count - scrollpos) * (MENULINESIZE + MENULINESPACE) + MENUENTRYSPACE;
                    spriteBatch.DrawString(DEFAULT_FONT, directoryarray[count].Substring(directoryarray[count].LastIndexOf('\\')+1), pos, DEFAULT_COLOR);
                }

            pos.Y += MENUENTRYSPACE + MENUENTRYSIZE_Y;
            spriteBatch.Draw(Back, pos, Color.White);
            BackPos = new Vector2(pos.X, pos.Y);

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
            fileNamePos = new Vector2(pos.X, pos.Y);

            pos.Y += MENULINESIZE + MENULINESPACE;
            spriteBatch.DrawString(DEFAULT_FONT, "SW-File: " + FileNameSW, pos, DEFAULT_COLOR);
            fileNameSWPos = new Vector2(pos.X, pos.Y);

            //Buttons
            pos.Y += MENULINESIZE + MENULINESPACE;
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

            CreatePos = new Vector2(MAPLEFTBOUND, 2 * MAPLEFTBOUND + MAPSIZE);
            spriteBatch.Draw(Create, CreatePos, Color.White);

            spriteBatch.End();
        }

        private string Info()
        {
            switch (createState)
            {
                case ECreatState.Nothing :
                    return "Load a Map and its black-and-\nwhite-dublicate:\n" +
                            "black   - road\n" +
                            "greay   - offroad\n" +
                            "white   - object";
                case ECreatState.EditFileNameColor :
                    return "Enter the location of the color-\npicture.";
                case ECreatState.EditFileNameSW :
                    return "Enter the location of the black-and-\nwhite-picture:\n" +
                            "black   - road\n" +
                            "greay   - offroad\n" +
                            "white   - object";
                case ECreatState.SetCheckPoint_I :
                    return "Set a checkpoint:\n" +
                            "A checkpoint are two points on both \nsides of the road.\n" +
                            "Click in the map to set the first \none.";
                case ECreatState.SetCheckPoint_II :
                    return "Set a checkpoint:\n" +
                            "A checkpoint are two points on both \nsides of the road.\n" +
                            "Click in the map to set the second \none.";
                case ECreatState.SetStartPoint_I :
                    return "Click in the map to set the point, \nwere the cars start.";
                case ECreatState.SetStartPoint_II :
                    return "Click in the map to set the direction, \nthe cars drive in.";
                case ECreatState.SetFinish_I :
                    return "The finish is defind by two points \non both sides of the road:\n" +
                            "Click in the map to set the first \none.";
                case ECreatState.SetFinish_II :
                    return "The finish is defind by two points \non both sides of the road:\n" +
                            "Click in the map to set the second \none.";
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
