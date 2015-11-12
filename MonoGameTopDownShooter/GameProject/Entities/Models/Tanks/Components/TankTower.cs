using System;
using GameProject.Spawners;
using Microsoft.Xna.Framework;

namespace GameProject.Entities.Models.Tanks.Components
{
    public class TankTower : ITankTower
    {
        private readonly ITank _ownerTank;
        private readonly IPivot _pivot;
        private const float _fireRate = 4; //shots per second
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
            get { return _pivot.Position + RelativePosition; }
            set { RelativePosition = value - _pivot.Position; }
        }

        public float Rotation
        {
            get { return _pivot.Rotation + RelativeRotation; }
            set { RelativeRotation = value - _pivot.Rotation; }
        }

        public TankTower(IPivot pivot, IBulletSpawner bulletSpawner, ITank ownerTank)
        {
            _bulletSpawner = bulletSpawner;
            _pivot = pivot;
            _ownerTank = ownerTank;
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
                    _bulletSpawner.Spawn(Position, Rotation, _ownerTank);
            }
            else
            {
                _timeBeforeNextShot = Math.Max(0, _timeBeforeNextShot - deltaTime);
            }
        }
    }
}