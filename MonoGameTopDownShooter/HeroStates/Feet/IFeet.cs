namespace MonoGameTopDownShooter.HeroStates.Feet
{
    public interface IFeet
    {
        void Move(float yaw);
        void TurnTo(float rotation);
        void Run();
        void Walk();
    }
}
