using Microsoft.Xna.Framework;

namespace MonoGameTopDownShooter
{
    public interface IEntity
    {
        Vector2 Position { get; set; }
        float Rotation { get; set; }
    }
}