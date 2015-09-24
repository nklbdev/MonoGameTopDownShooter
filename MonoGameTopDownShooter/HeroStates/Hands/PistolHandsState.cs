using System;

namespace MonoGameTopDownShooter.HeroStates.Hands
{
    public class PistolHandsState : State<IHands>, IHands
    {
        public void Fire()
        {
            throw new NotImplementedException();
        }

        public void Drop()
        {
            throw new NotImplementedException();
        }

        public override IHands Gist { get { return this; } }

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
