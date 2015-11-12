using System;
using FarseerPhysics.Dynamics;
using GameProject.Entities.Models.Tanks;
using Microsoft.Xna.Framework;

namespace GameProject.Factories
{
    public class TankModelFactory : ITankModelFactory
    {
        private readonly World _world;
        private readonly ITankTowerFactory _tankTowerFactory;

        public TankModelFactory(World world, ITankTowerFactory tankTowerFactory)
        {
            if (world == null)
                throw new ArgumentNullException("world");
            if (tankTowerFactory == null)
                throw new ArgumentNullException("tankTowerFactory");
            _world = world;
            _tankTowerFactory = tankTowerFactory;
        }

        public ITank Create(Vector2 position, float rotation)
        {
            return new Tank(_world, position, rotation, _tankTowerFactory);
        }
    }
}