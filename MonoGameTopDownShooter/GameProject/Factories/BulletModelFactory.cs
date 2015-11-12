using System;
using FarseerPhysics.Dynamics;
using GameProject.Entities.Models.Bullets;
using GameProject.Entities.Models.Tanks;
using Microsoft.Xna.Framework;

namespace GameProject.Factories
{
    public class BulletModelFactory : IBulletModelFactory
    {
        private readonly World _world;

        public BulletModelFactory(World world)
        {
            if (world == null)
                throw new ArgumentNullException("world");
            _world = world;
        }

        public IBullet Create(Vector2 position, float rotation, ITank ownerTank)
        {
            return new SimpleBullet(_world, position, rotation, ownerTank);
        }
    }
}