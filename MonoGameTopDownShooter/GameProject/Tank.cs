using System;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject
{
    public class MyUpdaterFactory : IMyUpdaterFactory
    {
        public IMyUpdater Create()
        {
            return new MyUpdater();
        }
    }

    public class MyDrawerFactory : IMyDrawerFactory
    {
        public IMyDrawer Create()
        {
            return new MyDrawer();
        }
    }

    public class Tank : ITank
    {
        private readonly Body _body;
        private Vector2 _bodyDirection = Vector2.UnitY;
        private Vector2 _towerDirection = Vector2.UnitX;
        private const float _radius = 20;
        private bool _isMovingQueued;

        private const float _speed = 100;
        private const float _speedWithRotation = 80;
        private const float _rotationSpeed = 5;
        private const float _rotationSpeedWithMoving = 3;

        private bool _isMovingQueuedForward;
        private bool _isRotationQueued;
        private bool _isRotationQueuedClockwise;

        public Tank(World world, Vector2 position, float rotation)
        {
            _body = new Body(world, position, rotation, BodyType.Dynamic, this) { FixedRotation = true };
            FixtureFactory.AttachCircle(_radius, 1, _body, Vector2.Zero, this);
        }

        public Vector2 Position
        {
            get { return _body.Position; }
            set { _body.Position = value; }
        }

        public float Rotation
        {
            get { return _body.Rotation; }
            set { _body.Rotation = value; }
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
            Dispose();
        }

        public void TakeDamage(float damage)
        {
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
        }

        public void Dispose()
        {
            if (IsDisposed)
                return;
            IsDisposed = true;
            if (Disposed != null)
                Disposed(this);
        }

        public bool IsDisposed { get; private set; }
        public event Action<IObservableDisposable> Disposed;
    }
}
