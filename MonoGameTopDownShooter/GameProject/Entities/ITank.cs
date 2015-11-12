using Microsoft.Xna.Framework;

namespace GameProject.Entities
{
    public interface ITank : IModel
    {
        ITankBody Body { get; }
        ITankTower Tower { get; }
        ITankArmor Armor { get; }
        Vector2 ControlColumnPosition { get; set; }
    }
}