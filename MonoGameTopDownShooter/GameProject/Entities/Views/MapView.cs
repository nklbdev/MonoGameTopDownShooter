using System;
using Microsoft.Xna.Framework;
using MonoGameProxies;
using XTiled;

namespace GameProject.Entities.Views
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

        protected override void OnRender(IImmutableSpriteBatch spriteBatch, ref Rectangle boundingRectangle)
        {
            _map.Draw(spriteBatch, boundingRectangle);
        }
    }
}