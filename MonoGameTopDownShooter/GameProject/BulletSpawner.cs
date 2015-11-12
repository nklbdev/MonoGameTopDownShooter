using GameProject.Entities;
using Microsoft.Xna.Framework;

namespace GameProject
{
    public class BulletSpawner : IBulletSpawner
    {
        //создает системы MVC
        private readonly IEntityCollector<IModel> _modelsCollector;
        private readonly IEntityCollector<IView> _viewsCollector;

        private readonly IEntityCollector<IView> _worldViewCollector;

        private readonly IBulletModelFactory _modelFactory;
        private readonly IBulletViewFactory _worldViewFactory;

        public BulletSpawner(
            IEntityCollector<IModel> modelsCollector,
            IEntityCollector<IView> viewsCollector,

            IEntityCollector<IView> worldViewCollector,
            
            IBulletModelFactory modelFactory,
            IBulletViewFactory worldViewFactory)
        {
            _modelsCollector = modelsCollector;
            _viewsCollector = viewsCollector;
            _worldViewCollector = worldViewCollector;
            _modelFactory = modelFactory;
            _worldViewFactory = worldViewFactory;
        }

        public void Spawn(Vector2 position, float rotation, ITank ownerTank)
        {
            var model = _modelFactory.Create(position, rotation, ownerTank);
            _modelsCollector.Collect(model);
            _viewsCollector.Collect(_worldViewFactory.Create(model));
            _worldViewCollector.Collect(_worldViewFactory.Create(model));
        }
    }
}