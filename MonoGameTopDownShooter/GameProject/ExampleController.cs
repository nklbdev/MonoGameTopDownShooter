using System;
using System.Diagnostics;

namespace GameProject
{
    public class ExampleController : NewEntityBase, IController
    {
        private readonly IModel _model;

        public ExampleController(IModel model)
        {
            if (model == null)
                throw new ArgumentNullException("model");
            if (model.IsDestroyed)
                throw new InvalidOperationException("Cannot create model controller for destroyed model");
            _model = model;
            _model.Destroyed += ModelOnDestroyed;
        }

        private void ModelOnDestroyed(INewEntity entity)
        {
            if (entity != _model)
                return;
            Destroy();
        }

        public override void OnDestroy()
        {
            _model.Destroyed -= ModelOnDestroyed;
        }

        public override void OnUpdate(float deltaTime)
        {
            Trace.WriteLine("ExampleController.Update");
            //_model.Destroy();
        }
    }
}