using GameProject.Entities.Models.Tanks;
using Microsoft.Xna.Framework;

namespace GameProject.Spawners
{
    public interface IBulletSpawner
    {
        void Spawn(Vector2 position, float rotation, ITank ownerTank);
    }
}