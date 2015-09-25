namespace MonoGameTopDownShooter.HeroStates.WeaponStates
{
    public interface IWeapon : IDrawable
    {
        void Fire();
        //void SetWeapon(IWeapon weapon);
        void Drop();
        void Destroy();
    }
}
