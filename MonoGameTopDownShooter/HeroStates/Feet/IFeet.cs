namespace MonoGameTopDownShooter.HeroStates.Feet
{
    public interface IFeet
    {
        void Move(float yaw);
        void TurnTo(float rotation);
        void Crouch();
        void Run();
        void Walk();
        void Stop();
    }
}
