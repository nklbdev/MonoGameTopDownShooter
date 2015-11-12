using Microsoft.Xna.Framework;

namespace GameProject
{
    public interface ITankSpawner
    {
        void Spawn(Vector2 position, float rotation);
    }
}