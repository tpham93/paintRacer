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

        public static string KeyToChar(KeyboardState keyboardState)
        {
            //A
            if (keyboardState.IsKeyDown(Keys.A))
            {
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "A";
                else
                    return "a";
            }

            //B
            else if (keyboardState.IsKeyDown(Keys.B))
            {
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "B";
                else
                    return "b";
            }

            //C
            else if (keyboardState.IsKeyDown(Keys.C))
            {
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "C";
                else
                    return "c";
            }

            //D
            else if (keyboardState.IsKeyDown(Keys.D))
            {
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "D";
                else
                    return "d";
            }

            //E
            else if (keyboardState.IsKeyDown(Keys.E))
            {
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "E";
                else
                    return "e";
            }

            //F
            else if (keyboardState.IsKeyDown(Keys.F))
            {
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "F";
                else
                    return "f";
            }

            //G
            else if (keyboardState.IsKeyDown(Keys.G))
            {
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "G";
                else
                    return "g";
            }

            //H
            else if (keyboardState.IsKeyDown(Keys.H))
            {
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "H";
                else
                    return "h";
            }

            //I
            else if (keyboardState.IsKeyDown(Keys.I))
            {
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "I";
                else
                    return "i";
            }

            //J
            else if (keyboardState.IsKeyDown(Keys.J))
            {
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "J";
                else
                    return "j";
            }

            //K
            else if (keyboardState.IsKeyDown(Keys.K))
            {
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "K";
                else
                    return "k";
            }

            //L
            else if (keyboardState.IsKeyDown(Keys.L))
            {
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "L";
                else
                    return "l";
            }

            //M
            else if (keyboardState.IsKeyDown(Keys.M))
            {
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "M";
                else
                    return "m";
            }

            //N
            else if (keyboardState.IsKeyDown(Keys.N))
            {
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "N";
                else
                    return "n";
            }

            //O
            else if (keyboardState.IsKeyDown(Keys.O))
            {
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "O";
                else
                    return "o";
            }

            //P
            else if (keyboardState.IsKeyDown(Keys.P))
            {
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "P";
                else
                    return "p";
            }

            //Q
            else if (keyboardState.IsKeyDown(Keys.Q))
            {
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "Q";
                else
                    return "q";
            }

            //R
            else if (keyboardState.IsKeyDown(Keys.R))
            {
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "R";
                else
                    return "r";
            }

            //S
            else if (keyboardState.IsKeyDown(Keys.S))
            {
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "S";
                else
                    return "s";
            }

            //T
            else if (keyboardState.IsKeyDown(Keys.T))
            {
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "T";
                else
                    return "t";
            }

            //B
            else if (keyboardState.IsKeyDown(Keys.B))
            {
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "B";
                else
                    return "b";
            }

            //U
            else if (keyboardState.IsKeyDown(Keys.U))
            {
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "U";
                else
                    return "u";
            }

            //V
            else if (keyboardState.IsKeyDown(Keys.V))
            {
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "V";
                else
                    return "v";
            }

            //W
            else if (keyboardState.IsKeyDown(Keys.W))
            {
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "W";
                else
                    return "w";
            }

            //X
            else if (keyboardState.IsKeyDown(Keys.X))
            {
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "X";
                else
                    return "x";
            }

            //Y
            else if (keyboardState.IsKeyDown(Keys.Y))
            {
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "Y";
                else
                    return "z";
            }

            //Z
            else if (keyboardState.IsKeyDown(Keys.Z))
            {
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "Z";
                else
                    return "z";
            }

            //0
            else if (keyboardState.IsKeyDown(Keys.D0) || keyboardState.IsKeyDown(Keys.NumPad0))
            {
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "=";
                else
                    return "0";
            }

            //1
            else if (keyboardState.IsKeyDown(Keys.D1) || keyboardState.IsKeyDown(Keys.NumPad1))
            {
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "!";
                else
                    return "1";
            }

            //2
            else if (keyboardState.IsKeyDown(Keys.D2) || keyboardState.IsKeyDown(Keys.NumPad2))
            {
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "\"";
                else
                    return "2";
            }

            //3
            else if (keyboardState.IsKeyDown(Keys.D3) || keyboardState.IsKeyDown(Keys.NumPad3))
            {
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "§";
                else
                    return "3";
            }

            //4
            else if (keyboardState.IsKeyDown(Keys.D4) || keyboardState.IsKeyDown(Keys.NumPad4))
            {
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "$";
                else
                    return "4";
            }

            //5
            else if (keyboardState.IsKeyDown(Keys.D5) || keyboardState.IsKeyDown(Keys.NumPad5))
            {
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "%";
                else
                    return "5";
            }

            //6
            else if (keyboardState.IsKeyDown(Keys.D6) || keyboardState.IsKeyDown(Keys.NumPad6))
            {
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "&";
                else
                    return "6";
            }

            //7
            else if (keyboardState.IsKeyDown(Keys.D7) || keyboardState.IsKeyDown(Keys.NumPad7))
            {
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "/";
                else
                    return "7";
            }

            //8
            else if (keyboardState.IsKeyDown(Keys.D8) || keyboardState.IsKeyDown(Keys.NumPad8))
            {
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "(";
                else
                    return "8";
            }

            //9
            else if (keyboardState.IsKeyDown(Keys.D9) || keyboardState.IsKeyDown(Keys.NumPad9))
            {
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return ")";
                else
                    return "9";
            }

            //+
            else if (keyboardState.IsKeyDown(Keys.Add))
            {
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift) || keyboardState.IsKeyDown(Keys.CapsLock))
                    return "*";
                else if (keyboardState.IsKeyDown(Keys.LeftAlt) || keyboardState.IsKeyDown(Keys.RightAlt))
                    return "~";
                else
                    return "+";
            }

            ///TODO: continue
            
            else 
            {
                return "";
            }
        }
    }
}
