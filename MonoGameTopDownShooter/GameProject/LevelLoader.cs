using System.Linq;
using FarseerPhysics.Dynamics;
using GameProject.Views;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XTiled;

namespace GameProject
{
    public class LevelLoader : ILevelLoader
    {
        private readonly ContentManager _contentManager;
        private readonly IMapObjectProcessorFactory _mapObjectProcessorFactory;

        public LevelLoader(ContentManager contentManager, IMapObjectProcessorFactory mapObjectProcessorFactory)
        {
            _contentManager = contentManager;
            _mapObjectProcessorFactory = mapObjectProcessorFactory;
        }

        public void LoadLevel(Ticker physicsTicker, Ticker inputTicker, Ticker logicTicker, Ticker drawingTicker, SpriteBatch spriteBatch, string resourceName)
        {
            var map = _contentManager.Load<Map>(resourceName);

            //new BackgroundView(_contentManager.Load<Texture2D>("grass"), spriteBatch) { Ticker = drawingTicker };
            new MapView(map, spriteBatch) { Ticker = drawingTicker };

            var world = new World(Vector2.Zero);
            new PhysicsWorldUpdater(world).Ticker = physicsTicker;

            var mapObjectProcessor = _mapObjectProcessorFactory.Create(inputTicker, logicTicker, drawingTicker, world, _contentManager, spriteBatch);

            foreach (var mapObject in map.ObjectLayers.SelectMany(layer => layer.MapObjects))
                mapObjectProcessor.Process(mapObject);
        }
    }
}