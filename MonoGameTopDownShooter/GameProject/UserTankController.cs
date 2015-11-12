using System;
using GameProject.Entities;
using Microsoft.Xna.Framework.Input;

namespace GameProject
{
    public class UserTankController : NewEntityBase, IController
    {
        private readonly ITank _tank;

        public UserTankController(ITank tank)
        {
            if (tank == null)
                throw new ArgumentNullException("tank");
            _tank = tank;
            _tank.Destroyed += TankOnDestroyed;
        }

        private void TankOnDestroyed(INewEntity entity)
        {
            if (entity != _tank)
                return;
            Destroy();
        }

        public override void OnDestroy()
        {
            _tank.Destroyed -= TankOnDestroyed;
        }

        public override void OnUpdate(float deltaTime)
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
