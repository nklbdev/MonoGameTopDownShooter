using FarseerPhysics.Dynamics;
using GameProject.Entities;
using Microsoft.Xna.Framework;

namespace GameProject
{
    public class PhysicsWorldUpdater : Entity
    {
        private readonly World _world;

        public PhysicsWorldUpdater(World world)
        {
            _world = world;
        }

        public override void Update(GameTime gameTime)
        {
            _world.Step((float) gameTime.ElapsedGameTime.TotalSeconds);
        }
    }
}