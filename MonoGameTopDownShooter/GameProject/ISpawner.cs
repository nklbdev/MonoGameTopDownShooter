using Microsoft.Xna.Framework;

namespace GameProject
{
    public interface ISpawner
    {
        void Spawn(Vector2 position, float rotation);
    }
}