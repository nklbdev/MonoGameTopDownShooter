using System;
using FarseerPhysics.Dynamics;

namespace GameProject.Infrastructure
{
    public class WorldUpdateableAdapter : IUpdateable
    {
        private readonly World _world;

        public WorldUpdateableAdapter(World world)
        {
            if (world == null)
                throw new ArgumentNullException("world");
            _world = world;
        }

        public void Update(float deltaTime)
        {
            _world.Step(deltaTime);
        }
    }
}