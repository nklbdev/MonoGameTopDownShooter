using System;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTopDownShooter.HeroStates.CharacterStates;
using MonoGameTopDownShooter.States;

namespace MonoGameTopDownShooter.HeroStates.VehicleStates
{
    public class StayingVehicleState : State<IVehicle>, IVehicle
    {
        private readonly Body _body;
        private readonly ICharacter _character;

        public StayingVehicleState(ICharacter character, Body body)
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
            //todo: normalize it!!!
            var state = new MovingVehicleState(_character, _body);
            Owner.State = state;
            state.Move(yaw);
            //Owner.State.Gist.Move(yaw);
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
            //Trace.WriteLine("Staying");
            _body.LinearVelocity = Vector2.Zero;
            return elapsedSeconds;
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
            //todo: draw some
        }
    }
}