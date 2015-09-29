using Microsoft.Xna.Framework.Input;

namespace GameProject
{
    public class CharacterController : IUpdateable
    {
        private readonly ICharacter _character;

        public CharacterController(ICharacter character)
        {
            _character = character;
        }

        public void Update(float elapsedSeconds)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W))
                _character.Move(0);
            _character.LookAt(Mouse.GetState().Position.ToVector2());
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                _character.Fire();
        }
    }
}
