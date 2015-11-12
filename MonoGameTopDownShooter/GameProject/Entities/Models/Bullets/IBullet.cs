using Microsoft.Xna.Framework;

namespace GameProject.Entities.Models.Bullets
{
    public interface IBullet : IModel
    {
        Vector2 Position { get; }
        float Rotation { get; }
    }
}