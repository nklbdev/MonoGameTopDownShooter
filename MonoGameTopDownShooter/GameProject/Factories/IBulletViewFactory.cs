using GameProject.Entities.Models.Bullets;
using GameProject.Entities.Views;

namespace GameProject.Factories
{
    public interface IBulletViewFactory
    {
        IView Create(IBullet model);
    }
}