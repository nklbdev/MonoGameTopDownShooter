using Microsoft.Xna.Framework;

namespace GameProject
{
    public interface IScreen
    {
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);
    }
}
