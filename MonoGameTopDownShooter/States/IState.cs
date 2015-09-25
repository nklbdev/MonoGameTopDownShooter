namespace MonoGameTopDownShooter.States
{
    public interface IState<T> where T : class
    {
        T Gist { get; }
        void Bring(Stateful<T> owner);
        void Leave();
        float Update(float elapsedSeconds);
    }
}
