using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework;

namespace GameProject.Entities.Bullets
{
    public class SimpleBullet : Entity, IBullet
    {
        private readonly Body _body;

        public SimpleBullet(Body body)
        {
            _body = body;
            _body.LinearVelocity = Rotation.ToVector() * 50;
            _body.OnCollision += BodyOnCollision;
        }

        private bool BodyOnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureA.Body.UserData is IEntity)
                (fixtureA.Body.UserData as IEntity).Dispose();
            if (fixtureB.Body.UserData is IEntity)
                (fixtureB.Body.UserData as IEntity).Dispose();

            Dispose();
            return true;
        }

        public Vector2 Position
        {
            get { return _body.Position; }
            set { _body.Position = value; }
        }

        public float Rotation
        {
            get { return _body.Rotation; }
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Dispose()
        {
            _body.OnCollision -= BodyOnCollision;
            _body.Dispose();
            base.Dispose();
        }
    }
}