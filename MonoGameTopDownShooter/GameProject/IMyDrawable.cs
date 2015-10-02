using Microsoft.Xna.Framework.Graphics;

namespace GameProject
{
    public interface IMyDrawable : IObservableDisposable
    {
        void Draw(SpriteBatch spriteBatch, float elapsedSeconds);
    }
}