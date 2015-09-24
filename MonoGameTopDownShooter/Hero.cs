using MonoGameTopDownShooter.HeroStates;
using MonoGameTopDownShooter.HeroStates.Character;

namespace MonoGameTopDownShooter
{
    public class Hero : Stateful<ICharacter>, IUpdateable
    {
        public Hero()
        {
        }

        public void Update(float elapsedSeconds)
        {
            while (elapsedSeconds > 0)
                elapsedSeconds -= State.Update(elapsedSeconds);
        }
    }
}
