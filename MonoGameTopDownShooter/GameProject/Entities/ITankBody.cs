namespace GameProject.Entities
{
    public interface ITankBody : IPivot, INewEntity
    {
        MovingDirection MovingDirection { get; set; }
        RotatingDirection RotatingDirection { get; set; }
        void Update(float elapsedSeconds);
    }
}