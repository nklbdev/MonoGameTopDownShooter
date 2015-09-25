using FarseerPhysics.Dynamics;

namespace MonoGameTopDownShooter
{
    public class HeroFactory
    {
        private readonly World _world;
        private readonly IDispatcher<IUpdateable> _updationDispatcher;
        private readonly IDispatcher<IDrawable> _drawingDispatcher;

        private readonly ICharacterStateFactory _characterStateFactory;

        public HeroFactory(World world, IDispatcher<IUpdateable> updationDispatcher, IDispatcher<IDrawable> drawingDispatcher, ICharacterStateFactory characterStateFactory)
        {
            _world = world;
            _updationDispatcher = updationDispatcher;
            _drawingDispatcher = drawingDispatcher;
            _characterStateFactory = characterStateFactory;
        }

        public Hero Create()
        {

            var hero = new Hero();
            hero.State = _characterStateFactory.CreateAliveState(hero);


            _updationDispatcher.Register(hero);
            _drawingDispatcher.Register(hero);
            return hero;
        }
    }
}
