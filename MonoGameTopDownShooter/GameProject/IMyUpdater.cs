namespace GameProject
{
    public interface IMyUpdater : IMyUpdateable
    {
        void AddUpdateable(IMyUpdateable updateable);
        void RemoveUpdateable(IMyUpdateable updateable);
    }
}