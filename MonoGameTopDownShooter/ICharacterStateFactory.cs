using MonoGameTopDownShooter.HeroStates.CharacterStates;
using MonoGameTopDownShooter.States;

namespace MonoGameTopDownShooter
{
    public interface ICharacterStateFactory
    {
        IState<ICharacter> CreateAliveState(ICharacter character);
        IState<ICharacter> CreateDeadState(ICharacter character);
    }
}