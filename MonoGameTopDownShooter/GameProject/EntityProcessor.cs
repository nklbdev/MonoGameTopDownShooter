using System;

namespace GameProject
{
    public class EntityProcessor<TEntity> : IEntityCollector<TEntity>, IProcessor<TEntity> where TEntity : class, INewEntity
    {
        private class EntityNode
        {
            public TEntity Entity;
            public EntityNode Next;
        }

        private EntityNode _firstNode;

        public void Process(Action<TEntity> action)
        {
            EntityNode previousNode = null;
            var currentNode = _firstNode;

            while (currentNode != null)
            {
                if (!currentNode.Entity.IsDestroyed)
                {
                    action(currentNode.Entity);
                }
                else
                {
                    if (previousNode == null)
                        _firstNode = currentNode.Next;
                    else
                        previousNode.Next = currentNode.Next;
                }
                previousNode = currentNode;
                currentNode = currentNode.Next;
            }
        }

        public void Collect(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            if (entity.IsDestroyed)
                return;
            var newNode = new EntityNode {Entity = entity, Next = _firstNode};
            _firstNode = newNode;
        }
    }
}