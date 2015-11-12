using GameProject.Entities;

namespace GameProject
{
    public interface ITankControllerFactory
    {
        IController Create(ITank tank);
    }
}