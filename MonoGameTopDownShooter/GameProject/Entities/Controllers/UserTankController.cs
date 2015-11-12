using System;
using GameProject.Entities.Models.Tanks;
using Microsoft.Xna.Framework.Input;

namespace GameProject.Entities.Controllers
{
    public class UserTankController : EntityBase, IController
    {
        private readonly ITank _tank;

        public UserTankController(ITank tank)
        {
            if (tank == null)
                throw new ArgumentNullException("tank");
            _tank = tank;
            _tank.Destroyed += TankOnDestroyed;
        }

        private void TankOnDestroyed(object entity)
        {
            if (entity != _tank)
                return;
            Destroy();
        }

        protected override void OnDestroy()
        {
            _tank.Destroyed -= TankOnDestroyed;
        }

        protected override void OnUpdate(float deltaTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W))
                _tank.MovingDirection = MovingDirection.Forward;
            else if (Keyboard.GetState().IsKeyDown(Keys.S))
                _tank.MovingDirection = MovingDirection.Backward;
            else
                _tank.MovingDirection = MovingDirection.None;

            if (Keyboard.GetState().IsKeyDown(Keys.A))
                _tank.RotatingDirection = RotatingDirection.Left;
            else if (Keyboard.GetState().IsKeyDown(Keys.D))
                _tank.RotatingDirection = RotatingDirection.Right;
            else
                _tank.RotatingDirection = RotatingDirection.None;


            _tank.Tower.Target = Mouse.GetState().Position.ToVector2() / 10;

            _tank.Tower.IsFiring = Mouse.GetState().LeftButton == ButtonState.Pressed;
        }
    }
}
