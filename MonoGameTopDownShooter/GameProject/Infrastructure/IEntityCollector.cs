namespace GameProject.Infrastructure
{
    public interface IEntityCollector<in TEntity>
    {
        void Collect(TEntity entity);
    }
}