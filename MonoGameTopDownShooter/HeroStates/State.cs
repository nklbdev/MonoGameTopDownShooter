using System;

namespace MonoGameTopDownShooter.HeroStates
{
    public abstract class State<T> : IState<T> where T : class
    {
        public abstract T Gist { get; }
        public Stateful<T> Owner { get; private set; }

        public virtual void Bring(Stateful<T> owner)
        {
            if (owner == null)
                throw new ArgumentNullException("owner");
            Owner = owner;
        }

        public virtual void Leave()
        {
            Owner = null;
        }

        public abstract float Update(float elapsedSeconds);
    }
}