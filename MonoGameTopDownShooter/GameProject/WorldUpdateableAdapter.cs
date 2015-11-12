using System;
using System.Diagnostics;
using FarseerPhysics.Dynamics;

namespace GameProject
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
            Trace.WriteLine("WorldUpdateableAdapter.Update");
            _world.Step(deltaTime);
        }
    }
}