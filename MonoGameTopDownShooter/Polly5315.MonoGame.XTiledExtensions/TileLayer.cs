using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Polly.MonoGame.XTiledExtensions
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