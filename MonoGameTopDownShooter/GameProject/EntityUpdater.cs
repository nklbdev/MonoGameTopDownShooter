using System;

namespace GameProject
{
    public class EntityUpdater<TEntity> : IUpdateable where TEntity : IEntity
    {
        private readonly IProcessor<TEntity> _processor;

        public EntityUpdater(IProcessor<TEntity> processor)
        {
            if (processor == null)
                throw new ArgumentNullException("processor");
            _processor = processor;
        }

        public void Update(float deltaTime)
        {
            _processor.Process(e => e.Update(deltaTime));
        }
    }
}