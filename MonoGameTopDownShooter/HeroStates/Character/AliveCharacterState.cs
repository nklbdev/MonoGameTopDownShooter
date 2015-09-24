using System;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameTopDownShooter.HeroStates.Feet;
using MonoGameTopDownShooter.HeroStates.Hands;

namespace MonoGameTopDownShooter.HeroStates.Character
{
    public class AliveCharacterState : State<ICharacter>, ICharacter
    {
        private Body _body;
        private readonly World _world;
        private readonly Stateful<ICharacter> _owner;
        private readonly Stateful<IHands> _hands;
        private readonly Stateful<IFeet> _feet;

        public AliveCharacterState(World world, Stateful<ICharacter> owner, Stateful<IHands> hands, Stateful<IFeet> feet)
        {
            if (owner == null)
                throw new ArgumentNullException("owner");
            if (world == null)
                throw new ArgumentNullException("world");
            if (hands == null)
                throw new ArgumentNullException("hands");
            if (feet == null)
                throw new ArgumentNullException("feet");
            _owner = owner;
            _world = world;
            _hands = hands;
            _feet = feet;
        }

        public void Die()
        {
            _hands.State.Gist.Drop();
            _owner.State = new DeadCharacterState();
        }

        public void Fire()
        {
            _hands.State.Gist.Fire();
        }

        public void Drop()
        {
            _hands.State.Gist.Drop();
        }

        public void TurnTo(float rotation)
        {
            _feet.State.Gist.TurnTo(rotation);
        }

        public void Crouch()
        {
            _feet.State.Gist.Crouch();
        }

        public void Run()
        {
            _feet.State.Gist.Run();
        }

        public void Walk()
        {
            _feet.State.Gist.Walk();
        }

        public void Stop()
        {
            _feet.State.Gist.Stop();
        }

        public void Move(float yaw)
        {
            _feet.State.Gist.Move(yaw);
        }

        public override ICharacter Gist { get { return this; } }

        public override void Bring()
        {
            base.Bring();
            _body = new Body(_world, Vector2.Zero, 0, BodyType.Dynamic);
            FixtureFactory.AttachCircle(10, 1, _body);
        }

        public override void Leave()
        {
            base.Leave();
            _body.Dispose();
            _body = null;
        }

        public override float Update(float elapsedSeconds)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W))
                _body.LinearVelocity += new Vector2(-10, 0);

            if (Keyboard.GetState().IsKeyDown(Keys.D))
                _owner.State = new DeadCharacterState { Position = Position, Rotation = Rotation };

            return elapsedSeconds;
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
    }
}
