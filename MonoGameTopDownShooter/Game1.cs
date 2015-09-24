using System.Linq;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TiledSharp;

namespace MonoGameTopDownShooter
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private World _world;

        private readonly IDispatcher<IUpdateable> _updateableDispatcher;
        private readonly IDispatcher<IDrawable> _drawableDispatcher;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _updateableDispatcher = new UpdateableDispatcher();
            _drawableDispatcher = new DrawableDispatcher();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            //Trace.WriteLine(Content.RootDirectory);
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _world = new World(Vector2.Zero);

            var texture = new Texture2D(GraphicsDevice, 10, 10);
            texture.SetData(new[] { Color.Black });

            var heroFactory = new HeroFactory(_world, texture, _updateableDispatcher, _drawableDispatcher);

            var map = new TmxMap(Content.RootDirectory + "\\map.tmx");
            foreach (var tmxObject in map.ObjectGroups.SelectMany(tmxObjectGroup => tmxObjectGroup.Objects))
            {
                var hero = heroFactory.Create();
                hero.State.Gist.Position = new Vector2((float) tmxObject.X, (float) tmxObject.Y);
            }
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var elapsedSeconds = (float) gameTime.ElapsedGameTime.TotalSeconds;
            _world.Step(elapsedSeconds);

            foreach (var updatable in _updateableDispatcher.Items)
                updatable.Update(elapsedSeconds);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            foreach (var drawable in _drawableDispatcher.Items)
                drawable.Draw(_spriteBatch, gameTime);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
