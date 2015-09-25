using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTopDownShooter.HeroStates.CharacterStates;
using MonoGameTopDownShooter.States;

namespace MonoGameTopDownShooter
{
    public class Hero : Stateful<ICharacter>, ICharacter, IUpdateable
    {
        public void Update(float elapsedSeconds)
        {
            while (elapsedSeconds > 0)
                elapsedSeconds -= State.Update(elapsedSeconds);
        }

        public Vector2 Position
        {
            get { return State.Gist.Position; }
            set { State.Gist.Position = value; }
        }

        public float Rotation
        {
            get { return State.Gist.Rotation; }
            set { State.Gist.Rotation = value; }
        }
        public void Fire()
        {
            State.Gist.Fire();
        }

        public void Drop()
        {
            State.Gist.Drop();
        }

        public void Move(float yaw)
        {
            State.Gist.Move(yaw);
        }

        public void TurnTo(float rotation)
        {
            State.Gist.TurnTo(rotation);
        }

        public void Die()
        {
            State.Gist.Die();
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            State.Gist.Draw(spriteBatch, gameTime);
        }
    }
}
