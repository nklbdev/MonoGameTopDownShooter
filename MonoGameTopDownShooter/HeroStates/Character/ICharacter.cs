using MonoGameTopDownShooter.HeroStates.Feet;
using MonoGameTopDownShooter.HeroStates.Hands;

namespace MonoGameTopDownShooter.HeroStates.Character
{
    public interface ICharacter : IEntity, IHands, IFeet
    {
        void Die();
    }
}
