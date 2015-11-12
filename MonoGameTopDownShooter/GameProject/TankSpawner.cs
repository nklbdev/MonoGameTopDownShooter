using Microsoft.Xna.Framework;

namespace GameProject
{
    public class TankSpawner : ITankSpawner
    {
        //создает системы MVC
        private readonly IEntityCollector<IModel> _modelsCollector;
        private readonly IEntityCollector<IController> _controllersCollector;
        private readonly IEntityCollector<IView> _viewsCollector;

        private readonly IEntityCollector<IView> _worldViewCollector;
        //private readonly IEntityCollector<IView> _hudViewCollector;

        private readonly ITankModelFactory _modelFactory;
        private readonly ITankViewFactory _worldViewFactory;
        //private readonly ITankViewFactory _hudViewFactory;
        private readonly ITankControllerFactory _controllerFactory;

        public TankSpawner(
            IEntityCollector<IModel> modelsCollector,
            IEntityCollector<IController> controllersCollector,
            IEntityCollector<IView> viewsCollector,

            IEntityCollector<IView> worldViewCollector,
            //IEntityCollector<IView> hudViewCollector,
            
            ITankModelFactory modelFactory,
            ITankViewFactory worldViewFactory,
            //IViewFactory hudViewFactory,
            ITankControllerFactory controllerFactory)
        {
            _modelsCollector = modelsCollector;
            _controllersCollector = controllersCollector;
            _viewsCollector = viewsCollector;
            _worldViewCollector = worldViewCollector;
            //_hudViewCollector = hudViewCollector;
            _modelFactory = modelFactory;
            _worldViewFactory = worldViewFactory;
            //_hudViewFactory = hudViewFactory;
            _controllerFactory = controllerFactory;
        }

        public void Spawn(Vector2 position, float rotation)
        {
            var model = _modelFactory.Create(position, rotation);
            _modelsCollector.Collect(model);
            _controllersCollector.Collect(_controllerFactory.Create(model));
            _viewsCollector.Collect(_worldViewFactory.Create(model));
            //_viewsCollector.Collect(_hudViewFactory.Create(model));
            _worldViewCollector.Collect(_worldViewFactory.Create(model));
            //_hudViewCollector.Collect(_hudViewFactory.Create(model));
        }
    }
}