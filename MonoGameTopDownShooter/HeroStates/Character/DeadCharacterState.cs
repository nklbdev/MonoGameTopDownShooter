using Microsoft.Xna.Framework;

namespace MonoGameTopDownShooter.HeroStates.Character
{
    public class DeadCharacterState : State<ICharacter>, ICharacter
    {
        public void Die()
        {
        }

        public void Fire()
        {
        }

        public void Drop()
        {
        }

        public void TurnTo(float rotation)
        {
        }

        public void Run()
        {
        }

        public void Walk()
        {
        }

        public void Move(float yaw)
        {
        }

        public override ICharacter Gist { get { return this; } }

        public override float Update(float elapsedSeconds)
        {
            return elapsedSeconds;
        }

        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
    }
}