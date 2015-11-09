using Microsoft.Xna.Framework.Graphics;

namespace GameProject
{
    public interface IMyDrawable
    {
        void Draw(float elapsedSeconds, SpriteBatch spriteBatch);
    }
}