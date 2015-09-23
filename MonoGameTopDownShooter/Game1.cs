using System.Collections.Generic;
using System.Diagnostics;
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

        private readonly List<IDrawable> _drawables = new List<IDrawable>();
        private readonly List<IUpdateable> _updateables = new List<IUpdateable>();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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

            var map = new TmxMap(Content.RootDirectory + "\\map.tmx");
            foreach (var tmxObjectGroup in map.ObjectGroups)
            {
                foreach (var tmxObject in tmxObjectGroup.Objects)
                {
                    var hero = new Hero(_world, new Vector2((float) tmxObject.X, (float) tmxObject.Y));
                    var heroView = new HeroView(hero, texture);
                    _updateables.Add(hero);
                    _drawables.Add(heroView);
                }
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

            _world.Step((float) gameTime.ElapsedGameTime.TotalSeconds);

            foreach (var updatable in _updateables)
                updatable.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            foreach (var drawable in _drawables)
                drawable.Draw(_spriteBatch, gameTime);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
