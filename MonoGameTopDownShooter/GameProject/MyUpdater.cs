using System;
using System.Collections.Generic;

namespace GameProject
{
    public class MyUpdater : IMyUpdater
    {
        private readonly HashSet<IMyUpdateable> _updateables = new HashSet<IMyUpdateable>();
        private bool _isUpdating;
        private readonly Queue<IMyUpdateable> _toRemove = new Queue<IMyUpdateable>();
        private readonly Queue<IMyUpdateable> _toAdd = new Queue<IMyUpdateable>();

        public MyUpdater()
        {
            IsDisposed = false;
        }

        public void Update(float elapsedSeconds)
        {
            _isUpdating = true;

            foreach (var updateable in _updateables)
                if (updateable.IsDisposed)
                    _toRemove.Enqueue(updateable);
                else
                    updateable.Update(elapsedSeconds);

            _isUpdating = false;

            ProcessQueues();
        }

        private void ProcessQueues()
        {
            if (_isUpdating)
                return;

            while (_toRemove.Count > 0)
            {
                var updatable = _toRemove.Dequeue();
                if (_updateables.Remove(updatable))
                    updatable.Disposed -= UpdateableOnDisposed;
            }

            while (_toAdd.Count > 0)
            {
                var updatable = _toAdd.Dequeue();
                if (updatable.IsDisposed) continue;
                if (_updateables.Add(updatable))
                    updatable.Disposed += UpdateableOnDisposed;
            }
        }

        public void AddUpdateable(IMyUpdateable updateable)
        {
            if (updateable == null)
                return;
            _toAdd.Enqueue(updateable);
            ProcessQueues();
        }

        private void UpdateableOnDisposed(IObservableDisposable disposable)
        {
            RemoveUpdateable(disposable as IMyUpdateable);
        }

        public void RemoveUpdateable(IMyUpdateable updateable)
        {
            if (updateable == null)
                return;
            _toRemove.Enqueue(updateable);
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