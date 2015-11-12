using System;

namespace GameProject.Entities
{
    public abstract class EntityBase : IEntity
    {
        public void Update(float deltaTime)
        {
            if (IsDestroyed)
                throw new InvalidOperationException("Cannot update destroyed model");
            OnUpdate(deltaTime);
        }

        protected virtual void OnUpdate(float deltaTime)
        { }

        public event DestroyedEventHandler Destroyed;

        public void Destroy()
        {
            if (IsDestroyed)
                return;
            OnDestroy();
            IsDestroyed = true;
            if (Destroyed != null)
                Destroyed(this);
        }

        protected virtual void OnDestroy()
        { }

        public bool IsDestroyed { get; private set; }
    }
}