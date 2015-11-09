using Microsoft.Xna.Framework;
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

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _graphics.PreferredBackBufferWidth = 1024;
            _graphics.PreferredBackBufferHeight = 640;
            _graphics.IsFullScreen = true;
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
        private Ticker _physicsTicker;
        private Ticker _logicTicker;
        private Ticker _drawingTicker;
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _physicsTicker = new Ticker();
            _logicTicker = new Ticker();
            _drawingTicker = new Ticker();

            var container = new Container()
                .Bind<ILevelLoader>(c => new LevelLoader(Content, c.Resolve<IMapObjectProcessorFactory>()))
                .Bind<IMapObjectProcessorFactory>(c => new MapObjectProcessorFactory());

            container.Resolve<ILevelLoader>().LoadLevel(_physicsTicker, _logicTicker, _drawingTicker, _spriteBatch, "01");
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
            _logicTicker.Tick(gameTime);
            _physicsTicker.Tick(gameTime);
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
            _drawingTicker.Tick(gameTime);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
