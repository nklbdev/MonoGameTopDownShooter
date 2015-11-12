using GameProject.Entities;

namespace GameProject
{
    public class TankEnemyControllerFactory : ITankControllerFactory
    {
        public IController Create(ITank tank)
        {
            return new EnemyTankController(tank);
        }
    }
}