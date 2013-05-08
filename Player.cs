using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace paintRacer
{
    class Player
    {
        private bool[,] collisiondata;
        private EventManager eventManager;
        private Vector2 position;
        private float rotation;
        private float speed;
        private Texture2D texture;
        private Vector2 velocity;
        private Color color;


        public Player(String filename, Color color);
        public static void Init();
        public void Update(GameTime gameTime);
        public void Draw(SpriteBatch spriteBatch);

    }
}
