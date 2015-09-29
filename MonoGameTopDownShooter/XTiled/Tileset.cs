using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XTiled
{
    public class Tileset
    {
        public string Name;
        public int TileWidth;
        public int TileHeight;
        public int Spacing;
        public int Margin;
        public int TileOffsetX;
        public int TileOffsetY;
        public Dictionary<string, Property> Properties;
        public Tile[] Tiles;
        public string ImageFileName;
        public Color? ImageTransparentColor;
        public int ImageWidth;
        public int ImageHeight;
        public Texture2D Texture;
    }
}