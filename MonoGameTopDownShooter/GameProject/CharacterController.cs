using Microsoft.Xna.Framework.Input;

namespace GameProject
{
    public class CharacterController : IUpdateable
    {
        private readonly ITank _tank;

        public CharacterController(ITank tank)
        {
            _tank = tank;
        }

        public void Update(float elapsedSeconds)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W))
                _tank.Move(true);
            if (Keyboard.GetState().IsKeyDown(Keys.A))
                _tank.RotateBody(false);
            if (Keyboard.GetState().IsKeyDown(Keys.S))
                _tank.Move(false);
            if (Keyboard.GetState().IsKeyDown(Keys.D))
                _tank.RotateBody(true);
            _tank.RotateTowerTo(Mouse.GetState().Position.ToVector2());
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                _tank.Fire();
        }
    }
}
