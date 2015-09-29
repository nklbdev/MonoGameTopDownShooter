using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace XTiled
{
    public class TileLayer
    {
        public string Name;
        public float Opacity;
        public Color OpacityColor;
        public bool Visible;
        public Dictionary<string, Property> Properties;
        public TileData[][] Tiles;
    }
}