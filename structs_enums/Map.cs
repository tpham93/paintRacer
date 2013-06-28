using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace paintRacer
{
    struct Map
    {
        public Texture2D image;
        public EMapStates[,] data;
        public Vector2[] checkPoints;
        public Vector2 start;
        public float startRotation;

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

        /// <summary>
        /// getter for the map-width
        /// </summary>
        /// 
        /// <returns>
        /// the width of the map
        /// </returns>
        public int getWidth()
        {
            return this.image.Width;
        }

        /// <summary>
        /// getter for the map-height
        /// </summary>
        /// 
        /// <returns>
        /// the height of the map
        /// </returns>
        public int getHeight()
        {
            return this.image.Height;
        }
    }
}