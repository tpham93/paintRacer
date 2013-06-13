using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace paintRacer
{
    class Multiplayer : IGameStateElements
    {

        public void Load(ContentManager content)
        {

        }

        public EGameStates Update(GameTime gameTime)
        {
            return EGameStates.Menue;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

        }

        public void Unload()
        {

        }
    }
}
