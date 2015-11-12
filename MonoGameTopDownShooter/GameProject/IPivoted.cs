using Microsoft.Xna.Framework;

namespace GameProject
{
    public interface IPivoted : IPivot
    {
        Vector2 RelativePosition { get; set; }
        float RelativeRotation { get; set; }
    }
}