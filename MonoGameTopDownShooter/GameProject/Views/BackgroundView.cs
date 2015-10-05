using System;
using GameProject.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Views
{
    public class BackgroundView : Entity
    {
        private Texture2D _texture;
        private SpriteBatch _spriteBatch;

        public BackgroundView(Texture2D texture, SpriteBatch spriteBatch)
        {
            if (texture == null)
                throw new ArgumentNullException("texture");
            if (spriteBatch == null)
                throw new ArgumentNullException("spriteBatch");
            _texture = texture;
            _spriteBatch = spriteBatch;
        }

        public override void Update(GameTime gameTime)
        {
            _spriteBatch.Draw(
                _texture,
                Vector2.Zero,
                null,
                null,
                null, //origin
                0,
                null,
                null);
        }
    }
}