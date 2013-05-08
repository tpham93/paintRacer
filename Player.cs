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
        //In radian
        private float rotation;
        private Vector2 speed;

        private Texture2D texture;
        //Going to be used to identify player in splitscreen/multiplayer play
        private Color color;


        //public Player(String filename, Color color)
        //public static void Init()
        //public void Update(GameTime gameTime)
        //public void Draw(SpriteBatch spriteBatch)

    }
}
