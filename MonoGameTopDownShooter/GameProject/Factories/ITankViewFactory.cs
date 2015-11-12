using GameProject.Entities.Models.Tanks;
using GameProject.Entities.Views;

namespace GameProject.Factories
{
    public interface ITankViewFactory
    {
        IView Create(ITank tank);
    }
}