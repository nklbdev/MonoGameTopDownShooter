using System;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using GameProject.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject
{
    public class Character : ICharacter, IUpdateable, IDrawable
    {
        private readonly Body _body;
        private readonly World _world;
        public bool IsAlive { get; private set; }
        private readonly Texture2D _aliveTexture;
        private readonly Texture2D _deadTexture;
        private const float _radius = 20;
        private bool _isMovingQueued;
        private Vector2 _direction;
        private const float _speed = 100;
        private readonly BulletFactory _bulletFactory;

        public Character(World world, Vector2 position, float rotation, Texture2D aliveTexture, Texture2D deadTexture, BulletFactory bulletFactory)
        {
            _world = world;
            _aliveTexture = aliveTexture;
            _deadTexture = deadTexture;
            _body = new Body(world, position, 0, BodyType.Dynamic, this) {FixedRotation = true};
            FixtureFactory.AttachCircle(_radius, 1, _body, Vector2.Zero, this);
            IsAlive = true;
            _direction = Vector2.UnitX;
            _bulletFactory = bulletFactory;
        }

        public void Update(float elapsedSeconds)
        {
            if (IsAlive)
                if (_isMovingQueued)
                {
                    _body.LinearVelocity = _direction * _speed;
                    _isMovingQueued = false;
                }
                else
                {
                    _body.LinearVelocity = Vector2.Zero;
                }
        }

        public void Fire()
        {
            _bulletFactory.Create(_world, _body.Position + _direction * 30, _direction*100);
        }

        public void LookAt(Vector2 destination)
        {
            _direction = Vector2.Normalize(destination - _body.Position);
        }

        public void Move(float yaw)
        {
            _isMovingQueued = true;
        }

        public void TurnTo(float rotation)
        {
        }

        public void TakeDamage(float damage)
        {
            IsAlive = false;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            var texture = IsAlive ? _aliveTexture : _deadTexture;
            spriteBatch.Draw(
                texture,
                new Rectangle(_body.Position.ToPoint(), new Point((int) (_radius * 2), (int) (_radius * 2))),
                null,
                Color.White,
                (float) Math.Atan2(_direction.Y, _direction.X),
                new Vector2(texture.Width / 2, texture.Height / 2),
                SpriteEffects.None,
                0);
        }
    }
}
