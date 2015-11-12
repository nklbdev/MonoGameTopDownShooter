namespace GameProject
{
    public interface IEntityCollector<in TEntity>
    {
        void Collect(TEntity entity);
    }
}