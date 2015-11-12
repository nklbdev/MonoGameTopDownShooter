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
        public ITankArmor Armor { get; private set; }

        protected override void OnUpdate(float deltaTime)
        {
            _body.Rotation += _rotatingSpeed*deltaTime*(int) RotatingDirection;
            _body.LinearVelocity = _body.Rotation.ToVector()*_movingSpeed*(int) MovingDirection;
            Tower.Update(deltaTime);
            //Armor.Update(deltaTime);
        }

        protected override void OnDestroy()
        {
            _world.RemoveBody(_body);
            //Armor.Dispose();
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
            _body = new Body(_world, position, rotation, BodyType.Dynamic) { FixedRotation = true };
            FixtureFactory.AttachCircle(2.4f, 1, _body, Vector2.Zero);
            _body.UserData = this;
            Tower = tankTowerFactory.Create(this);
        }
    }
}
