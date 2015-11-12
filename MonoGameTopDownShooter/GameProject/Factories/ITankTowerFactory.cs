using GameProject.Entities.Models.Tanks;
using GameProject.Entities.Models.Tanks.Components;

namespace GameProject.Factories
{
    public interface ITankTowerFactory
    {
        TankTower Create(ITank tank);
    }
}