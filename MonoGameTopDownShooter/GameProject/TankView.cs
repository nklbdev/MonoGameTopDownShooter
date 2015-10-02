using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject
{
    public class TankView : IMyDrawable
    {
        private Tank _tank;
        private Texture2D _bodyTexture;
        private Texture2D _towerTexture;

        public TankView(Tank tank, Texture2D bodyTexture, Texture2D towerTexture)
        {
            if (tank == null)
                throw new ArgumentNullException("tank");
            if (tank.IsDisposed)
                throw new ObjectDisposedException("tank");
            if (bodyTexture == null)
                throw new ArgumentNullException("bodyTexture");
            if (towerTexture == null)
                throw new ArgumentNullException("towerTexture");
            _tank = tank;
            _bodyTexture = bodyTexture;
            _towerTexture = towerTexture;
            _tank.Disposed += TankOnDisposed;
        }

        private void TankOnDisposed(IObservableDisposable disposable)
        {
            if (disposable != _tank)
                return;
            _tank.Disposed -= TankOnDisposed;
            Dispose();
        }

        public void Dispose()
        {
            if (IsDisposed)
                return;
            _tank = null;
            _bodyTexture = null;
            _towerTexture = null;
            IsDisposed = true;
            if (Disposed != null)
                Disposed(this);
        }

        public bool IsDisposed { get; private set; }
        public event Action<IObservableDisposable> Disposed;
        public void Draw(SpriteBatch spriteBatch, float elapsedSeconds)
        {
            if (IsDisposed)
                return;
            spriteBatch.Draw(
                _bodyTexture,
                new Rectangle(_tank.Position.ToPoint(), new Point(100, 100)),
                null,
                Color.White,
                _tank.Rotation,
                new Vector2(_bodyTexture.Width / 2, _bodyTexture.Height / 2),
                SpriteEffects.None,
                0);

            spriteBatch.Draw(
                _towerTexture,
                new Rectangle(_tank.Position.ToPoint(), new Point(100, 100)),
                null,
                Color.White,
                _tank.Rotation,
                new Vector2(_towerTexture.Width / 2, _towerTexture.Height / 2),
                SpriteEffects.None,
                0);
        }
    }
}