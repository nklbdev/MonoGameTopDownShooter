using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using GameProject.Entities.Bullets;
using GameProject.Views;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Spawners
{
    public class BulletSpawner : ISpawner
    {
        private readonly Ticker _logicTicker;
        private readonly Ticker _drawingTicker;
        private readonly World _world;
        private readonly Texture2D _texture;
        private readonly SpriteBatch _spriteBatch;
        private readonly Body _ignoredBody;

        public BulletSpawner(Ticker logicTicker, Ticker drawingTicker, World world, Texture2D texture, SpriteBatch spriteBatch, Body ignoredBody)
        {
            _logicTicker = logicTicker;
            _drawingTicker = drawingTicker;
            _world = world;
            _texture = texture;
            _spriteBatch = spriteBatch;
            _ignoredBody = ignoredBody;
        }

        public void Spawn(Vector2 position, float rotation)
        {
            var body = new Body(_world, position, rotation, BodyType.Dynamic, this) { FixedRotation = true, IsBullet = true };
            FixtureFactory.AttachCircle(0.2f, 1, body, Vector2.Zero, this);
            body.IgnoreCollisionWith(_ignoredBody);

            var bullet = new SimpleBullet(body) { Ticker = _logicTicker };
            new BulletView(bullet, _texture, _spriteBatch).Ticker = _drawingTicker;
        }
    }
}