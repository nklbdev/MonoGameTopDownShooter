using System;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using GameProject.Entities.Models.Tanks.Components;
using GameProject.Factories;
using Microsoft.Xna.Framework;

namespace GameProject.Entities.Models.Tanks
{
    public class Tank : EntityBase, ITank
    {
        public ITankTower Tower { get; private set; }
        public float Health { get; set; }

        public void TakeDamage(float damage)
        {
            if (IsDestroyed)
                throw new InvalidOperationException("Destroyed tank cannot take damage");
            if (Health > damage)
                Health -= damage;
            else
            {
                Health = 0;
                Destroy();
            }
        }

        protected override void OnUpdate(float deltaTime)
        {
            _body.Rotation += _rotatingSpeed*deltaTime*(int) RotatingDirection;
            _body.LinearVelocity = _body.Rotation.ToVector()*_movingSpeed*(int) MovingDirection;
            Tower.Update(deltaTime);
        }

        protected override void OnDestroy()
        {
            _world.RemoveBody(_body);
        }

        private readonly Body _body;
        private readonly World _world;
        private const float _movingSpeed = 10;
        private const float _rotatingSpeed = 3;

        public Vector2 Position
        {
            get { return _body.Position; }
            set { _body.Position = value;  }
        }

        public float Rotation
        {
            get { return _body.Rotation; }
            set { _body.Rotation = value;  }
        }

        public MovingDirection MovingDirection { get; set; }
        public RotatingDirection RotatingDirection { get; set; }

        public Tank(World world, Vector2 position, float rotation, ITankTowerFactory tankTowerFactory)
        {
            _world = world;
            _body = new Body(_world, position, rotation, BodyType.Dynamic)
            {
                FixedRotation = true,
                UserData = this
            };
            FixtureFactory.AttachCircle(2.4f, 1, _body, Vector2.Zero);
            Tower = tankTowerFactory.Create(this);
        }
    }
}
