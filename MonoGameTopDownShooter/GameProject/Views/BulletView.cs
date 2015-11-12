using System;
using GameProject.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Views
{
    //public class BulletView : Entity
    //{
    //    private IBullet _bullet;
    //    private Texture2D _texture;
    //    private Texture2D _towerTexture;
    //    private SpriteBatch _spriteBatch;
    //    private readonly Vector2 _bodyTextureCenter;
    //    private const float _scale = 10;

    //    public BulletView(IBullet bullet, Texture2D texture, SpriteBatch spriteBatch)
    //    {
    //        if (bullet == null)
    //            throw new ArgumentNullException("bullet");
    //        if (texture == null)
    //            throw new ArgumentNullException("texture");
    //        if (spriteBatch == null)
    //            throw new ArgumentNullException("spriteBatch");
    //        _bullet = bullet;
    //        _bullet.Disposed += BulletOnDisposed;
    //        _texture = texture;
    //        _spriteBatch = spriteBatch;
    //        _bodyTextureCenter = new Vector2(_texture.Width, _texture.Height) / 2;
    //    }

    //    private void BulletOnDisposed(IObservableDisposable observableDisposable)
    //    {
    //        Dispose();
    //    }

    //    public override void Update(GameTime gameTime)
    //    {
    //        _spriteBatch.Draw(
    //            _texture,
    //            _bullet.Position * _scale,
    //            null,
    //            null,
    //            _bodyTextureCenter, //origin
    //            _bullet.Rotation,
    //            null,
    //            null);
    //    }

    //    public override void Dispose()
    //    {
    //        _bullet.Disposed -= BulletOnDisposed;
    //        base.Dispose();
    //    }
    //}
}