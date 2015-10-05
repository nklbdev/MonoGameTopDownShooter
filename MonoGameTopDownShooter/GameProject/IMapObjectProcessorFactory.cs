using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject
{
    public interface IMapObjectProcessorFactory
    {
        IMapObjectProcessor Create(Ticker inputTicker, Ticker logicTicker, Ticker drawingTicker, World world, ContentManager contentManager, SpriteBatch spriteBatch);
    }
}