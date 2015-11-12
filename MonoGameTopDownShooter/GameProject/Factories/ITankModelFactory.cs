using GameProject.Entities.Models.Tanks;
using Microsoft.Xna.Framework;

namespace GameProject.Factories
{
    public interface ITankModelFactory
    {
        ITank Create(Vector2 position, float rotation);
    }
}