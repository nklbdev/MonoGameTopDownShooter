using Microsoft.Xna.Framework;

namespace GameProject.Entities
{
    public interface IPivoted : IPivot
    {
        Vector2 RelativePosition { get; set; }
        float RelativeRotation { get; set; }
    }
}