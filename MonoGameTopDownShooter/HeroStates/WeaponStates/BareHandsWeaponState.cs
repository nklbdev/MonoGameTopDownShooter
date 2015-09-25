using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTopDownShooter.HeroStates.CharacterStates;
using MonoGameTopDownShooter.States;

namespace MonoGameTopDownShooter.HeroStates.WeaponStates
{
    public class BareHandsWeaponState : State<IWeapon>, IWeapon
    {
        private bool _continueFiring;
        private readonly ICharacter _character;

        public BareHandsWeaponState(ICharacter character)
        {
            if (character == null)
                throw new ArgumentNullException("character");
            _character = character;
        }

        public void Fire()
        {
            _continueFiring = true;
        }

        public void Drop()
        {
            //Do nothing
        }

        public void Destroy()
        {
            throw new NotImplementedException();
        }

        public override IWeapon Gist { get { return this; } }

        public override float Update(float elapsedSeconds)
        {
            if (!_continueFiring)
                return elapsedSeconds;
            throw new NotImplementedException();
            //циклически наносить удар через равные промежутки времени, пока не закончится время в буфере
            //
            _continueFiring = false;
            return elapsedSeconds;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //todo: draw some
        }
    }
}
