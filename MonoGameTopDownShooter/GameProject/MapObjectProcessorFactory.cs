using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject
{
    public class MapObjectProcessorFactory : IMapObjectProcessorFactory
    {
        public IMapObjectProcessor Create(Ticker inputTicker, Ticker logicTicker, Ticker drawingTicker, World world, ContentManager contentManager, SpriteBatch spriteBatch)
        {
            return new MapObjectProcessor(inputTicker, logicTicker, drawingTicker, world, contentManager, spriteBatch);
        }
    }
}