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
        private Ticker _inputTicker;
        private Ticker _logicTicker;
        private Ticker _drawingTicker;
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _physicsTicker = new Ticker();
            _inputTicker = new Ticker();
            _logicTicker = new Ticker();
            _drawingTicker = new Ticker();

            var container = new Container()
                .Bind<ILevelLoader>(c => new LevelLoader(Content, c.Resolve<IMapObjectProcessorFactory>()))
                .Bind<IMapObjectProcessorFactory>(c => new MapObjectProcessorFactory());

            container.Resolve<ILevelLoader>().LoadLevel(_physicsTicker, _inputTicker, _logicTicker, _drawingTicker, _spriteBatch, "01");
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
            _inputTicker.Tick(gameTime);
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

            //_spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend,
            //    null,
            //    null,
            //    null,
            //    null,
            //    _worldCamera.Matrix);
            _spriteBatch.Begin();
            _drawingTicker.Tick(gameTime);
            _spriteBatch.End();

            //

            base.Draw(gameTime);
        }

        //private Camera _hudCamera;
        //private Camera _worldCamera;
    }

    //public class Camera
    //{
    //    public Matrix Matrix { get; set; }


    //    private float _zoom; // Camera Zoom
    //    private Matrix _transform; // Matrix Transform
    //    private Vector2 _position; // Camera Position
    //    private float _rotation; // Camera Rotation

    //    public Camera()
    //    {
    //        _zoom = 1.0f;
    //        _rotation = 0.0f;
    //        _position = Vector2.Zero;
    //    }

    //    // Sets and gets zoom
    //    public float Zoom
    //    {
    //        get { return _zoom; }
    //        set { _zoom = value; if (_zoom < 0.1f) _zoom = 0.1f; } // Negative zoom will flip image
    //    }

    //    public float Rotation
    //    {
    //        get { return _rotation; }
    //        set { _rotation = value; }
    //    }

    //    // Auxiliary function to move the camera
    //    public void Move(Vector2 amount)
    //    {
    //        _position += amount;
    //    }

    //    // Get set position
    //    public Vector2 Position
    //    {
    //        get { return _position; }
    //        set { _position = value; }
    //    }

    //    public Matrix get_transformation(GraphicsDevice graphicsDevice)
    //    {
    //        _transform =       // Thanks to o KB o for this solution
    //          Matrix.CreateTranslation(new Vector3(-_position.X, -_position.Y, 0)) *
    //                                     Matrix.CreateRotationZ(Rotation) *
    //                                     Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
    //                                     Matrix.CreateTranslation(new Vector3(ViewportWidth * 0.5f, ViewportHeight * 0.5f, 0));
    //        return _transform;
    //    }
    //}
}
