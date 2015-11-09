namespace GameProject
{
    public interface IUpdater
    {
        void AddUpdateable(IMyUpdateable updateable);
        void RemoveUpdateable(IMyUpdateable updateable);
    }
}