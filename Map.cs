using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace paintRacer
{
    class Map
    {
        Texture2D image;
        EMapStates[,] data;
        Vector2[] checkPoints;
        Vector2 start;
        float startRotation;

        public EMapStates[,] Data
        {
            get { return data; }
        }
        public Vector2[] CheckPoints
        {
            get { return checkPoints; }
        }
        public Vector2 Start
        {
            get { return start; }
        }
        public float StartRotation
        {
            get { return startRotation; }
        }


        /// <summary>
        /// Struct to save maps (constructor for the prototype)
        /// </summary>
        /// 
        /// <param name="swImagePath">
        /// the location of the black white picture
        /// </param>
        /// 
        /// <param name="imagePath">
        /// the location of the picture
        /// </param>
        /// 
        public Map(string swImagePath, string imagePath)
        {
            this.image = Helper.loadImage(imagePath);
            this.data = MapReader.createDataFromSWImage(Helper.loadImage(swImagePath));
            checkPoints = new Vector2[0];
            start = new Vector2(1535, 550);
            startRotation = 0;
        }


        /// <summary>
        /// Struct to save maps
        /// </summary>
        /// 
        /// <param name="imageFile">
        /// the location of the picture
        /// </param>
        /// 
        /// <param name="mapData">
        /// the data of the map, def. were road, offroad and objects are
        /// </param>
        /// 
        /// <param name="checkPoints">
        /// the points whitch mark the checkpoints
        /// size%2 must be 0
        /// last tupel is finish
        /// </param>
        /// 
        /// <param name="startPoint">
        /// the midpoint of the cars in startposition
        /// </param>
        /// 
        /// <param name="startRotation">
        /// the rotation the cars start with
        /// </param>
        /// 
        public Map(Texture2D image, EMapStates[,] mapData, Vector2[] checkPoints, Vector2 startPoint, float startRotation)
        {
            this.image = image;
            this.data = mapData;
            this.checkPoints = checkPoints;
            this.start = startPoint;
            this.startRotation = startRotation;
        }

        ///// <summary>
        ///// getter for the map-width
        ///// </summary>
        ///// 
        ///// <returns>
        ///// the width of the map
        ///// </returns>
        //public int getWidth()
        //{
        //    return this.image.Width;
        //}

        ///// <summary>
        ///// getter for the map-height
        ///// </summary>
        ///// 
        ///// <returns>
        ///// the height of the map
        ///// </returns>
        //public int getHeight()
        //{
        //    return this.image.Height;
        //}

        ///// <summary>
        ///// getter for the internal data
        ///// </summary>
        ///// 
        ///// <returns>
        ///// the internal data
        ///// </returns>
        //public EMapStates[,] getMapData()
        //{
        //    return data;
        //}

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice, Viewport viewport, Player player)
        {
            //Shortening the Draw call
            int width = image.Bounds.Width;
            int height = image.Bounds.Height;

            //Assumes GraphicsDevice previously contained default Viewport
            Viewport defaultView = GraphicsDevice.Viewport;

            //Switches to the given Viewport
            GraphicsDevice.Viewport = viewport;

            spriteBatch.Begin();
            //Positions texture in the middle of the screen with the Player Rotation set appropriately and the Player Position set as its origin
            spriteBatch.Draw(image, new Rectangle((int)(viewport.Width / 2), (int)(viewport.Height / 2), width, height), null, Color.White, -player.getRotation(), player.getPosition(), SpriteEffects.None, 0);
            spriteBatch.End();

            //Restores previous Viewport
            GraphicsDevice.Viewport = defaultView;
        }
    }
}