using Microsoft.Xna.Framework;

namespace GameProject.Spawners
{
    public interface ITankSpawner
    {
        void Spawn(Vector2 position, float rotation);
    }
}