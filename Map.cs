using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace paintRacer
{
    class Map
    {
        Texture2D image;
        Texture2D swImage;
        Texture2D backgroundImage;
        EMapStates[,] data;
        Vector2[] checkPoints;
        Vector2 start;
        float startRotation;
        Highscore highscore;

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
        public Texture2D Image
        {
            get { return image; }
        }
        public Texture2D SwImage
        {
            get { return swImage; }
        }
        public Highscore Highscore
        {
            get { return highscore; }
            set { highscore = value; }
        }


        ///// <summary>
        ///// Struct to save maps (constructor for the prototype)
        ///// </summary>
        ///// 
        ///// <param name="swImagePath">
        ///// the location of the black white picture
        ///// </param>
        ///// 
        ///// <param name="imagePath">
        ///// the location of the picture
        ///// </param>
        ///// 
        //public Map(string swImagePath, string imagePath)
        //{
        //    this.image = Helper.loadImage(imagePath);
        //    swImage = Helper.loadImage(swImagePath);
        //    this.data = MapReader.createDataFromSWImage(swImage);
        //    checkPoints = new Vector2[0];
        //    start = new Vector2(1535, 550);
        //    startRotation = 0;
        //}



        /// <summary>
        /// Struct to save maps
        /// </summary>
        /// 
        /// <param name="image">
        /// the location of the picture
        /// </param>
        /// 
        /// <param name="swImage">
        /// the location of the picture for collisionDetection
        /// </param>
        /// 
        /// <param name="highscore">
        /// Highscore-Object
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
        public Map(Texture2D image, Texture2D swImage, Highscore highscore, Vector2[] checkPoints, Vector2 startPoint, float startRotation)
        {
            this.image = image;
            this.swImage = swImage;
            this.data = MapReader.createDataFromSWImage(swImage);
            this.checkPoints = checkPoints;
            this.start = startPoint;
            this.startRotation = startRotation;
            this.highscore = highscore;
            this.backgroundImage = Helper.genRectangleTexture(1, 1, Config.MAP_BACKGROUND_COLOR);
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
        public Map(Texture2D image, Texture2D swImage, string directoryPath, Vector2[] checkPoints, Vector2 startPoint, float startRotation)
        {
            this.image = image;
            this.swImage = swImage;
            this.data = MapReader.createDataFromSWImage(swImage);
            this.checkPoints = checkPoints;
            this.start = startPoint;
            this.startRotation = startRotation;
            this.highscore = new Highscore(directoryPath+@"\highscore");
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
            spriteBatch.Draw(backgroundImage, new Rectangle(0,0,viewport.Width,viewport.Height), Color.White);
            //Positions texture in the middle of the screen with the Player Rotation set appropriately and the Player Position set as its origin
            spriteBatch.Draw(image, new Rectangle((int)(viewport.Width / 2), (int)(viewport.Height / 2), width, height), null, Color.White, -player.getRotation(), player.getPosition(), SpriteEffects.None, 0);
            spriteBatch.End();

            //Restores previous Viewport
            GraphicsDevice.Viewport = defaultView;
        }
    }
}