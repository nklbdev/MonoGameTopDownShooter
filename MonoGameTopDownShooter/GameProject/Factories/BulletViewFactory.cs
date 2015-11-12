using System;
using GameProject.Entities.Models.Bullets;
using GameProject.Entities.Views;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Factories
{
    public class BulletViewFactory : IBulletViewFactory
    {
        private readonly Texture2D _bulletTexture;

        public BulletViewFactory(Texture2D bulletTexture)
        {
            if (bulletTexture == null)
                throw new ArgumentNullException("bulletTexture");
            _bulletTexture = bulletTexture;
        }

        public IView Create(IBullet bullet)
        {
            return new BulletView(bullet, _bulletTexture);
        }
    }
}