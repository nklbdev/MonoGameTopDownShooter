using Microsoft.Xna.Framework;

namespace GameProject.Entities
{
    public interface IBullet : IEntity
    {
        Vector2 Position { get; }
        float Rotation { get; }
    }
}