using Microsoft.Xna.Framework;

namespace GameProject
{
    public interface IPivot
    {
        Vector2 Position { get; set; }
        float Rotation { get; set; }
    }
}