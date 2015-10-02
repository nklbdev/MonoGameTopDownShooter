using System;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject
{
    public class CustomMyComponent : IMyComponent
    {
        private IMyUpdateable _updationHandle;
        private IMyDrawable _drawingHandle;

        public CustomMyComponent(IMyUpdateable updationHandle, IMyDrawable drawingHandle)
        {
            if (updationHandle == null)
                throw new ArgumentNullException("updationHandle");
            if (drawingHandle == null)
                throw new ArgumentNullException("drawingHandle");
            _updationHandle = updationHandle;
            _updationHandle.Disposed += UpdationHandleOnDisposed;
            _drawingHandle = drawingHandle;
            _drawingHandle.Disposed += DrawingHandleOnDisposed;
        }

        private void DrawingHandleOnDisposed(IObservableDisposable observableDisposable)
        {
            if (observableDisposable != _drawingHandle)
                return;
            _drawingHandle.Disposed -= DrawingHandleOnDisposed;
            _drawingHandle = null;
        }

        private void UpdationHandleOnDisposed(IObservableDisposable observableDisposable)
        {
            if (observableDisposable != _updationHandle)
                return;
            _updationHandle.Disposed -= UpdationHandleOnDisposed;
            _updationHandle = null;
        }

        public void Dispose()
        {
            if (IsDisposed)
                return;
            IsDisposed = true;
            if (Disposed != null)
                Disposed(this);
        }

        public bool IsDisposed { get; private set; }
        public event Action<IObservableDisposable> Disposed;

        public void Update(float elapsedSeconds)
        {
            if (_updationHandle != null)
                _updationHandle.Update(elapsedSeconds);
        }

        public void Draw(SpriteBatch spriteBatch, float elapsedSeconds)
        {
            if (_drawingHandle != null)
                _drawingHandle.Draw(spriteBatch, elapsedSeconds);
        }
    }
}