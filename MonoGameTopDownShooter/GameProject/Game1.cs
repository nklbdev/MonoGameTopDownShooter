using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SimplestIocContainer;

namespace GameProject
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private IMyComponent _myComponent;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            IsMouseVisible = true;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            var container = new Container()
                .Bind<ILevelLoader>(c => new LevelLoader(Content, c.Resolve<IMyUpdaterFactory>(), c.Resolve<IMyDrawerFactory>(), c.Resolve<IMapObjectProcessorFactory>()))
                .Bind<IMyUpdaterFactory>(c => new MyUpdaterFactory())
                .Bind<IMyDrawerFactory>(c => new MyDrawerFactory())
                .Bind<IMapObjectProcessorFactory>(c => new MapObjectProcessorFactory());

            _myComponent = container.Resolve<ILevelLoader>().LoadLevel("map");
        }


        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            _myComponent.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _myComponent.Draw(_spriteBatch, (float)gameTime.ElapsedGameTime.TotalSeconds);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }

    public class MapObjectProcessorFactory : IMapObjectProcessorFactory
    {
        public IMapObjectProcessor Create(IMyUpdater updater, IMyDrawer drawer, World world, ContentManager contentManager)
        {
            return new MapObjectProcessor(updater, drawer, world, contentManager);
        }
    }
}
