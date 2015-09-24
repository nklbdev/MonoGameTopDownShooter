using System;

namespace MonoGameTopDownShooter.HeroStates.Feet
{
    public class CrouchFeetState : State<IFeet>, IFeet
    {
        private readonly Stateful<IFeet> _owner;

        public CrouchFeetState(Stateful<IFeet> owner)
        {
            if (owner == null)
                throw new ArgumentNullException("owner");
            _owner = owner;
        }

        public void Move(float yaw)
        {
            //set chouch speed to physical body
        }

        public void TurnTo(float rotation)
        {
            //на бегу повороты медленнее, чем при ходьбе
            throw new NotImplementedException();
        }

        public void Crouch()
        {
            //Do nothing
        }

        public void Run()
        {
            //Do nothing
        }

        public void Walk()
        {
            //Do nothing
        }

        public void Stop()
        {
            //set zero speed to physical body
        }

        public override IFeet Gist { get { return this; } }

        public override void Bring()
        {
            base.Bring();
            throw new NotImplementedException();
        }

        public override void Leave()
        {
            base.Leave();
            throw new NotImplementedException();
        }

        public override float Update(float elapsedSeconds)
        {
            throw new NotImplementedException();
        }
    }
}