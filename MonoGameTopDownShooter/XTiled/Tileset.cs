using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XTiled
{
    /// <summary>
    /// Represents a tileset (aka tile sheet) in a Map
    /// </summary>
    public class Tileset
    {
        /// <summary>
        /// Optional name of the tileset
        /// </summary>
        public string Name;
        /// <summary>
        /// Width of a single tile in pixels
        /// </summary>
        public int TileWidth;
        /// <summary>
        /// Height of a single tile in pixels
        /// </summary>
        public int TileHeight;
        /// <summary>
        /// Spacing between tiles in pixels
        /// </summary>
        public int Spacing;
        /// <summary>
        /// Outside margin of the tilesheet in pixels
        /// </summary>
        public int Margin;
        /// <summary>
        /// Horizontal offset of tiles
        /// </summary>
        public int TileOffsetX;
        /// <summary>
        /// Vertical offset of tiles
        /// </summary>
        public int TileOffsetY;
        /// <summary>
        /// Custom properties
        /// </summary>
        public Dictionary<string, Property> Properties;
        /// <summary>
        /// List of tiles in this tilesheet
        /// </summary>
        public Tile[] Tiles;
        /// <summary>
        /// Full path of the image referenced in the TMX file
        /// </summary>
        public string ImageFileName;
        /// <summary>
        /// Transparent color as set in the Tiled editor; null if not set
        /// </summary>
        public Color? ImageTransparentColor;
        /// <summary>
        /// Width of image in pixels
        /// </summary>
        public int ImageWidth;
        /// <summary>
        /// Height of image in pixels
        /// </summary>
        public int ImageHeight;
        /// <summary>
        /// Image as an XNA Texture2D instance; null if Map.LoadTextures is false
        /// </summary>
        public Texture2D Texture;
    }
}