using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace XTiled
{
    public class MapObject
    {
        public string Name;
        public string Type;
        public Rectangle Bounds;
        public int? TileID;
        public bool Visible;
        public Dictionary<string, Property> Properties;
        public Polygon Polygon;
        public Polyline Polyline;
    }
}