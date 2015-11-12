using GameProject.Entities.Controllers;
using GameProject.Entities.Models.Tanks;

namespace GameProject.Factories
{
    public class TankUserControllerFactory : ITankControllerFactory
    {
        public IController Create(ITank tank)
        {
            return new UserTankController(tank);
        }
    }
}