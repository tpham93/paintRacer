using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace paintRacer.gameStates
{
    class Credits : IGameStateElements
    {

        private Texture2D bgPic;

        public void Load(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            bgPic = Helper.loadImage("Content/credits.png");
        }

        public EGameStates Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            return EGameStates.Credits;
        }

        public void Draw(Microsoft.Xna.Framework.GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            if (bgPic != null)
                spriteBatch.Draw(bgPic, new Vector2(0, 0), Color.White);
            spriteBatch.End();
        }

        public void Unload()
        {
        }
    }
}
