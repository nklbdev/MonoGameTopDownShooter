using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject
{
    public class MyDrawer : IMyDrawer
    {
        private readonly HashSet<IMyDrawable> _drawables = new HashSet<IMyDrawable>();
        private bool _isUpdating;
        private readonly Queue<IMyDrawable> _toRemove = new Queue<IMyDrawable>();
        private readonly Queue<IMyDrawable> _toAdd = new Queue<IMyDrawable>();

        public MyDrawer()
        {
            IsDisposed = false;
        }

        public void Draw(SpriteBatch spriteBatch, float elapsedSeconds)
        {
            _isUpdating = true;

            foreach (var drawable in _drawables)
                if (drawable.IsDisposed)
                    _toRemove.Enqueue(drawable);
                else
                    drawable.Draw(spriteBatch, elapsedSeconds);

            _isUpdating = false;

            ProcessQueues();
        }

        private void ProcessQueues()
        {
            if (_isUpdating)
                return;

            while (_toRemove.Count > 0)
            {
                var dequeue = _toRemove.Dequeue();
                if (_drawables.Remove(dequeue))
                    dequeue.Disposed -= DrawableOnDisposed;
            }

            while (_toAdd.Count > 0)
            {
                var drawable = _toAdd.Dequeue();
                if (drawable.IsDisposed) continue;
                if (_drawables.Add(drawable))
                    drawable.Disposed += DrawableOnDisposed;
            }
        }

        public void AddDrawable(IMyDrawable drawable)
        {
            if (drawable == null)
                return;
            _toAdd.Enqueue(drawable);
            ProcessQueues();
        }

        private void DrawableOnDisposed(IObservableDisposable disposable)
        {
            RemoveDrawable(disposable as IMyDrawable);
        }

        public void RemoveDrawable(IMyDrawable drawable)
        {
            if (drawable == null)
                return;
            _toRemove.Enqueue(drawable);
            ProcessQueues();
        }

        public void Dispose()
        {
            throw new NotSupportedException();
        }

        public bool IsDisposed { get; private set; }
        public event Action<IObservableDisposable> Disposed;
    }
}