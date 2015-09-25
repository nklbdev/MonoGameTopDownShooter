using System;

namespace MonoGameTopDownShooter.States
{
    public class Stateful<T> where T : class
    {
        private IState<T> _state;

        public IState<T> State
        {
            get { return _state; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                if (_state == value)
                    return;
                if (_state != null)
                    _state.Leave();
                _state = value;
                    _state.Bring(this);
            }
        }
    }
}
