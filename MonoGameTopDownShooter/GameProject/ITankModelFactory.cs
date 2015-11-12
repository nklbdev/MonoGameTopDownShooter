using GameProject.Entities;
using Microsoft.Xna.Framework;

namespace GameProject
{
    public interface ITankModelFactory
    {
        ITank Create(Vector2 position, float rotation);
    }
}