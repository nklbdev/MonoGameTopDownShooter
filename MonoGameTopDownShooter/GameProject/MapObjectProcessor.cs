using System.Linq;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using GameProject.Entities;
using GameProject.Spawners;
using GameProject.Views;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XTiled;

namespace GameProject
{
    public class MapObjectProcessor : IMapObjectProcessor
    {
        private readonly World _world;
        private readonly Ticker _inputTicker;
        private readonly Ticker _logicTicker;
        private readonly Ticker _drawingTicker;
        private readonly Texture2D _bulletTexture;
        private readonly Texture2D _bodyTexture;
        private readonly Texture2D _towerTexture;
        private readonly SpriteBatch _spriteBatch;

        public MapObjectProcessor(Ticker inputTicker, Ticker logicTicker, Ticker drawingTicker, World world, ContentManager contentManager, SpriteBatch spriteBatch)
        {
            _world = world;
            _inputTicker = inputTicker;
            _logicTicker = logicTicker;
            _drawingTicker = drawingTicker;
            _bodyTexture = contentManager.Load<Texture2D>("body");
            _towerTexture = contentManager.Load<Texture2D>("tower");
            _bulletTexture = contentManager.Load<Texture2D>("bullet");
            _spriteBatch = spriteBatch;
        }

        public void Process(MapObject mapObject)
        {
            if (mapObject.Type == "Wall")
            {
                //var body = new Body(_world, new Vector2((mapObject.Bounds.X - 32) / 10f, (mapObject.Bounds.Y - 32)/10f), 0, BodyType.Static, this) { FixedRotation = true };
                var body = new Body(_world, new Vector2((mapObject.Bounds.X - 48) / 10f, (mapObject.Bounds.Y - 48)/10f), 0, BodyType.Static, this) { FixedRotation = true };
                var vertices = new Vertices(mapObject.Polyline.Points.Select(p => new Vector2(p.X/10f, p.Y/10f)));
                FixtureFactory.AttachChainShape(vertices, body);
            }
            if (mapObject.Type == "Player")
            {
                var body = new Body(_world, new Vector2(mapObject.Bounds.X / 10f, mapObject.Bounds.Y / 10f), 0, BodyType.Dynamic, this) { FixedRotation = true };
                FixtureFactory.AttachCircle(2.4f, 1, body, Vector2.Zero, this);

                var tankBody = new TankBody(body);
                var tankTower = new TankTower(tankBody, new BulletSpawner(_logicTicker, _drawingTicker, _world, _bulletTexture, _spriteBatch, body)) { AimingSpeed = 3 };

                var tank = new Tank(tankBody, tankTower) { Ticker = _logicTicker };
                body.UserData = tank;
                new UserTankController(tank).Ticker = _logicTicker;
                new TankView(tank, _bodyTexture, _towerTexture, _spriteBatch).Ticker = _drawingTicker;
            }
            else if (mapObject.Type == "Enemy")
            {
                var body = new Body(_world, new Vector2(mapObject.Bounds.X / 10f, mapObject.Bounds.Y / 10f), 0, BodyType.Dynamic, this) { FixedRotation = true };
                FixtureFactory.AttachCircle(2.4f, 1, body, Vector2.Zero, this);

                var tankBody = new TankBody(body);
                var tankTower = new TankTower(tankBody, new BulletSpawner(_logicTicker, _drawingTicker, _world, _bulletTexture, _spriteBatch, body)) { AimingSpeed = 3 };

                var tank = new Tank(tankBody, tankTower) { Ticker = _logicTicker };
                body.UserData = tank;
                new TankView(tank, _bodyTexture, _towerTexture, _spriteBatch).Ticker = _drawingTicker;
            }
        }
    }
}