using System;
using Microsoft.Xna.Framework;

namespace GameProject.Entities
{
    public abstract class Entity : IEntity
    {
        private Ticker _ticker;

        public Ticker Ticker {
            get { return _ticker; }
            set
            {
                if (_ticker != value && _ticker != null)
                    _ticker.Ticked -= Update;
                _ticker = value;
                if (_ticker != null)
                    _ticker.Ticked += Update;
            }
        }

        public bool IsDisposed { get; private set; }

        public event Action<IEntity> Disposed;

        public abstract void Update(GameTime gameTime);

        public virtual void Dispose()
        {
            if (IsDisposed)
                return;
            _ticker.Ticked -= Update;

            IsDisposed = true;
            if (Disposed != null)
                Disposed(this);
        }
    }
}