using System;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;

namespace MonoGameTopDownShooter
{
    public class Hero : IUpdateable
    {
        public Body Body { get; private set; }

        public Hero(World world, Vector2 position)
        {
            if (world == null)
                throw new ArgumentNullException("world");

            Body = new Body(world, position, 0, BodyType.Dynamic);
            FixtureFactory.AttachCircle(10, 1, Body);

            Body.LinearVelocity = new Vector2(1000, 0);
            Body.OnCollision += BodyOnOnCollision;
        }

        private bool BodyOnOnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            //fixtureA.
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
        }
    }
}
