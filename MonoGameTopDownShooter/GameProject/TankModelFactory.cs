using System;
using FarseerPhysics.Dynamics;
using GameProject.Entities;
using Microsoft.Xna.Framework;

namespace GameProject
{
    public class TankModelFactory : ITankModelFactory
    {
        private readonly World _world;
        private readonly IBulletSpawner _bulletSpawner;

        public TankModelFactory(World world, IBulletSpawner bulletSpawner)
        {
            if (world == null)
                throw new ArgumentNullException("world");
            _world = world;
            _bulletSpawner = bulletSpawner;
        }

        public ITank Create(Vector2 position, float rotation)
        {
            var tank = new Tank();
            var body = new TankBody(_world, position, rotation, tank);
            var tower = new TankTower(body, _bulletSpawner, tank) { AimingSpeed = 3 };
            tank.Body = body;
            tank.Tower = tower;
            return tank;
        }
    }
}