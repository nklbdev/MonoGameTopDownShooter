namespace GameProject.Entities
{
    public interface ITankBody : IPivot, IObservableDisposable
    {
        MovingDirection MovingDirection { get; set; }
        RotatingDirection RotatingDirection { get; set; }
        void Update(float elapsedSeconds);
    }
}