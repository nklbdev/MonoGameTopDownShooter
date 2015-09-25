using Microsoft.Xna.Framework;

namespace MonoGameTopDownShooter
{
    public interface ILocation
    {
        Vector2 Position { get; set; }
        float Rotation { get; set; }
    }
}