using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;
using GameProject.Entities.Models.Tanks;
using Microsoft.Xna.Framework;

namespace GameProject.Entities.Models.Bullets
{
    public class SimpleBullet : EntityBase, IBullet
    {
        private readonly Body _body;
        private readonly ITank _ownerTank;
        private readonly World _world;
        private readonly float _damage;

        public SimpleBullet(World world, Vector2 position, float rotation, ITank ownerTank, float damage)
        {
            _world = world;
            _ownerTank = ownerTank;
            _damage = damage;
            _body = new Body(_world, position, rotation, BodyType.Dynamic)
            {
                FixedRotation = true,
                IsBullet = true,
                IsSensor = true,
                UserData = this
            };
            FixtureFactory.AttachCircle(0.1f, 1, _body, Vector2.Zero);
            _body.LinearVelocity = Rotation.ToVector() * 50;
            _body.OnCollision += BodyOnCollision;
        }

        private bool BodyOnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            object other;
            if (fixtureA.Body == _body)
                other = fixtureB.Body.UserData;
            else if (fixtureB.Body == _body)
                other = fixtureA.Body.UserData;
            else
                return false;

            if (other == _ownerTank)
                return false;

            if (other is ITank) (other as ITank).TakeDamage(_damage);

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

        protected override void OnDestroy()
        {
            _body.OnCollision -= BodyOnCollision;
            _world.RemoveBody(_body);
        }
    }
}