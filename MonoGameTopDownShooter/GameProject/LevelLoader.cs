using System.Linq;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using XTiled;

namespace GameProject
{
    public class LevelLoader : ILevelLoader
    {
        private readonly ContentManager _contentManager;
        private readonly IMyUpdaterFactory _updaterFactory;
        private readonly IMyDrawerFactory _drawerrFactory;
        private readonly IMapObjectProcessorFactory _mapObjectProcessorFactory;

        public LevelLoader(ContentManager contentManager, IMyUpdaterFactory updaterFactory, IMyDrawerFactory drawerrFactory, IMapObjectProcessorFactory mapObjectProcessorFactory)
        {
            _contentManager = contentManager;
            _updaterFactory = updaterFactory;
            _drawerrFactory = drawerrFactory;
            _mapObjectProcessorFactory = mapObjectProcessorFactory;
        }

        public IMyComponent LoadLevel(string resourceName)
        {
            var updater = _updaterFactory.Create();
            var drawer = _drawerrFactory.Create();

            var map = _contentManager.Load<Map>(resourceName);
            var world = new World(Vector2.Zero);
            updater.AddUpdateable(new PhysicsWorldUpdater(world));
            var mapObjectProcessor = _mapObjectProcessorFactory.Create(updater, drawer, world, _contentManager);

            foreach (var mapObject in map.ObjectLayers.SelectMany(layer => layer.MapObjects))
                mapObjectProcessor.Process(mapObject);

            var context = new CustomMyComponent(updater, drawer);
            return context;
        }
    }
}