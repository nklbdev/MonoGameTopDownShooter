namespace MonoGameTopDownShooter.HeroStates.CharacterStates
{
    public interface ICharacter : ILocation, IDrawable
    {
        void Fire();
        //void SetWeapon(IWeapon weapon);
        void Drop();
        void Die();
        void Move(float yaw);
        void TurnTo(float rotation);
    }
}
