using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTopDownShooter.HeroStates;
using MonoGameTopDownShooter.HeroStates.Character;
using MonoGameTopDownShooter.HeroStates.Feet;
using MonoGameTopDownShooter.HeroStates.Hands;

namespace MonoGameTopDownShooter
{
    public class HeroFactory
    {
        private readonly World _world;
        private readonly Texture2D _texture;
        private readonly IDispatcher<IUpdateable> _entityDispatcher;
        private readonly IDispatcher<IDrawable> _viewDispatcher;

        public HeroFactory(World world, Texture2D texture, IDispatcher<IUpdateable> entityDispatcher, IDispatcher<IDrawable> viewDispatcher)
        {
            _world = world;
            _texture = texture;
            _entityDispatcher = entityDispatcher;
            _viewDispatcher = viewDispatcher;
        }

        public Hero Create()
        {
            var hero = new Hero();
            _entityDispatcher.Register(hero);
            var heroView = new HeroView(hero, _texture);
            _viewDispatcher.Register(heroView);
            hero.State = new AliveCharacterState(_world, hero, new Stateful<IHands>(), new Stateful<IFeet>());
            return hero;
        }
    }
}
