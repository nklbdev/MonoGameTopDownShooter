using System;
using GameProject.Entities.Models.Bullets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameProxies;

namespace GameProject.Entities.Views
{
    public class BulletView : ViewBase
    {
        private readonly IBullet _bullet;
        private readonly Texture2D _texture;
        private readonly Vector2 _bodyTextureCenter;
        private const float _scale = 10;
        private bool _isBulletDestroyed;

        public BulletView(IBullet bullet, Texture2D texture)
        {
            if (bullet == null)
                throw new ArgumentNullException("bullet");
            if (texture == null)
                throw new ArgumentNullException("texture");
            _bullet = bullet;
            _bullet.Destroyed += BulletOnDestroyed;
            _texture = texture;
            _bodyTextureCenter = new Vector2(_texture.Width, _texture.Height) / 2;
        }

        private void BulletOnDestroyed(object entity)
        {
            if (entity != _bullet)
                return;
            _isBulletDestroyed = true;
            //todo: change animation state from Flight to Explosion
            Destroy();
        }

        protected override void OnUpdate(float deltaTime)
        {
            //todo: Update animation
            //if current state is Explosion,
            //count down time to self-destroy and destroy self if it wasted
        }

        protected override void OnRender(IImmutableSpriteBatch spriteBatch, ref Rectangle boundingRectangle)
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
            _bullet.Destroyed -= BulletOnDestroyed;
        }
    }
}