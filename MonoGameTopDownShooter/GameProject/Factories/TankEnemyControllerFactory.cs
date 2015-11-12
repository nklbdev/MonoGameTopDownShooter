using GameProject.Entities.Controllers;
using GameProject.Entities.Models.Tanks;

namespace GameProject.Factories
{
    public class TankEnemyControllerFactory : ITankControllerFactory
    {
        public IController Create(ITank tank)
        {
            return new EnemyTankController(tank);
        }
    }
}