using System;
using GameProject.Entities.Models.Tanks;

namespace GameProject.Entities.Controllers
{
    public class EnemyTankController : EntityBase, IController
    {
        private readonly ITank _tank;

        public EnemyTankController(ITank tank)
        {
            if (tank == null)
                throw new ArgumentNullException("tank");
            _tank = tank;
            _tank.Destroyed += TankOnDestroyed;
        }

        private void TankOnDestroyed(object entity)
        {
            if (entity != _tank)
                return;
            Destroy();
        }

        protected override void OnDestroy()
        {
            _tank.Destroyed -= TankOnDestroyed;
        }

        protected override void OnUpdate(float deltaTime)
        {
            //todo: put code here
        }
    }
}