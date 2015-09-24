using System.Collections.Generic;

namespace MonoGameTopDownShooter
{
    public interface IDispatcher<T>
    {
        void Register(T item);
        void Deregister(T item);
        IEnumerable<T> Items { get; }
    }
}