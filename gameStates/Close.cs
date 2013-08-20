using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace paintRacer.gameStates
{
    class Close : IGameStateElements
    {
        public void Load(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            Environment.Exit(0);
        }

        public EGameStates Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Environment.Exit(0);
            return EGameStates.Nothing;
        }

        public void Draw(Microsoft.Xna.Framework.GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            Environment.Exit(0);
        }

        public void Unload()
        {
            Environment.Exit(0);
        }
    }
}
