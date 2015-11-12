using System;

namespace GameProject
{
    public abstract class NewEntityBase : INewEntity
    {
        public void Update(float deltaTime)
        {
            if (IsDestroyed)
                throw new InvalidOperationException("Cannot update destroyed model");
            OnUpdate(deltaTime);
        }

        public virtual void OnUpdate(float deltaTime)
        { }

        public event EntityDestroyedEventHandler Destroyed;

        public void Destroy()
        {
            if (IsDestroyed)
                return;
            OnDestroy();
            IsDestroyed = true;
            if (Destroyed != null)
                Destroyed(this);
        }

        public virtual void OnDestroy()
        {
        }

        public bool IsDestroyed { get; private set; }
    }
}