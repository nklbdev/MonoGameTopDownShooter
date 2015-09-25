using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTopDownShooter.HeroStates.VehicleStates;
using MonoGameTopDownShooter.HeroStates.WeaponStates;
using MonoGameTopDownShooter.States;

namespace MonoGameTopDownShooter.HeroStates.CharacterStates
{
    public class AliveCharacterState : State<ICharacter>, ICharacter
    {
        private readonly Stateful<IWeapon> _hands;
        private readonly Stateful<IVehicle> _feet;
        private readonly ICharacter _character;
        private readonly ICharacterStateFactory _factory;

        public AliveCharacterState(ICharacter character, Stateful<IWeapon> hands, Stateful<IVehicle> feet, Texture2D texture, ICharacterStateFactory factory)
        {
            if (hands == null)
                throw new ArgumentNullException("hands");
            if (feet == null)
                throw new ArgumentNullException("feet");
            if (character == null)
                throw new ArgumentNullException("character");
            if (factory == null)
                throw new ArgumentNullException("factory");
            _hands = hands;
            _feet = feet;
            _character = character;

            _texture = texture;
            _factory = factory;
        }

        public void Die()
        {
            _hands.State.Gist.Drop();
            _hands.State.Gist.Destroy();
            _feet.State.Gist.Destroy();
            Owner.State = _factory.CreateDeadState(_character);
        }

        public void Fire()
        {
            _hands.State.Gist.Fire();
        }

        public void Drop()
        {
            _hands.State.Gist.Drop();
        }

        public void TurnTo(float rotation)
        {
            _feet.State.Gist.TurnTo(rotation);
        }

        public void Move(float yaw)
        {
            _feet.State.Gist.Move(yaw);
        }

        public override ICharacter Gist { get { return this; } }

        public override float Update(float elapsedSeconds)
        {
            var handsElapsedSeconds = elapsedSeconds;
            var feetElapsedSeconds = elapsedSeconds;
            while (handsElapsedSeconds > 0 || feetElapsedSeconds > 0)
            {
                if (handsElapsedSeconds > 0)
                    handsElapsedSeconds -= _hands.State.Update(handsElapsedSeconds);
                if (feetElapsedSeconds > 0)
                    feetElapsedSeconds -= _feet.State.Update(feetElapsedSeconds);
            }

            return elapsedSeconds;
        }

        public Vector2 Position
        {
            get { return _feet.State.Gist.Position; }
            set { _feet.State.Gist.Position = value; }
        }

        public float Rotation
        {
            get { return _feet.State.Gist.Rotation; }
            set { _feet.State.Gist.Rotation = value; }
        }

        private readonly Vector2 _size = new Vector2(50, 50);
        private readonly Texture2D _texture;

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //draw some
            spriteBatch.DrawRectangle(Position, _size, Color.Green, _texture);
            spriteBatch.DrawRay(Position, Rotation, 300, Color.Green, _texture);

            _feet.State.Gist.Draw(spriteBatch, gameTime);
            _hands.State.Gist.Draw(spriteBatch, gameTime);
        }
    }
}
