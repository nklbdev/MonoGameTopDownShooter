using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Factories
{
    public class HeroFactory
    {
        private readonly IDispatcher<IUpdateable> _updationDispatcher;
        private readonly IDispatcher<IDrawable> _drawingDispatcher;
        private readonly Texture2D _aliveTexture;
        private readonly Texture2D _deadTexture;
        private readonly BulletFactory _bulletFactory;

        public HeroFactory(IDispatcher<IUpdateable> updationDispatcher, IDispatcher<IDrawable> drawingDispatcher, Texture2D aliveTexture, Texture2D deadTexture, BulletFactory bulletFactory)
        {
            _updationDispatcher = updationDispatcher;
            _drawingDispatcher = drawingDispatcher;
            _aliveTexture = aliveTexture;
            _deadTexture = deadTexture;
            _bulletFactory = bulletFactory;
        }

        public Character Create(World world, Vector2 position)
        {
            var hero = new Character(world, position, 0, _aliveTexture, _deadTexture, _bulletFactory);
            _updationDispatcher.Register(hero);
            _drawingDispatcher.Register(hero);
            return hero;
        }
    }
}
