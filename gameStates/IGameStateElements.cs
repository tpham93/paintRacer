using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace paintRacer
{
    interface IGameStateElements
    {
        void Load(ContentManager content);
        EGameStates Update(GameTime gameTime);
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        void Unload();
    }
}
