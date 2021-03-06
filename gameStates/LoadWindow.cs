﻿using System;
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
            SetFinish_II,
            /// <summary>
            /// if an error happens
            /// </summary>
            Error
        }

        private EMainState mainState;
        private EGameStates nextState;
        private ECreatState createState;

        private string[] directoryarray;
        private string fileName = "";
        private string FileNameSW = "";

        private int scrollpos = 0;
        private TimeSpan timeSpace = new TimeSpan();
        private float scal;

        //some textures and positions
        private Texture2D whitePixel;
        private Texture2D LoadNewMap;
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
        private Vector2 MapPicPos;
        private Texture2D bgPic;

        private Vector2 textArrayPos;
        private Vector2 fileNamePos;
        private Vector2 fileNameSWPos;

        private Vector2[] checkPoints;
        private Vector2[] StartPosDirection;
        private Vector2[] FinishPoints;

        //const

        private const int MAPSIZE = 350;
        private const int MAPLEFTBOUND = 25;
        private const int MAINMENUBUTTONLEFTBOUND = 500;
        private const int LOADMENUBUTTONLEFTBOUND = 200;
        private const int CREATEMENUBUTTONLEFTBOUND = MAPSIZE + MAPLEFTBOUND + 25;
        private SpriteFont DEFAULT_FONT;

        GraphicsDevice graphicsDevice;

        // constants
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

            directoryarray = Directory.GetDirectories(@"saved_maps\").Where(file => File.Exists(file + @"\map.xml")).ToArray<string>();

            DEFAULT_FONT = content.Load<SpriteFont>(@"font");

            //load Textures
            whitePixel = Helper.genRectangleTexture(1, 1, Color.White);
            LoadNewMap = Helper.loadImage(@"Content\Buttons\LoadNewMap.png", new Rectangle(0, 0, (int)Config.BIG_BUTTON.X, (int)Config.BIG_BUTTON.Y));
            Back = Helper.loadImage(@"Content\Buttons\Back.png", new Rectangle(0, 0, (int)Config.BIG_BUTTON.X, (int)Config.BIG_BUTTON.Y));
            LoadMap = Helper.loadImage(@"Content\Buttons\Load.png", new Rectangle(0, 0, (int)Config.SMALL_BUTTON.X, (int)Config.SMALL_BUTTON.Y));
            CheckPoint = Helper.loadImage(@"Content\Buttons\Checkpoint.png", new Rectangle(0, 0, (int)Config.SMALL_BUTTON.X, (int)Config.SMALL_BUTTON.Y));
            Start = Helper.loadImage(@"Content\Buttons\Start.png", new Rectangle(0, 0, (int)Config.SMALL_BUTTON.X, (int)Config.SMALL_BUTTON.Y));
            Finish = Helper.loadImage(@"Content\Buttons\Finish.png", new Rectangle(0, 0, (int)Config.SMALL_BUTTON.X, (int)Config.SMALL_BUTTON.Y));
            remCheckPoint = Helper.loadImage(@"Content\Buttons\Remove.png", new Rectangle(0, 0, (int)Config.SMALL_BUTTON.X, (int)Config.SMALL_BUTTON.Y));
            Create = Helper.loadImage(@"Content\Buttons\Create.png", new Rectangle(0, 0, (int)Config.SMALL_BUTTON.X, (int)Config.SMALL_BUTTON.Y));
            bgPic = Helper.loadImage(@"Content\Backgrounds\track.png");
        }

        /// <summary>
        /// Update Function
        /// </summary>
        /// <param name="gameTime"></param>
        /// the gameTime
        /// <returns>the next gamestate - EGameState</returns>
        public EGameStates Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            switch (mainState)
            {
                case EMainState.LoadNewMap:
                    return UpdateLoadNewMap(gameTime);
                case EMainState.MainMenu:
                    return UpdateMainMenu(gameTime);
                default:
                    return UpdateMainMenu(gameTime);
            }

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
            timeSpace += gameTime.ElapsedGameTime;
            if (mouse.LeftButton == ButtonState.Pressed && timeSpace > Config.TIME_BETWEEN_SAME_EVENT)
            {
                if ((mouse.X > BackPos.X) && (mouse.X < BackPos.X + (int)Config.BIG_BUTTON.X) && (mouse.Y > BackPos.Y) && (mouse.Y < BackPos.Y + (int)Config.BIG_BUTTON.Y))
                {
                    timeSpace = new TimeSpan();
                    return EGameStates.Menue;
                }

                else if ((mouse.X > LoadMapPos.X) && (mouse.X < LoadMapPos.X + (int)Config.BIG_BUTTON.X) && (mouse.Y > LoadMapPos.Y) && (mouse.Y < LoadMapPos.Y + (int)Config.BIG_BUTTON.Y))
                {
                    timeSpace = new TimeSpan();
                    mainState = EMainState.LoadNewMap;
                }

                for (int count = 0; count < NUM_ENTRIES; ++count)
                {
                    int offset = count * (Config.LINE_SIZE + Config.BIG_LINE_SPACE);
                    if ((mouse.X > textArrayPos.X) && (mouse.Y > textArrayPos.Y + offset) && (mouse.Y < textArrayPos.Y + offset + Config.LINE_SIZE))
                    {
                        Global.map = XmlLoad.parseMapConfig(directoryarray[count + scrollpos]);
                        scrollpos = 0;
                        timeSpace = new TimeSpan();
                        return nextState;
                    }
                }
            }

            if (timeSpace > Config.TIME_BETWEEN_SAME_EVENT)
            {
                KeyboardState keyboartState = Keyboard.GetState();
                if (keyboartState.IsKeyDown(Keys.Down))
                {
                    ++scrollpos;
                    scrollpos = scrollpos >= directoryarray.Length ? directoryarray.Length - 1 : scrollpos;
                    timeSpace = new TimeSpan();
                }
                else if (keyboartState.IsKeyDown(Keys.Up))
                {
                    --scrollpos;
                    scrollpos = scrollpos < 0 ? 0 : scrollpos;
                    timeSpace = new TimeSpan();
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
            timeSpace += gameTime.ElapsedGameTime;
            MouseState mouseState = Mouse.GetState();

            if (mouseState.LeftButton == ButtonState.Pressed && timeSpace > Config.TIME_BETWEEN_SAME_EVENT)
            {
                timeSpace = new TimeSpan();
                if ((mouseState.X > fileNamePos.X) && (mouseState.Y > fileNamePos.Y) && (mouseState.Y < fileNamePos.Y + Config.LINE_SIZE))
                {
                    createState = ECreatState.EditFileNameColor;
                }
                else if ((mouseState.X > fileNameSWPos.X) && (mouseState.Y > fileNameSWPos.Y) && (mouseState.Y < fileNameSWPos.Y + Config.LINE_SIZE))
                {
                    createState = ECreatState.EditFileNameSW;
                }
                else if ((mouseState.X > LoadMapPos.X) && (mouseState.X < LoadMapPos.X + (int)Config.SMALL_BUTTON.X) && (mouseState.Y > LoadMapPos.Y) && (mouseState.Y < LoadMapPos.Y + (int)Config.SMALL_BUTTON.Y))
                {
                    createState = ECreatState.Nothing;
                    try
                    {
                        MapPic = Helper.loadImage("map_pictures\\" + fileName);
                        MapPicSW = Helper.loadImage("map_pictures\\" + FileNameSW);
                    }
                    catch
                    {
                        return EGameStates.LoadMenu;
                    }
                }
                else if ((mouseState.X > CheckPointPos.X) && (mouseState.X < CheckPointPos.X + (int)Config.SMALL_BUTTON.X) && (mouseState.Y > CheckPointPos.Y) && (mouseState.Y < CheckPointPos.Y + (int)Config.SMALL_BUTTON.Y))
                {
                    createState = ECreatState.SetCheckPoint_I;
                }
                else if ((mouseState.X > remCheckPointPos.X) && (mouseState.X < remCheckPointPos.X + (int)Config.SMALL_BUTTON.X) && (mouseState.Y > remCheckPointPos.Y) && (mouseState.Y < remCheckPointPos.Y + (int)Config.SMALL_BUTTON.Y))
                {
                    Helper.resizeV2Array(checkPoints, -2);
                    createState = ECreatState.Nothing;
                }
                else if ((mouseState.X > StartPos.X) && (mouseState.X < StartPos.X + (int)Config.SMALL_BUTTON.X) && (mouseState.Y > StartPos.Y) && (mouseState.Y < StartPos.Y + (int)Config.SMALL_BUTTON.Y))
                {
                    createState = ECreatState.SetStartPoint_I;
                }
                else if ((mouseState.X > FinishPos.X) && (mouseState.X < FinishPos.X + (int)Config.SMALL_BUTTON.X) && (mouseState.Y > FinishPos.Y) && (mouseState.Y < FinishPos.Y + (int)Config.SMALL_BUTTON.Y))
                {
                    createState = ECreatState.SetFinish_I;
                }
                else if ((mouseState.X > CreatePos.X) && (mouseState.X < CreatePos.X + (int)Config.SMALL_BUTTON.X) && (mouseState.Y > CreatePos.Y) && (mouseState.Y < CreatePos.Y + (int)Config.SMALL_BUTTON.Y))
                {
                    //try
                    {
                        Vector2[] points = Helper.resizeV2Array(checkPoints, 2);
                        points[points.Length - 2] = FinishPoints[0];
                        points[points.Length - 1] = FinishPoints[1];
                        string[] addressParts = fileName.Split(new char[] { '\\', '/', '.' });
                        string mapName = XMLSave.getDirectoryName(addressParts[addressParts.Length - 2]);
                        Directory.CreateDirectory(@"saved_maps\" + mapName);
                        Global.map = new Map(MapPic, MapPicSW, @"saved_maps\" + mapName, points, StartPosDirection[0], Physic.calculateRotation(StartPosDirection[1] - StartPosDirection[0]));
                        XMLSave.saveMap(@"saved_maps\" + mapName, Global.map);
                        return nextState;
                    }
                    //catch
                    //{
                    //    createState = ECreatState.Error;
                    //    return EGameStates.LoadMenu;
                    //}
                }
                else if ((mouseState.X > BackPos.X) && (mouseState.X < BackPos.X + (int)Config.SMALL_BUTTON.X) && (mouseState.Y > BackPos.Y) && (mouseState.Y < BackPos.Y + (int)Config.SMALL_BUTTON.Y))
                {
                    MapPic = null;
                    MapPicSW = null;
                    checkPoints = null;
                    FinishPoints = null;
                    StartPosDirection = null;
                    createState = ECreatState.Nothing;
                    mainState = EMainState.MainMenu;
                    return EGameStates.LoadMenu;
                }
                else if ((mouseState.X > MapPicPos.X) && (mouseState.X < MapPicPos.X + MAPSIZE) && (mouseState.Y > MapPicPos.Y) && (mouseState.Y < MapPicPos.Y + MAPSIZE))
                {
                    Vector2 relativePos = new Vector2(mouseState.X, mouseState.Y) - MapPicPos;

                    switch (createState)
                    {
                        case ECreatState.SetCheckPoint_II:
                            checkPoints[checkPoints.Length - 1] = new Vector2((int)(relativePos.X / scal), (int)(relativePos.Y / scal));
                            createState = ECreatState.Nothing;
                            break;
                        case ECreatState.SetCheckPoint_I:
                            if (checkPoints == null)
                                checkPoints = new Vector2[2];
                            else
                                checkPoints = Helper.resizeV2Array(checkPoints, 2);
                            checkPoints[checkPoints.Length - 2] = new Vector2((int)(relativePos.X / scal), (int)(relativePos.Y / scal));
                            createState = ECreatState.SetCheckPoint_II;
                            break;
                        case ECreatState.SetFinish_II:
                            FinishPoints[1] = new Vector2((int)(relativePos.X / scal), (int)(relativePos.Y / scal));
                            createState = ECreatState.Nothing;
                            break;
                        case ECreatState.SetFinish_I:
                            FinishPoints = new Vector2[2];
                            FinishPoints[0] = new Vector2((int)(relativePos.X / scal), (int)(relativePos.Y / scal));
                            createState = ECreatState.SetFinish_II;
                            break;
                        case ECreatState.SetStartPoint_II:
                            StartPosDirection[1] = new Vector2((int)(relativePos.X / scal), (int)(relativePos.Y / scal));
                            createState = ECreatState.Nothing;
                            break;
                        case ECreatState.SetStartPoint_I:
                            StartPosDirection = new Vector2[2];
                            StartPosDirection[0] = new Vector2((int)(relativePos.X / scal), (int)(relativePos.Y / scal));
                            createState = ECreatState.SetStartPoint_II;
                            break;
                    }
                }
            }

            if (createState == ECreatState.EditFileNameColor)
            {
                KeyboardState keyboardState = Keyboard.GetState();
                if (timeSpace > Config.TIME_BETWEEN_SAME_EVENT)
                {
                    if (keyboardState.IsKeyDown(Keys.Back) && fileName.Length > 0)
                    {
                        fileName = fileName.Substring(0, fileName.Length - 1);
                        timeSpace = new TimeSpan();
                    }
                    else
                    {
                        string input = Helper.KeyToChar(keyboardState, gameTime);
                        if (!input.Equals(""))
                        {
                            fileName += input;
                            timeSpace = new TimeSpan();
                        }
                    }
                }
            }
            if (createState == ECreatState.EditFileNameSW)
            {
                KeyboardState keyboardState = Keyboard.GetState();
                if (timeSpace > Config.TIME_BETWEEN_SAME_EVENT)
                {
                    if (keyboardState.IsKeyDown(Keys.Back) && FileNameSW.Length > 0)
                    {
                        FileNameSW = FileNameSW.Substring(0, FileNameSW.Length - 1);
                        timeSpace = new TimeSpan();
                    }
                    else
                    {
                        string input = Helper.KeyToChar(keyboardState, gameTime);
                        if (!input.Equals(""))
                        {
                            FileNameSW += input;
                            timeSpace = new TimeSpan();
                        }
                    }
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
            spriteBatch.Begin();
            spriteBatch.Draw(bgPic, new Vector2(0, 0), Color.White);
            spriteBatch.End();

            switch (mainState)
            {
                case EMainState.LoadNewMap:
                    DrawLoadNewMap(gameTime, spriteBatch);
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

            Vector2 pos = new Vector2(MAINMENUBUTTONLEFTBOUND, Config.BIG_BUTTON_SPACE);
            textArrayPos = new Vector2(pos.X, pos.Y);

            spriteBatch.Draw(whitePixel, new Rectangle((int)textArrayPos.X - Config.TEXTFIELD_BORDER, (int)textArrayPos.Y - Config.TEXTFIELD_BORDER, 800 - (int)textArrayPos.X, /*2 * Config.TEXTFIELD_BORDER*/ + NUM_ENTRIES * (Config.LINE_SIZE + Config.BIG_LINE_SPACE) - Config.BIG_LINE_SPACE), Config.TEXT_BOX_COLOR);

            if (directoryarray != null)
            {
                for (int count = scrollpos; (count < scrollpos + NUM_ENTRIES) && (count < directoryarray.Length); count++)
                {
                    pos.Y = (count - scrollpos) * (Config.LINE_SIZE + Config.BIG_LINE_SPACE) + Config.SMALL_BUTTON_SPACE;
                    spriteBatch.DrawString(DEFAULT_FONT, directoryarray[count].Substring(directoryarray[count].LastIndexOf('\\') + 1), pos, Config.TEXT_COLOR);
                }
            }
            pos.Y = NUM_ENTRIES * (Config.LINE_SIZE + Config.BIG_LINE_SPACE) + 3 * Config.SMALL_BUTTON_SPACE + (int)Config.SMALL_BUTTON.X;
            spriteBatch.Draw(LoadNewMap, pos, Color.White);
            LoadMapPos = new Vector2(pos.X, pos.Y);

            pos.Y += Config.BIG_BUTTON_SPACE + (int)Config.BIG_BUTTON.Y;
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
                scal = (float)MAPSIZE / (float)max;

                x = (int)(x * scal);
                y = (int)(y * scal);

                spriteBatch.Draw(MapPic, new Rectangle(MAPLEFTBOUND, MAPLEFTBOUND, x, y), Color.White);
            }
            else
            {
                spriteBatch.Draw(whitePixel, new Rectangle(MAPLEFTBOUND, MAPLEFTBOUND, MAPSIZE, MAPSIZE), Config.TEXT_BOX_COLOR);
            }

            MapPicPos = new Vector2(MAPLEFTBOUND);

            //Text
            Vector2 pos = new Vector2(CREATEMENUBUTTONLEFTBOUND, Config.SMALL_BUTTON_SPACE);

            spriteBatch.Draw(whitePixel, new Rectangle((int)pos.X - Config.TEXTFIELD_BORDER, (int)pos.Y - Config.TEXTFIELD_BORDER, 800 - (int)pos.X, 2 * Config.LINE_SIZE + Config.BIG_LINE_SPACE + 2 * Config.TEXTFIELD_BORDER), Config.TEXT_BOX_COLOR);

            spriteBatch.DrawString(DEFAULT_FONT, "Color-File: " + fileName, pos, Config.TEXT_COLOR);
            fileNamePos = new Vector2(pos.X, pos.Y);

            pos.Y += Config.LINE_SIZE + Config.BIG_LINE_SPACE;
            spriteBatch.DrawString(DEFAULT_FONT, "SW-File: " + FileNameSW, pos, Config.TEXT_COLOR);
            fileNameSWPos = new Vector2(pos.X, pos.Y);

            //Buttons
            pos.Y += Config.LINE_SIZE + Config.BIG_LINE_SPACE + Config.TEXTFIELD_BORDER;
            spriteBatch.Draw(LoadMap, pos, Color.White);
            LoadMapPos = new Vector2(pos.X, pos.Y);

            pos.Y += (int)Config.SMALL_BUTTON.Y + Config.SMALL_BUTTON_SPACE;
            spriteBatch.Draw(CheckPoint, pos, Color.White);
            CheckPointPos = new Vector2(pos.X, pos.Y);

            remCheckPointPos = new Vector2(pos.X, pos.Y);
            remCheckPointPos.X += (int)Config.SMALL_BUTTON.X + Config.SMALL_BUTTON_SPACE;
            spriteBatch.Draw(remCheckPoint, remCheckPointPos, Color.White);

            pos.Y += (int)Config.SMALL_BUTTON.Y + Config.SMALL_BUTTON_SPACE;
            spriteBatch.Draw(Start, pos, Color.White);
            StartPos = new Vector2(pos.X, pos.Y);

            pos.Y += (int)Config.SMALL_BUTTON.Y + Config.SMALL_BUTTON_SPACE;
            spriteBatch.Draw(Finish, pos, Color.White);
            FinishPos = new Vector2(pos.X, pos.Y);

            pos.Y += (int)Config.SMALL_BUTTON.Y + Config.SMALL_BUTTON_SPACE + Config.TEXTFIELD_BORDER;
            spriteBatch.Draw(whitePixel, new Rectangle((int)pos.X - Config.TEXTFIELD_BORDER, (int)pos.Y - Config.TEXTFIELD_BORDER, 800 - (int)pos.X, 480 - (int)pos.Y), Config.TEXT_BOX_COLOR);
            spriteBatch.DrawString(DEFAULT_FONT, Info(), pos, Config.TEXT_COLOR);

            CreatePos = new Vector2(MAPLEFTBOUND, 2 * MAPLEFTBOUND + MAPSIZE);
            spriteBatch.Draw(Create, CreatePos, Color.White);

            BackPos = new Vector2(MAPLEFTBOUND + (int)Config.SMALL_BUTTON.X + Config.SMALL_BUTTON_SPACE, CreatePos.Y);
            spriteBatch.Draw(Back, new Rectangle((int)BackPos.X, (int)BackPos.Y, (int)Config.SMALL_BUTTON.X, (int)Config.SMALL_BUTTON.Y), Color.White);

            spriteBatch.End();
        }

        private string Info()
        {
            switch (createState)
            {
                case ECreatState.Nothing:
                    return "Load a Map and its black-and-\nwhite-dublicate:\n" +
                            "white   - road\n" +
                            "greay   - offroad\n" +
                            "black   - object";
                case ECreatState.EditFileNameColor:
                    return "Enter the name of the color-\npicture.";
                case ECreatState.EditFileNameSW:
                    return "Enter the name of the black-and-\nwhite-picture:\n" +
                            "white   - road\n" +
                            "greay   - offroad\n" +
                            "black   - object";
                case ECreatState.SetCheckPoint_I:
                    return "Set a checkpoint:\n" +
                            "A checkpoint are two points on both \nsides of the road.\n" +
                            "Click in the map to set the first \none.";
                case ECreatState.SetCheckPoint_II:
                    return "Set a checkpoint:\n" +
                            "A checkpoint are two points on both \nsides of the road.\n" +
                            "Click in the map to set the second \none.";
                case ECreatState.SetStartPoint_I:
                    return "Click in the map to set the point, \nwere the cars start.";
                case ECreatState.SetStartPoint_II:
                    return "Click in the map to set the direction, \nthe cars drive in.";
                case ECreatState.SetFinish_I:
                    return "The finish is defind by two points \non both sides of the road:\n" +
                            "Click in the map to set the first \none.";
                case ECreatState.SetFinish_II:
                    return "The finish is defind by two points \non both sides of the road:\n" +
                            "Click in the map to set the second \none.";
                case ECreatState.Error:
                    return "ERROR\n";
                default:
                    return "I can't help you!";
            }
        }

        /// <summary>
        /// Unload
        /// </summary>
        public void Unload()
        { }


        public EGameStates getGameState()
        {
            return EGameStates.LoadMenu;
        }
    }
}
