using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XTiled
{
    /// <summary>
    /// Represents a placed tiled on the map
    /// </summary>
    public class TileData
    {
        /// <summary>
        /// Index of the source tile in the Map.SourceTiles collection
        /// </summary>
        public int SourceId;
        /// <summary>
        /// Horizontal and/or vertical flipping state of the placed tile
        /// </summary>
        public SpriteEffects Effects;
        /// <summary>
        /// Location in pixels of the tile on the map
        /// </summary>
        public Rectangle Target;
        /// <summary>
        /// Rotation of the placed tile, in radians
        /// </summary>
        public float Rotation;
    }
}