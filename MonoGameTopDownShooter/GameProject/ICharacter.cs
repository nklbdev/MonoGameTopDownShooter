using Microsoft.Xna.Framework;

namespace GameProject
{
    public interface ICharacter : IUpdateable, IDrawable
    {
        void Fire();
        void LookAt(Vector2 destination);
        void Move(float yaw);
        void TurnTo(float rotation);
        void TakeDamage(float damage);
    }
}