using System;
using GameProject.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XTiled;

namespace GameProject.Views
{
    public class MapView : Entity
    {
        private Map _map;
        private SpriteBatch _spriteBatch;

        public MapView(Map map, SpriteBatch spriteBatch)
        {
            if (map == null)
                throw new ArgumentNullException("map");
            if (spriteBatch == null)
                throw new ArgumentNullException("spriteBatch");
            _map = map;
            _spriteBatch = spriteBatch;
        }

        public override void Update(GameTime gameTime)
        {
            _map.Draw(_spriteBatch, new Rectangle(0, 0, 32*32, 32*32));
        }
    }
}