using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTopDownShooter.HeroStates.CharacterStates;
using MonoGameTopDownShooter.States;

namespace MonoGameTopDownShooter.HeroStates.WeaponStates
{
    public class PistolWeaponState : State<IWeapon>, IWeapon
    {
        private readonly ICharacter _character;

        public PistolWeaponState(ICharacter character, Texture2D texture)
        {
            if (character == null)
                throw new ArgumentNullException("character");
            _character = character;
            _texture = texture;
        }

        public void Fire()
        {
            throw new NotImplementedException();
        }

        public void Drop()
        {
            //throw new NotImplementedException();
            Trace.WriteLine("Pistol is dropped");
        }

        public void Destroy()
        {
            Trace.WriteLine("Pistol is destroyed");
        }

        public override IWeapon Gist { get { return this; } }

        public override float Update(float elapsedSeconds)
        {
            //throw new NotImplementedException();
            return elapsedSeconds;
        }

        private readonly Vector2 _size = new Vector2(10, 10);
        private readonly Texture2D _texture;

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //todo: draw some
            spriteBatch.DrawRectangle(_character.Position + new Vector2((float)Math.Cos(_character.Rotation), (float)Math.Sin(_character.Rotation)) * 30, _size, Color.Black, _texture);

            //var direction = mousePosition - _hero.Position;
            //_hero.TurnTo((float) Math.Atan2(direction.Y, direction.X));
        }
    }
}
