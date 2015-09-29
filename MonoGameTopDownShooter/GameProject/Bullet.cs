using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject
{
    public class Bullet : IUpdateable, IDrawable
    {
        private readonly Body _body;
        private readonly Texture2D _texture;
        public bool IsAlive { get; private set; }

        public Bullet(World world, Vector2 position, Vector2 velocity, Texture2D texture)
        {
            _body = new Body(world, position, 0, BodyType.Dynamic)
            {
                IsBullet = true
            };
            var fixture = FixtureFactory.AttachCircle(1, 1, _body);
            fixture.IsSensor = true;
            fixture.OnCollision += (a, b, contact) =>
            {
                var heroa = a.UserData as Tank;
                var herob = b.UserData as Tank;
                if (heroa != null)
                {
                    heroa.TakeDamage(100);
                    IsAlive = false;
                    return true;
                }
                if (herob != null)
                {
                    herob.TakeDamage(100);
                    IsAlive = false;
                    return true;
                }
                return false;
            };
            _texture = texture;
            IsAlive = true;
            _body.LinearVelocity = velocity;
        }

        public void Update(float elapsedSeconds)
        {
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(
                _texture,
                new Rectangle(_body.Position.ToPoint(), new Point(10, 10)),
                null,
                Color.White);
        }
    }
}
