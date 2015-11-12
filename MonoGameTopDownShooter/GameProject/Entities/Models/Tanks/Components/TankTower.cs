using System;
using GameProject.Spawners;
using Microsoft.Xna.Framework;

namespace GameProject.Entities.Models.Tanks.Components
{
    public class TankTower : ITankTower
    {
        private readonly ITank _tank;
        private const float _fireRate = 2; //shots per second
        private const float _shotDuration = 1/_fireRate;
        private float _timeBeforeNextShot;
        private readonly IBulletSpawner _bulletSpawner;

        public Vector2 RelativePosition { get; set; }
        public float RelativeRotation { get; set; }
        public float AimingSpeed { get; set; }
        public Vector2 Target { get; set; }
        public bool IsAiming { get; set; }

        public bool IsFiring { get; set; }

        public Vector2 Position
        {
            get { return _tank.Position + RelativePosition; }
            set { RelativePosition = value - _tank.Position; }
        }

        public float Rotation
        {
            get { return _tank.Rotation + RelativeRotation; }
            set { RelativeRotation = value - _tank.Rotation; }
        }

        public TankTower(ITank tank, IBulletSpawner bulletSpawner)
        {
            _bulletSpawner = bulletSpawner;
            _tank = tank;
        }

        public void Update(float deltaTime)
        {
            var absDirection = Rotation.ToVector();
            var pathToTarget = Target - Position;
            var angleToTarget = Vector2Helpers.SignedAngle(absDirection, pathToTarget);
            RelativeRotation += angleToTarget > 0
                ? Math.Min(AimingSpeed*deltaTime, angleToTarget)
                : Math.Max(-AimingSpeed*deltaTime, angleToTarget);

            if (IsFiring)
            {
                _timeBeforeNextShot -= deltaTime;
                for (; _timeBeforeNextShot < 0; _timeBeforeNextShot += _shotDuration)
                    _bulletSpawner.Spawn(Position, Rotation, _tank);
            }
            else
            {
                _timeBeforeNextShot = Math.Max(0, _timeBeforeNextShot - deltaTime);
            }
        }
    }
}