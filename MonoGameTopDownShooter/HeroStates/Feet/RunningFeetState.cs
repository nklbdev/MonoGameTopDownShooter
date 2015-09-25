using System;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace MonoGameTopDownShooter.HeroStates.Feet
{
    public class RunningFeetState : State<IFeet>, IFeet
    {
        private readonly Body _body;
        private bool _continue;

        public RunningFeetState(Body body)
        {
            _body = body;
        }

        public void Move(float yaw)
        {
            _continue = true;
        }

        public void TurnTo(float rotation)
        {
            //на бегу повороты медленнее, чем при ходьбе
            throw new NotImplementedException();
        }

        public void Run()
        {
            //Do nothing
        }

        public void Walk()
        {
            Owner.State = new WalkingFeetState(_body);
        }

        public override IFeet Gist { get { return this; } }

        public override void Bring(Stateful<IFeet> owner)
        {
            base.Bring(owner);
            throw new NotImplementedException();
        }

        public override void Leave()
        {
            base.Leave();
            throw new NotImplementedException();
        }

        public override float Update(float elapsedSeconds)
        {
            if (_continue)
            {
                _body.LinearVelocity = new Vector2(1000, 0);
                return elapsedSeconds;
            }
            _body.LinearVelocity = Vector2.Zero;
            return 0;
        }
    }
}
