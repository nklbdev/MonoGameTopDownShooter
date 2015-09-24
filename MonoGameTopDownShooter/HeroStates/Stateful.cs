namespace MonoGameTopDownShooter.HeroStates
{
    public class Stateful<T> where T : class
    {
        private IState<T> _state;

        public IState<T> State
        {
            get { return _state; }
            set
            {
                if (_state == value)
                    return;
                if (_state != null)
                    _state.Leave();
                _state = value;
                if (_state != null)
                    _state.Bring();
            }
        }
    }
}
