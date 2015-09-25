using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTopDownShooter.States;

namespace MonoGameTopDownShooter.HeroStates.CharacterStates
{
    public class DeadCharacterState : State<ICharacter>, ICharacter
    {
        private readonly ICharacter _character;

        public DeadCharacterState(ICharacter character, Texture2D texture)
        {
            if (character == null)
                throw new ArgumentNullException("character");
            if (texture == null)
                throw new ArgumentNullException("texture");
            _character = character;
            _texture = texture;
        }

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

        private readonly Vector2 _size = new Vector2(50, 50);
        private readonly Texture2D _texture;
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //draw some
            spriteBatch.DrawRectangle(Position, _size, Color.Red, _texture);
            spriteBatch.DrawRay(Position, Rotation, 300, Color.Red, _texture);
        }
    }
}