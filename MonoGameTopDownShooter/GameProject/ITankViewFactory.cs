using GameProject.Entities;

namespace GameProject
{
    public interface ITankViewFactory
    {
        IView Create(ITank tank);
    }
}