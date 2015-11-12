using Microsoft.Xna.Framework;
using IUpdateable = GameProject.Infrastructure.IUpdateable;

namespace GameProject.Entities.Models.Tanks.Components
{
    public interface ITankTower : IPivoted, IUpdateable
    {
        float AimingSpeed { get; set; }
        Vector2 Target { get; set; }
        bool IsAiming { get; set; }
        bool IsFiring { get; set; }
    }
}