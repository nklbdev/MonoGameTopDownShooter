using GameProject.Entities.Controllers;
using GameProject.Entities.Models.Tanks;

namespace GameProject.Factories
{
    public interface ITankControllerFactory
    {
        IController Create(ITank tank);
    }
}