using System;
using Microsoft.Xna.Framework;
using MonoGameProxies;
using XTiled;

namespace GameProject
{
    public class MapView : ViewBase
    {
        private readonly Map _map;

        public MapView(Map map)
        {
            if (map == null)
                throw new ArgumentNullException("map");
            _map = map;
        }

        public override void OnRender(IImmutableSpriteBatch spriteBatch, ref Rectangle boundingRectangle)
        {
            _map.Draw(spriteBatch, boundingRectangle);
        }
    }
}