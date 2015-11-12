using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;

namespace GameProject.Entities
{
    public class TankBody : EntityBase, ITankBody
    {
        private readonly Body _body;
        private readonly World _world;
        private const float _movingSpeed = 10;
        private const float _rotatingSpeed = 3;

        public TankBody(World world, Vector2 position, float rotation, ITank ownerTank)
        {
            _world = world;
            _body = new Body(_world, position, rotation, BodyType.Dynamic) { FixedRotation = true };
            FixtureFactory.AttachCircle(2.4f, 1, _body, Vector2.Zero);
            _body.UserData = ownerTank;
        }

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

        public Vector2 RelativePosition
        {
            get { return Position; }
            set { Position = value;  }
        }

        public float RelativeRotation
        {
            get { return Rotation; }
            set { Rotation = value;  }
        }

        public MovingDirection MovingDirection { get; set; }
        public RotatingDirection RotatingDirection { get; set; }

        protected override void OnUpdate(float elapsedSeconds)
        {
            _body.Rotation += _rotatingSpeed*elapsedSeconds*(int) RotatingDirection;
            _body.LinearVelocity = _body.Rotation.ToVector()*_movingSpeed*(int) MovingDirection;
        }

        protected override void OnDestroy()
        {
            _world.RemoveBody(_body);
        }
    }
}