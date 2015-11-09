using Microsoft.Xna.Framework.Graphics;

namespace GameProject
{
    public interface ILevelLoader
    {
        void LoadLevel(Ticker physicsTicker, Ticker logicTicker, Ticker drawingTicker, SpriteBatch spriteBatch, string resourceName);
    }
}