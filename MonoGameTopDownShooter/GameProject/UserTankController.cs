using GameProject.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameProject
{
    public class UserTankController : Entity
    {
        private readonly ITank _tank;

        public UserTankController(ITank tank)
        {
            _tank = tank;
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W))
                _tank.Body.MovingDirection = MovingDirection.Forward;
            else if (Keyboard.GetState().IsKeyDown(Keys.S))
                _tank.Body.MovingDirection = MovingDirection.Backward;
            else
                _tank.Body.MovingDirection = MovingDirection.None;

            if (Keyboard.GetState().IsKeyDown(Keys.A))
                _tank.Body.RotatingDirection = RotatingDirection.Left;
            else if (Keyboard.GetState().IsKeyDown(Keys.D))
                _tank.Body.RotatingDirection = RotatingDirection.Right;
            else
                _tank.Body.RotatingDirection = RotatingDirection.None;


            _tank.Tower.Target = Mouse.GetState().Position.ToVector2() / 10;

            _tank.Tower.IsFiring = Mouse.GetState().LeftButton == ButtonState.Pressed;
        }
    }
}
