using System;

namespace GameProject
{
    public interface IObservableDisposable : IDisposable
    {
        bool IsDisposed { get; }
        event Action<IEntity> Disposed;
    }
}