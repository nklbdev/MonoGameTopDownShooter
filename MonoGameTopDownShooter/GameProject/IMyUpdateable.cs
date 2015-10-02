namespace GameProject
{
    public interface IMyUpdateable : IObservableDisposable
    {
        void Update(float elapsedSeconds);
    }
}