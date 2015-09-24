using System;

namespace MonoGameTopDownShooter.HeroStates
{
    public interface IState<out T>
    {
        bool IsPresent { get; }
        T Gist { get; }
        void Bring();
        void Leave();
        float Update(float elapsedSeconds);
    }

    public abstract class State<T> : IState<T>
    {
        public bool IsPresent { get; private set; }
        public abstract T Gist { get; }

        protected State()
        {
            IsPresent = false;
        }

        public virtual void Bring()
        {
            if (IsPresent)
                throw new InvalidOperationException("Cannot bring in present state");
            IsPresent = true;
        }

        public virtual void Leave()
        {
            IsPresent = false;
        }

        public abstract float Update(float elapsedSeconds);
    }
}
