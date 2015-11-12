using System;
using System.Diagnostics;

namespace GameProject
{
    public class EntityUpdater<TEntity> : IUpdateable where TEntity : INewEntity
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
            Trace.WriteLine("EntityUpdater.Update");
            _processor.Process(e => e.Update(deltaTime));
        }
    }
}