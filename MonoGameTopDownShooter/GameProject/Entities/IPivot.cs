using Microsoft.Xna.Framework;

namespace GameProject.Entities
{
    public interface IPivot
    {
        Vector2 Position { get; set; }
        float Rotation { get; set; }
    }
}