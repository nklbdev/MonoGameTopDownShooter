namespace GameProject
{
    public interface IDestructable
    {
        event EntityDestroyedEventHandler Destroyed;
        void Destroy();
        bool IsDestroyed { get; }
    }
}