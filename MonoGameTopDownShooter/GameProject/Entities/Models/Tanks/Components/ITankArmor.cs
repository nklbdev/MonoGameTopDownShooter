namespace GameProject.Entities.Models.Tanks.Components
{
    public interface ITankArmor : IEntity
    {
        void TakeDamage(float damage);
    }
}