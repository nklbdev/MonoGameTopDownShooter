using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Polly.MonoGame.XTiledExtensions
{
    public class Tile
    {
        public int TilesetID;
        public Dictionary<string, Property> Properties;
        public Rectangle Source;
        public Vector2 Origin;
    }
}