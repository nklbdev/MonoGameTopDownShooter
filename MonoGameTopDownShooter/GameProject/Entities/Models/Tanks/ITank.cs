using GameProject.Entities.Models.Tanks.Components;

namespace GameProject.Entities.Models.Tanks
{
    public interface ITank : IModel, IPivot
    {
        MovingDirection MovingDirection { get; set; }
        RotatingDirection RotatingDirection { get; set; }
        ITankTower Tower { get; }
        void TakeDamage(float damage);
    }
}