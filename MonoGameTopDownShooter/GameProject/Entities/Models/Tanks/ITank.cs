using GameProject.Entities.Models.Tanks.Components;

namespace GameProject.Entities.Models.Tanks
{
    public interface ITank : IModel, IPivot
    {
        MovingDirection MovingDirection { get; set; }
        RotatingDirection RotatingDirection { get; set; }
        //ITankBody Body { get; }
        ITankTower Tower { get; }
        ITankArmor Armor { get; }
    }
}