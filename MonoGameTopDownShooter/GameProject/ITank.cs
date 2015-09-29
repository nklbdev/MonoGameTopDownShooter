using Microsoft.Xna.Framework;

namespace GameProject
{
    public interface ITank : IUpdateable, IDrawable
    {
        void Fire();
        void RotateTowerTo(Vector2 destination);
        void RotateBody(bool isClockWise);
        void Move(bool isForward);
        void TakeDamage(float damage);
    }
}