using GameProject.Entities.Models.Bullets;
using GameProject.Entities.Models.Tanks;
using Microsoft.Xna.Framework;

namespace GameProject.Factories
{
    public interface IBulletModelFactory
    {
        IBullet Create(Vector2 position, float rotation, ITank ownerTank);
    }
}