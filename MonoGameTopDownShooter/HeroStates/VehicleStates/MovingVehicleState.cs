using System;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTopDownShooter.HeroStates.CharacterStates;
using MonoGameTopDownShooter.States;

namespace MonoGameTopDownShooter.HeroStates.VehicleStates
{
    public class MovingVehicleState : State<IVehicle>, IVehicle
    {
        private readonly Body _body;
        private bool _continue;
        private float _yaw;
        private const float _speed = 1000;
        private readonly ICharacter _character;

        public MovingVehicleState(ICharacter character, Body body)
        {
            if (body == null)
                throw new ArgumentNullException("body");
            if (character == null)
                throw new ArgumentNullException("character");
            _body = body;
            _character = character;
        }

        public void Move(float yaw)
        {
            _yaw = yaw;
            _continue = true;
        }

        public void TurnTo(float rotation)
        {
            _body.Rotation = rotation;
        }

        public void Destroy()
        {
            _body.Dispose();
        }

        public override IVehicle Gist { get { return this; } }

        public override float Update(float elapsedSeconds)
        {
            //Trace.WriteLine("Moving");
            if (_continue)
            {
                var direction = Vector2.Transform(Vector2.UnitX, Matrix.CreateRotationZ(_body.Rotation + _yaw));
                _body.LinearVelocity = direction * _speed;
                _continue = false;
                return elapsedSeconds;
            }
            Owner.State = new StayingVehicleState(_character, _body);
            return 0;
        }

        public Vector2 Position
        {
            get { return _body.Position; }
            set { _body.Position = value; }
        }

        public float Rotation
        {
            get { return _body.Rotation; }
            set { _body.Rotation = value; }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //draw some
        }
    }
}
