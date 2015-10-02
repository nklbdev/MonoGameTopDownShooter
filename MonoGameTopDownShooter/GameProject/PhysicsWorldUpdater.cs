using System;
using FarseerPhysics.Dynamics;

namespace GameProject
{
    public class PhysicsWorldUpdater : IMyUpdateable
    {
        private readonly World _world;

        public PhysicsWorldUpdater(World world)
        {
            _world = world;
        }

        public void Dispose()
        {
            throw new NotSupportedException();
        }

        public bool IsDisposed { get { return false; } }
        public event Action<IObservableDisposable> Disposed;
        public void Update(float elapsedSeconds)
        {
            _world.Step(elapsedSeconds);
        }
    }
}