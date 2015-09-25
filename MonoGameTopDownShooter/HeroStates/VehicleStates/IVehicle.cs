namespace MonoGameTopDownShooter.HeroStates.VehicleStates
{
    public interface IVehicle : ILocation, IDrawable
    {
        void Move(float yaw);
        void TurnTo(float rotation);
        void Destroy();
    }
}
