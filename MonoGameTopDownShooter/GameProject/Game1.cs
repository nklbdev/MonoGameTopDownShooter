using System.Diagnostics;
using FarseerPhysics.Dynamics;
using GameProject.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SimplestIocContainer;
using XTiled;

namespace GameProject
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Map _map;
        private Rectangle _mapView;
        private Tank _tank;

        private IGameState _state;
        private IGameStateFactory _stateFactory;


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
            this.IsMouseVisible = true;
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
                .Bind("alive", () => Content.Load<Texture2D>("body"))
                .Bind("dead", () => Content.Load<Texture2D>("tower"))
                .Bind(() => new World(Vector2.Zero));

            _mapView = _graphics.GraphicsDevice.Viewport.Bounds;

            _map = Content.Load<Map>("map");
            foreach (var obj in _map.ObjectLayers["characters"].MapObjects)
            {
                Trace.WriteLine(obj.Bounds);
            }

            _stateFactory = new GameStateFactory(new CharacterFactory(container.Resolve<Texture2D>("alive"), container.Resolve<Texture2D>("dead")));

            _state = _stateFactory.CreateLevelState(Content.Load<Map>("map"));
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _state.Update(elapsedSeconds);

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            //_map.Draw(_spriteBatch, _mapView);

            //var elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _state.Draw(_spriteBatch, gameTime);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
