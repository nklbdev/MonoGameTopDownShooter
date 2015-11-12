using GameProject.Entities.Models.Tanks.Components;

namespace GameProject.Entities.Models.Tanks
{
    public interface ITank : IModel
    {
        ITankBody Body { get; }
        ITankTower Tower { get; }
        ITankArmor Armor { get; }
    }
}