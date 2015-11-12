using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;

namespace GameProject.Entities.Bullets
{
    public class SimpleBullet : EntityBase, IBullet
    {
        private readonly Body _body;
        private readonly ITank _ownerTank;
        private readonly World _world;

        public SimpleBullet(World world, Vector2 position, float rotation, ITank ownerTank)
        {
            _world = world;
            _ownerTank = ownerTank;
            _body = new Body(_world, position, rotation, BodyType.Dynamic)
            {
                FixedRotation = true,
                IsBullet = true,
                IsSensor = true
            };
            FixtureFactory.AttachCircle(0.1f, 1, _body, Vector2.Zero);
            _body.LinearVelocity = Rotation.ToVector() * 50;
            _body.OnCollision += BodyOnCollision;
        }

        private bool BodyOnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            var userDataA = fixtureA.Body.UserData;
            var userDataB = fixtureB.Body.UserData;

            if (userDataA == _ownerTank || userDataB == _ownerTank)
                return false;

            if (userDataA is IEntity)
                (userDataA as IEntity).Destroy();
            if (userDataB is IEntity)
                (userDataB as IEntity).Destroy();

            Destroy();
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

        protected override void OnUpdate(float deltaTime)
        {
        }

        protected override void OnDestroy()
        {
            _body.OnCollision -= BodyOnCollision;
            _world.RemoveBody(_body);
        }
    }
}