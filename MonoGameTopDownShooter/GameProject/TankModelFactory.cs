using System;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using GameProject.Entities;
using Microsoft.Xna.Framework;

namespace GameProject
{
    public class TankModelFactory : ITankModelFactory
    {
        private readonly World _world;

        public TankModelFactory(World world)
        {
            if (world == null)
                throw new ArgumentNullException("world");
            _world = world;
        }

        public ITank Create(Vector2 position, float rotation)
        {
            var physicalBody = new Body(_world, position, rotation, BodyType.Dynamic) { FixedRotation = true };
            FixtureFactory.AttachCircle(2.4f, 1, physicalBody, Vector2.Zero, this);
            var body = new TankBody(physicalBody);
            var tower = new TankTower(body) { AimingSpeed = 3 };
            var tank = new Tank(body, tower);
            physicalBody.UserData = tank;
            return tank;
        }
    }
}