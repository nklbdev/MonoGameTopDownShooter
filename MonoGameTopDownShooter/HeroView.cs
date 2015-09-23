using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameTopDownShooter
{
    public class HeroView : IDrawable
    {
        private readonly Hero _hero;
        private readonly Point _size = new Point(100);
        private readonly Vector2 _offset = new Vector2(-50, -50);
        private readonly Texture2D _texture;

        public HeroView(Hero hero, Texture2D texture)
        {
            if (hero == null)
                throw new ArgumentNullException("hero");
            _hero = hero;
            _texture = texture;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //spriteBatch.Draw(_texture, new Rectangle((_hero.Body.Position + _offset).ToPoint(), _size), Color.Black);
            spriteBatch.Draw(_texture, _hero.Body.Position, null, Color.Chocolate, 0f, Vector2.Zero, new Vector2(80f, 30f), SpriteEffects.None, 0f);
        }
    }
}
