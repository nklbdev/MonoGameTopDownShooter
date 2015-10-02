using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XTiled;

namespace GameProject
{
    public class MapObjectProcessor : IMapObjectProcessor
    {
        private readonly World _world;
        private readonly IMyUpdater _updater;
        private readonly IMyDrawer _drawer;
        private readonly Texture2D _bodyTexture;
        private readonly Texture2D _towerTexture;

        public MapObjectProcessor(IMyUpdater updater, IMyDrawer drawer, World world, ContentManager contentManager)
        {
            _world = world;
            _updater = updater;
            _drawer = drawer;
            _bodyTexture = contentManager.Load<Texture2D>("body");
            _towerTexture = contentManager.Load<Texture2D>("tower");
        }

        public void Process(MapObject mapObject)
        {
            if (mapObject.Type == "Player")
            {
                var tank = new Tank(_world, new Vector2(mapObject.Bounds.X, mapObject.Bounds.Y), 0);
                var tankController = new CharacterController(tank);
                var tankView = new TankView(tank, _bodyTexture, _towerTexture);

                _updater.AddUpdateable(tankController);
                _updater.AddUpdateable(tank);
                _drawer.AddDrawable(tankView);
            }
            else if (mapObject.Type == "Enemy")
            {
                var tank = new Tank(_world, new Vector2(mapObject.Bounds.X, mapObject.Bounds.Y), 0);
                //var tankController = new CharacterController(tank);
                var tankView = new TankView(tank, _bodyTexture, _towerTexture);

                //_updater.AddUpdateable(tankController);
                _updater.AddUpdateable(tank);
                _drawer.AddDrawable(tankView);
            }
        }
    }
}