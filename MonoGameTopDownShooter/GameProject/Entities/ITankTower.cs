using Microsoft.Xna.Framework;

namespace GameProject.Entities
{
    public interface ITankTower : IPivoted, IEntity
    {
        float AimingSpeed { get; set; }
        Vector2 Target { get; set; }
        bool IsAiming { get; set; }
        bool IsFiring { get; set; }
        void Update(float elapsedSeconds);
    }
}