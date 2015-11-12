using Microsoft.Xna.Framework;

namespace GameProject
{
    public interface IGameState
    {
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);
    }
}
