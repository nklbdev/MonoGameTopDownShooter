using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Factories
{
    public class BulletFactory
    {
        private readonly IDispatcher<IUpdateable> _updationDispatcher;
        private readonly IDispatcher<IDrawable> _drawingDispatcher;
        private readonly Texture2D _deadTexture;

        public BulletFactory(IDispatcher<IUpdateable> updationDispatcher, IDispatcher<IDrawable> drawingDispatcher, Texture2D deadTexture)
        {
            _updationDispatcher = updationDispatcher;
            _drawingDispatcher = drawingDispatcher;
            _deadTexture = deadTexture;
        }

        public Bullet Create(World world, Vector2 position, Vector2 velocity)
        {
            var bullet = new Bullet(world, position, velocity, _deadTexture);
            _updationDispatcher.Register(bullet);
            _drawingDispatcher.Register(bullet);
            return bullet;
        }
    }
}