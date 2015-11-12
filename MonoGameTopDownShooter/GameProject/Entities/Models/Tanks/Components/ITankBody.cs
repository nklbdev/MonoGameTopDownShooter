using GameProject.Infrastructure;

namespace GameProject.Entities.Models.Tanks.Components
{
    public interface ITankBody : IPivot, IUpdateable, IDestructable
    {
        MovingDirection MovingDirection { get; set; }
        RotatingDirection RotatingDirection { get; set; }
    }
}