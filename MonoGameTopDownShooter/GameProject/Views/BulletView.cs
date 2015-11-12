using System;
using GameProject.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameProxies;

namespace GameProject.Views
{
    public class BulletView : ViewBase
    {
        private readonly IBullet _bullet;
        private readonly Texture2D _texture;
        private readonly Vector2 _bodyTextureCenter;
        private const float _scale = 10;

        public BulletView(IBullet bullet, Texture2D texture)
        {
            if (bullet == null)
                throw new ArgumentNullException("bullet");
            if (texture == null)
                throw new ArgumentNullException("texture");
            _bullet = bullet;
            _bullet.Destroyed += BulletOnDisposed;
            _texture = texture;
            _bodyTextureCenter = new Vector2(_texture.Width, _texture.Height) / 2;
        }

        private void BulletOnDisposed(IEntity entity)
        {
            if (entity == _bullet)
                Destroy();
        }

        public override void OnRender(IImmutableSpriteBatch spriteBatch, ref Rectangle boundingRectangle)
        {
            spriteBatch.Draw(
                _texture,
                _bullet.Position * _scale,
                null,
                null,
                _bodyTextureCenter, //origin
                _bullet.Rotation);
        }

        protected override void OnDestroy()
        {
            _bullet.Destroyed -= BulletOnDisposed;
        }
    }
}