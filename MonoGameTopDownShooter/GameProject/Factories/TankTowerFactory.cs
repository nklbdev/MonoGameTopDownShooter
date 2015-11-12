using System;
using GameProject.Entities.Models.Tanks;
using GameProject.Entities.Models.Tanks.Components;
using GameProject.Spawners;

namespace GameProject.Factories
{
    public class TankTowerFactory : ITankTowerFactory
    {
        private readonly IBulletSpawner _bulletSpawner;

        public TankTowerFactory(IBulletSpawner bulletSpawner)
        {
            if (bulletSpawner == null)
                throw new ArgumentNullException("bulletSpawner");
            _bulletSpawner = bulletSpawner;
        }

        public TankTower Create(ITank tank)
        {
            return new TankTower(tank, _bulletSpawner) { AimingSpeed = 3 };
        }
    }
}