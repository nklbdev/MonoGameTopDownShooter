using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;

namespace GameProject.Entities.Models.Tanks.Components
{
    public class TankBody : ITankBody
    {
        private readonly Body _body;
        private readonly World _world;
        private const float _movingSpeed = 10;
        private const float _rotatingSpeed = 3;
        public bool IsDestroyed { get; private set; }

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

        public TankBody(World world, Vector2 position, float rotation, ITank ownerTank)
        {
            _world = world;
            _body = new Body(_world, position, rotation, BodyType.Dynamic) { FixedRotation = true };
            FixtureFactory.AttachCircle(2.4f, 1, _body, Vector2.Zero);
            _body.UserData = ownerTank;
        }

        public void Update(float deltaTime)
        {
            _body.Rotation += _rotatingSpeed*deltaTime*(int) RotatingDirection;
            _body.LinearVelocity = _body.Rotation.ToVector()*_movingSpeed*(int) MovingDirection;
        }

        public event DestroyedEventHandler Destroyed;

        public void Destroy()
        {
            if (IsDestroyed)
                return;
            _world.RemoveBody(_body);
            IsDestroyed = true;
            if (Destroyed != null)
                Destroyed(this);
        }
    }
}