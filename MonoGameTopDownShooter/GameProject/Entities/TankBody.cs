using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace GameProject.Entities
{
    public class TankBody : NewEntityBase, ITankBody
    {
        private readonly Body _physicalBody;
        private const float _movingSpeed = 10;
        private const float _rotatingSpeed = 3;

        public TankBody(Body physicalBody)
        {
            _physicalBody = physicalBody;
        }

        public Vector2 Position
        {
            get { return _physicalBody.Position; }
            set { _physicalBody.Position = value;  }
        }

        public float Rotation
        {
            get { return _physicalBody.Rotation; }
            set { _physicalBody.Rotation = value;  }
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

        public override void OnUpdate(float elapsedSeconds)
        {
            _physicalBody.Rotation += _rotatingSpeed*elapsedSeconds*(int) RotatingDirection;
            _physicalBody.LinearVelocity = _physicalBody.Rotation.ToVector()*_movingSpeed*(int) MovingDirection;
        }
    }
}