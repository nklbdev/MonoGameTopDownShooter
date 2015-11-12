namespace GameProject.Entities
{
    public interface ITankBody : IPivot, IEntity
    {
        MovingDirection MovingDirection { get; set; }
        RotatingDirection RotatingDirection { get; set; }
        void Update(float elapsedSeconds);
    }
}