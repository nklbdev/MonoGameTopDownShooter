using GameProject.Entities;

namespace GameProject
{
    public class TankUserControllerFactory : ITankControllerFactory
    {
        public IController Create(ITank tank)
        {
            return new UserTankController(tank);
        }
    }
}