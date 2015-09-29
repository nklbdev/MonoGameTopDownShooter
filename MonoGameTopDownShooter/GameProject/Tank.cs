using System;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using GameProject.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject
{
    public class Tank : ITank, IUpdateable, IDrawable
    {
        private readonly Body _body;
        //private readonly World _world;
        private readonly Texture2D _bodyTexture;
        private readonly Texture2D _towerTexture;
        private Vector2 _bodyDirection = Vector2.UnitY;
        private Vector2 _towerDirection = Vector2.UnitX;
        private const float _radius = 20;
        private bool _isMovingQueued;

        private const float _speed = 100;
        private const float _speedWithRotation = 80;
        private const float _rotationSpeed = 5;
        private const float _rotationSpeedWithMoving = 3;

        private readonly BulletFactory _bulletFactory;
        private bool _isMovingQueuedForward;
        private bool _isRotationQueued;
        private bool _isRotationQueuedClockwise;

        public Tank(World world, Vector2 position, float rotation, Texture2D aliveTexture, Texture2D deadTexture, BulletFactory bulletFactory)
        {
            //_world = world;
            _bodyTexture = aliveTexture;
            _towerTexture = deadTexture;
            _body = new Body(world, position, 0, BodyType.Dynamic, this) { FixedRotation = true };
            FixtureFactory.AttachCircle(_radius, 1, _body, Vector2.Zero, this);
            _bulletFactory = bulletFactory;
        }

        public void Update(float elapsedSeconds)
        {
            if (_isMovingQueued)
            {
                if (_isRotationQueued)
                {
                    _body.LinearVelocity = _bodyDirection * _speedWithRotation * (_isMovingQueuedForward ? 1 : -1);
                    _bodyDirection = Vector2.Transform(_bodyDirection, Matrix.CreateRotationZ(_rotationSpeedWithMoving * elapsedSeconds * (_isRotationQueuedClockwise ? 1 : -1)));
                    _isRotationQueued = false;
                }
                else
                {
                    _body.LinearVelocity = _bodyDirection * _speed * (_isMovingQueuedForward ? 1 : -1);
                }
                _isMovingQueued = false;
            }
            else
            {
                _body.LinearVelocity = Vector2.Zero;
                if (!_isRotationQueued) return;
                _bodyDirection = Vector2.Transform(_bodyDirection, Matrix.CreateRotationZ(_rotationSpeed * elapsedSeconds * (_isRotationQueuedClockwise ? 1 : -1)));
                _isRotationQueued = false;
            }
        }

        public void Fire()
        {
            //_bulletFactory.Create(_world, _body.Position + _direction * 30, _direction*100);
        }

        public void RotateTowerTo(Vector2 destination)
        {
            _towerDirection = Vector2.Normalize(destination - _body.Position);
        }

        public void RotateBody(bool isClockWise)
        {
            _isRotationQueued = true;
            _isRotationQueuedClockwise = isClockWise;
        }

        public void Move(bool isForward)
        {
            _isMovingQueued = true;
            _isMovingQueuedForward = isForward;
        }

        public void TakeDamage(float damage)
        {
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(
                _bodyTexture,
                new Rectangle(_body.Position.ToPoint(), new Point((int)(_radius * 2), (int)(_radius * 2))),
                null,
                Color.White,
                (float)Math.Atan2(_bodyDirection.Y, _bodyDirection.X),
                new Vector2(_bodyTexture.Width / 2, _bodyTexture.Height / 2),
                SpriteEffects.None,
                0);

            spriteBatch.Draw(
                _towerTexture,
                new Rectangle(_body.Position.ToPoint(), new Point((int)(_radius * 2), (int)(_radius * 2))),
                null,
                Color.White,
                (float)Math.Atan2(_towerDirection.Y, _towerDirection.X),
                new Vector2(_towerTexture.Width / 2, _towerTexture.Height / 2),
                SpriteEffects.None,
                0);
        }
    }
}
