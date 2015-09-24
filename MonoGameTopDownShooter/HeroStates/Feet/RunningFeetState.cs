using System;

namespace MonoGameTopDownShooter.HeroStates.Feet
{
    public class RunningFeetState : State<IFeet>, IFeet
    {
        private readonly Stateful<IFeet> _owner;

        public RunningFeetState(Stateful<IFeet> owner)
        {
            if (owner == null)
                throw new ArgumentNullException("owner");
            _owner = owner;
        }

        public void Move(float yaw)
        {
            throw new NotImplementedException();
        }

        public void TurnTo(float rotation)
        {
            //на бегу повороты медленнее, чем при ходьбе
            throw new NotImplementedException();
        }

        public void Crouch()
        {
            throw new NotImplementedException();
        }

        public void Run()
        {
            throw new NotImplementedException();
        }

        public void Walk()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
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
