namespace MonoGameTopDownShooter.HeroStates
{
    public interface IState<T> where T : class
    {
        T Gist { get; }
        void Bring(Stateful<T> owner);
        Stateful<T> Owner { get; }
        void Leave();
        float Update(float elapsedSeconds);
    }
}
