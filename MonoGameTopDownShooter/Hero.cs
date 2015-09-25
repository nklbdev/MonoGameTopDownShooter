using System;
using MonoGameTopDownShooter.HeroStates;
using MonoGameTopDownShooter.HeroStates.Character;

namespace MonoGameTopDownShooter
{
    public class Hero : Stateful<ICharacter>, IUpdateable
    {
        public Hero(IState<ICharacter> state)
        {
            if (state == null)
                throw new ArgumentNullException("state");
            State = state;
        }
        public void Update(float elapsedSeconds)
        {
            while (elapsedSeconds > 0)
                elapsedSeconds -= State.Update(elapsedSeconds);
        }
    }
}
