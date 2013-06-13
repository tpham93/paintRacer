using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace paintRacer.gameStates
{
    class Singleplayer : IGameStateElements
    {

        public void Load(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            
        }

        public EGameStates Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            return EGameStates.Menue;
        }

        public void Draw(Microsoft.Xna.Framework.GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            
        }

        public void Unload()
        {
            
        }
    }
}
