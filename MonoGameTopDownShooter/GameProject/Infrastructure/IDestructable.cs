using GameProject.Entities;

namespace GameProject.Infrastructure
{
    public interface IDestructable
    {
        event DestroyedEventHandler Destroyed;
        void Destroy();
        bool IsDestroyed { get; }
    }
}