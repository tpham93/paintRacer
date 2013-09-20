using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace paintRacer
{
    class Helper
    {
        private static GraphicsDevice graphicsDevice;

        private static Keys lastKey;
        public static TimeSpan timeSpan;
        public static TimeSpan TIMEBETWEENKEYS = new TimeSpan(0, 0, 0, 0, 200);

        // set graphicsDevice
        public static void Init(GraphicsDevice graphicsDevice)
        {
            Helper.graphicsDevice = graphicsDevice;
        }


        public static Texture2D loadImage(String filename,Rectangle rect = new Rectangle(), Color color =new Color())
        {
            //Might throw FileNotFoundException
            FileStream fileStream = new FileStream(filename, FileMode.Open);
            Texture2D texture = Texture2D.FromStream(graphicsDevice, fileStream);
            fileStream.Close();
            // returns original car if the rectangle has no size
            if (rect.Width <= 0 || rect.Height <= 0)
            {
                return texture;
            }
            else
            {
                // rendertarget to save the resized image
                RenderTarget2D renderTarget = new RenderTarget2D(graphicsDevice,rect.Width,rect.Height,false,SurfaceFormat.Color,DepthFormat.Depth24Stencil8);
                // create  spriteBatch to draw the resized image
                SpriteBatch spriteBatch = new SpriteBatch(graphicsDevice);
                // save the earlier rendertargets
                RenderTargetBinding[] tmpRenderTargets = graphicsDevice.GetRenderTargets();
                graphicsDevice.SetRenderTarget(renderTarget);
                // draw the resized image on the rendertarget
                graphicsDevice.Clear(Color.Transparent);
                spriteBatch.Begin();
                if (color.Equals(new Color()))
                {
                    spriteBatch.Draw(texture, rect, Color.White);
                }
                else
                {
                    spriteBatch.Draw(texture, rect, color);
                }
                spriteBatch.End();
                // set back the saved rendertargets
                graphicsDevice.SetRenderTargets(tmpRenderTargets);
                // return the resized drawn Iimage
                return renderTarget;
            }
        }

        // rotates vector
        public static Vector2 rotateVector2(Vector2 vector, float rotation)
        {
            // multplicate vector with rotationmatrix
            // (cos(a) -sin(a))
            // (sin(a)  cos(a))
            return rotateVector2(vector, Math.Cos(rotation), Math.Sin(rotation));
        }
        // rotates vector
        public static Vector2 rotateVector2(Vector2 vector, double rotationCos, double rotationSin)
        {
            // multplicate vector with rotationmatrix
            // (cos(a) -sin(a))
            // (sin(a)  cos(a))
            return new Vector2((int)(0.5f + vector.X * rotationCos + vector.Y * -rotationSin), (int)(0.5f + vector.X * rotationSin + vector.Y * rotationCos));
        }

        //Creates playerCount amounts of viewports (appropriately stretched and positioned). Will throw Exceptions in case the user request 0 or less/3 or more viewports
        public static Viewport[] createViewports(int playerCount, Viewport defaultView)
        {
            Viewport[] viewports = new Viewport[playerCount];
            if(playerCount<=0)
            {
                throw new Exception("Couldn't create viewports, playerCount is smaller than (or equal to) 0.");
            }
            else if (playerCount == 1)
            {
                viewports[0] = defaultView;
            }
            else if (playerCount == 2)
            {
                viewports[0] = defaultView;
                viewports[1] = defaultView;
                viewports[0].Width /= 2;
                viewports[1].Width /= 2;
                viewports[1].X = viewports[0].Width;
            }
            else
            {
                throw new Exception("Couldn't create viewports, playerCount is too big (bigger than 2).");
            }
            return viewports;
        }

        public static string KeyToChar(KeyboardState keyboardState, GameTime gameTime)
        {
            timeSpan += gameTime.ElapsedGameTime;
            //A
            if (keyboardState.IsKeyDown(Keys.A) && !(lastKey == Keys.A && timeSpan < TIMEBETWEENKEYS))
            {
                lastKey = Keys.A;
                timeSpan = new TimeSpan();
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "A";
                else
                    return "a";
            }

            //B
            else if (keyboardState.IsKeyDown(Keys.B) && !(lastKey == Keys.B && timeSpan < TIMEBETWEENKEYS))
            {
                lastKey = Keys.B;
                timeSpan = new TimeSpan();
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "B";
                else
                    return "b";
            }

            //C
            else if (keyboardState.IsKeyDown(Keys.C) && !(lastKey == Keys.C && timeSpan < TIMEBETWEENKEYS))
            {
                lastKey = Keys.C;
                timeSpan = new TimeSpan();
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "C";
                else
                    return "c";
            }

            //D
            else if (keyboardState.IsKeyDown(Keys.D) && !(lastKey == Keys.D && timeSpan < TIMEBETWEENKEYS))
            {
                lastKey = Keys.D;
                timeSpan = new TimeSpan();
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "D";
                else
                    return "d";
            }

            //E
            else if (keyboardState.IsKeyDown(Keys.E) && !(lastKey == Keys.E && timeSpan < TIMEBETWEENKEYS))
            {
                lastKey = Keys.E;
                timeSpan = new TimeSpan();
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "E";
                else
                    return "e";
            }

            //F
            else if (keyboardState.IsKeyDown(Keys.F) && !(lastKey == Keys.F && timeSpan < TIMEBETWEENKEYS))
            {
                lastKey = Keys.F;
                timeSpan = new TimeSpan();
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "F";
                else
                    return "f";
            }

            //G
            else if (keyboardState.IsKeyDown(Keys.G) && !(lastKey == Keys.G && timeSpan < TIMEBETWEENKEYS))
            {
                lastKey = Keys.G;
                timeSpan = new TimeSpan();
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "G";
                else
                    return "g";
            }

            //H
            else if (keyboardState.IsKeyDown(Keys.H) && !(lastKey == Keys.H && timeSpan < TIMEBETWEENKEYS))
            {
                lastKey = Keys.H;
                timeSpan = new TimeSpan();
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "H";
                else
                    return "h";
            }

            //I
            else if (keyboardState.IsKeyDown(Keys.I) && !(lastKey == Keys.I && timeSpan < TIMEBETWEENKEYS))
            {
                lastKey = Keys.I;
                timeSpan = new TimeSpan();
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "I";
                else
                    return "i";
            }

            //J
            else if (keyboardState.IsKeyDown(Keys.J) && !(lastKey == Keys.J && timeSpan < TIMEBETWEENKEYS))
            {
                lastKey = Keys.J;
                timeSpan = new TimeSpan();
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "J";
                else
                    return "j";
            }

            //K
            else if (keyboardState.IsKeyDown(Keys.K) && !(lastKey == Keys.K && timeSpan < TIMEBETWEENKEYS))
            {
                lastKey = Keys.K;
                timeSpan = new TimeSpan();
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "K";
                else
                    return "k";
            }

            //L
            else if (keyboardState.IsKeyDown(Keys.L) && !(lastKey == Keys.L && timeSpan < TIMEBETWEENKEYS))
            {
                lastKey = Keys.L;
                timeSpan = new TimeSpan();
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "L";
                else
                    return "l";
            }

            //M
            else if (keyboardState.IsKeyDown(Keys.M) && !(lastKey == Keys.M && timeSpan < TIMEBETWEENKEYS))
            {
                lastKey = Keys.M;
                timeSpan = new TimeSpan();
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "M";
                else
                    return "m";
            }

            //N
            else if (keyboardState.IsKeyDown(Keys.N) && !(lastKey == Keys.N && timeSpan < TIMEBETWEENKEYS))
            {
                lastKey = Keys.N;
                timeSpan = new TimeSpan();
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "N";
                else
                    return "n";
            }

            //O
            else if (keyboardState.IsKeyDown(Keys.O) && !(lastKey == Keys.O && timeSpan < TIMEBETWEENKEYS))
            {
                lastKey = Keys.O;
                timeSpan = new TimeSpan();
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "O";
                else
                    return "o";
            }

            //P
            else if (keyboardState.IsKeyDown(Keys.P) && !(lastKey == Keys.P && timeSpan < TIMEBETWEENKEYS))
            {
                lastKey = Keys.P;
                timeSpan = new TimeSpan();
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "P";
                else
                    return "p";
            }

            //Q
            else if (keyboardState.IsKeyDown(Keys.Q) && !(lastKey == Keys.Q && timeSpan < TIMEBETWEENKEYS))
            {
                lastKey = Keys.Q;
                timeSpan = new TimeSpan();
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "Q";
                else
                    return "q";
            }

            //R
            else if (keyboardState.IsKeyDown(Keys.R) && !(lastKey == Keys.R && timeSpan < TIMEBETWEENKEYS))
            {
                lastKey = Keys.R;
                timeSpan = new TimeSpan();
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "R";
                else
                    return "r";
            }

            //S
            else if (keyboardState.IsKeyDown(Keys.S) && !(lastKey == Keys.S && timeSpan < TIMEBETWEENKEYS))
            {
                lastKey = Keys.S;
                timeSpan = new TimeSpan();
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "S";
                else
                    return "s";
            }

            //T
            else if (keyboardState.IsKeyDown(Keys.T) && !(lastKey == Keys.T && timeSpan < TIMEBETWEENKEYS))
            {
                lastKey = Keys.T;
                timeSpan = new TimeSpan();
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "T";
                else
                    return "t";
            }

            //B
            else if (keyboardState.IsKeyDown(Keys.B) && !(lastKey == Keys.B && timeSpan < TIMEBETWEENKEYS))
            {
                lastKey = Keys.B;
                timeSpan = new TimeSpan();
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "B";
                else
                    return "b";
            }

            //U
            else if (keyboardState.IsKeyDown(Keys.U) && !(lastKey == Keys.U && timeSpan < TIMEBETWEENKEYS))
            {
                lastKey = Keys.U;
                timeSpan = new TimeSpan();
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "U";
                else
                    return "u";
            }

            //V
            else if (keyboardState.IsKeyDown(Keys.V) && !(lastKey == Keys.V && timeSpan < TIMEBETWEENKEYS))
            {
                lastKey = Keys.V;
                timeSpan = new TimeSpan();
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "V";
                else
                    return "v";
            }

            //W
            else if (keyboardState.IsKeyDown(Keys.W) && !(lastKey == Keys.W && timeSpan < TIMEBETWEENKEYS))
            {
                lastKey = Keys.W;
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "W";
                else
                    return "w";
            }

            //X
            else if (keyboardState.IsKeyDown(Keys.X) && !(lastKey == Keys.X && timeSpan < TIMEBETWEENKEYS))
            {
                lastKey = Keys.X;
                timeSpan = new TimeSpan();
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "X";
                else
                    return "x";
            }

            //Y
            else if (keyboardState.IsKeyDown(Keys.Y) && !(lastKey == Keys.Y && timeSpan < TIMEBETWEENKEYS))
            {
                lastKey = Keys.Y;
                timeSpan = new TimeSpan();
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "Y";
                else
                    return "z";
            }

            //Z
            else if (keyboardState.IsKeyDown(Keys.Z) && !(lastKey == Keys.Z && timeSpan < TIMEBETWEENKEYS))
            {
                lastKey = Keys.Z;
                timeSpan = new TimeSpan();
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "Z";
                else
                    return "z";
            }

            //0
            else if (keyboardState.IsKeyDown(Keys.D0) && !(lastKey == Keys.D0 && timeSpan < TIMEBETWEENKEYS))
            {
                lastKey = Keys.D0;
                timeSpan = new TimeSpan();
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "=";
                else if (keyboardState.IsKeyDown(Keys.LeftAlt) || keyboardState.IsKeyDown(Keys.RightAlt))
                    return "}";
                else
                    return "0";
            }
            //0
            else if (keyboardState.IsKeyDown(Keys.NumPad0) && !(lastKey == Keys.NumPad0 && timeSpan < TIMEBETWEENKEYS))
            {
                lastKey = Keys.NumPad0;
                timeSpan = new TimeSpan();
                return "0";
            }

            //1
            else if (keyboardState.IsKeyDown(Keys.D1) && !(lastKey == Keys.D1 && timeSpan < TIMEBETWEENKEYS))
            {
                lastKey = Keys.D1;
                timeSpan = new TimeSpan();
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "!";
                else
                    return "1";
            }
            //1
            else if (keyboardState.IsKeyDown(Keys.NumPad0) && !(lastKey == Keys.NumPad1 && timeSpan < TIMEBETWEENKEYS))
            {
                lastKey = Keys.NumPad1;
                timeSpan = new TimeSpan();
                return "1";
            }

            //2
            else if (keyboardState.IsKeyDown(Keys.D2) && !(lastKey == Keys.D2 && timeSpan < TIMEBETWEENKEYS))
            {
                lastKey = Keys.D2;
                timeSpan = new TimeSpan();
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "\"";
                else
                    return "2";
            }
            //2
            else if (keyboardState.IsKeyDown(Keys.NumPad0) && !(lastKey == Keys.NumPad2 && timeSpan < TIMEBETWEENKEYS))
            {
                lastKey = Keys.NumPad2;
                timeSpan = new TimeSpan();
                return "2";
            }

            //3
            else if (keyboardState.IsKeyDown(Keys.D3) && !(lastKey == Keys.D3 && timeSpan < TIMEBETWEENKEYS))
            {
                lastKey = Keys.D3;
                timeSpan = new TimeSpan();
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "§";
                else
                    return "3";
            }
            //3
            else if (keyboardState.IsKeyDown(Keys.NumPad3) && !(lastKey == Keys.NumPad3 && timeSpan < TIMEBETWEENKEYS))
            {
                lastKey = Keys.NumPad3;
                return "3";
            }

            //4
            else if (keyboardState.IsKeyDown(Keys.D4) && !(lastKey == Keys.D4 && timeSpan < TIMEBETWEENKEYS))
            {
                lastKey = Keys.D4;
                timeSpan = new TimeSpan();
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "$";
                else
                    return "4";
            }
            //4
            else if (keyboardState.IsKeyDown(Keys.NumPad4) && !(lastKey == Keys.NumPad4 && timeSpan < TIMEBETWEENKEYS))
            {
                lastKey = Keys.NumPad4;
                timeSpan = new TimeSpan();
                return "4";
            }

            //5
            else if (keyboardState.IsKeyDown(Keys.D5) && !(lastKey == Keys.D5 && timeSpan < TIMEBETWEENKEYS))
            {
                lastKey = Keys.D5;
                timeSpan = new TimeSpan();
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "%";
                else
                    return "5";
            }
            //5
            else if (keyboardState.IsKeyDown(Keys.NumPad5) && !(lastKey == Keys.NumPad5 && timeSpan < TIMEBETWEENKEYS))
            {
                lastKey = Keys.NumPad5;
                timeSpan = new TimeSpan();
                return "5";
            }

            //6
            else if (keyboardState.IsKeyDown(Keys.D6) && !(lastKey == Keys.D6 && timeSpan < TIMEBETWEENKEYS))
            {
                lastKey = Keys.D6;
                timeSpan = new TimeSpan();
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "&";
                else
                    return "6";
            }
            //6
            else if (keyboardState.IsKeyDown(Keys.NumPad6) && !(lastKey == Keys.NumPad6 && timeSpan < TIMEBETWEENKEYS))
            {
                lastKey = Keys.NumPad6;
                timeSpan = new TimeSpan();
                return "6";
            }

            //7
            else if (keyboardState.IsKeyDown(Keys.D7) && !(lastKey == Keys.D7 && timeSpan < TIMEBETWEENKEYS))
            {
                lastKey = Keys.D7;
                timeSpan = new TimeSpan();
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "/";
                else if (keyboardState.IsKeyDown(Keys.LeftAlt) || keyboardState.IsKeyDown(Keys.RightAlt))
                    return "{";
                else
                    return "7";
            }
            //7
            else if (keyboardState.IsKeyDown(Keys.NumPad7) && !(lastKey == Keys.NumPad7 && timeSpan < TIMEBETWEENKEYS))
            {
                lastKey = Keys.NumPad7;
                timeSpan = new TimeSpan();
                return "7";
            }

            //8
            else if (keyboardState.IsKeyDown(Keys.D8) && !(lastKey == Keys.D8 && timeSpan < TIMEBETWEENKEYS))
            {
                lastKey = Keys.D8;
                timeSpan = new TimeSpan();
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "(";
                else if (keyboardState.IsKeyDown(Keys.LeftAlt) || keyboardState.IsKeyDown(Keys.RightAlt))
                    return "[";
                else
                    return "8";
            }
            //8
            else if (keyboardState.IsKeyDown(Keys.NumPad8) && !(lastKey == Keys.NumPad8 && timeSpan < TIMEBETWEENKEYS))
            {
                lastKey = Keys.NumPad8;
                timeSpan = new TimeSpan();
                return "8";
            }

            //9
            else if (keyboardState.IsKeyDown(Keys.D9) && !(lastKey == Keys.D9 && timeSpan < TIMEBETWEENKEYS))
            {
                lastKey = Keys.D9;
                timeSpan = new TimeSpan();
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return ")";
                else if (keyboardState.IsKeyDown(Keys.LeftAlt) || keyboardState.IsKeyDown(Keys.RightAlt))
                    return "]";
                else
                    return "9";
            }
            //9
            else if (keyboardState.IsKeyDown(Keys.NumPad9) && !(lastKey == Keys.NumPad9 && timeSpan < TIMEBETWEENKEYS))
            {
                lastKey = Keys.NumPad9;
                timeSpan = new TimeSpan();
                return "9";
            }

            //,
            else if (keyboardState.IsKeyDown(Keys.OemComma) && !(lastKey == Keys.OemComma && timeSpan < TIMEBETWEENKEYS))
            {
                lastKey = Keys.OemComma;
                timeSpan = new TimeSpan();
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return ";";
                else
                    return ",";
            }

            //<
            else if (keyboardState.IsKeyDown(Keys.OemBackslash) && !(lastKey == Keys.OemComma && timeSpan < TIMEBETWEENKEYS))
            {
                lastKey = Keys.OemComma;
                timeSpan = new TimeSpan();
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return ">";
                else if (keyboardState.IsKeyDown(Keys.LeftAlt) || keyboardState.IsKeyDown(Keys.RightAlt))
                    return "|";
                else
                    return "<";
            }

            //-
            else if (keyboardState.IsKeyDown(Keys.OemMinus) && !(lastKey == Keys.OemMinus && timeSpan < TIMEBETWEENKEYS))
            {
                lastKey = Keys.OemMinus;
                timeSpan = new TimeSpan();
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "_";
                else
                    return "-";
            }

            //+
            else if (keyboardState.IsKeyDown(Keys.OemPlus) && !(lastKey == Keys.OemPlus && timeSpan < TIMEBETWEENKEYS))
            {
                lastKey = Keys.OemPlus;
                timeSpan = new TimeSpan();
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "*";
                else if (keyboardState.IsKeyDown(Keys.LeftAlt) || keyboardState.IsKeyDown(Keys.RightAlt))
                    return "~";
                else
                    return "+";
            }

            //#
            else if (keyboardState.IsKeyDown(Keys.OemQuestion) && !(lastKey == Keys.OemQuestion && timeSpan < TIMEBETWEENKEYS))
            {
                lastKey = Keys.OemQuestion;
                timeSpan = new TimeSpan();
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "'";
                else
                    return "#";
            }

            //+
            else if (keyboardState.IsKeyDown(Keys.OemPipe) && !(lastKey == Keys.OemPipe && timeSpan < TIMEBETWEENKEYS))
            {
                lastKey = Keys.OemPipe;
                timeSpan = new TimeSpan();
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "°";
                else
                    return "^";
            }

            //ß
            else if (keyboardState.IsKeyDown(Keys.OemOpenBrackets) && !(lastKey == Keys.OemOpenBrackets && timeSpan < TIMEBETWEENKEYS))
            {
                lastKey = Keys.OemOpenBrackets;
                timeSpan = new TimeSpan();
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "?";
                else if (keyboardState.IsKeyDown(Keys.LeftAlt) || keyboardState.IsKeyDown(Keys.RightAlt))
                    return "\\";
                else
                    return "sz";
            }

            //.
            else if (keyboardState.IsKeyDown(Keys.OemPeriod) && !(lastKey == Keys.OemPeriod && timeSpan < TIMEBETWEENKEYS))
            {
                lastKey = Keys.OemPeriod;
                timeSpan = new TimeSpan();
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return ":";
                else
                    return ".";
            }


            ///TODO: continue
            
            else 
            {
                return "";
            }
        }

        public static Vector2[] resizeV2Array(Vector2[] array, int n)
        {
            if (array != null)
            {
                if (array.Length <= -n) return null;
                Vector2[] result = new Vector2[array.Length + n];
                for (int i = 0; i < result.Length && i < array.Length; i++)
                {
                    result[i] = new Vector2(array[i].X, array[i].Y);
                }
                return result;
            }
            return null;
        }

        public static Texture2D genRectangleTexture(int width, int height,Color color)
        {
            Texture2D output = new Texture2D(graphicsDevice, width, height);

            Color[] pixel = new Color[width * height];
            for (int i = 0; i < pixel.Length; ++i)
            {
                pixel[i] = color;
            }
            output.SetData<Color>(pixel);
            return output;
        }
    }
}
