using Microsoft.Xna.Framework;

namespace GameProject.Entities
{
    public interface IBullet : IModel
    {
        Vector2 Position { get; }
        float Rotation { get; }
    }
}