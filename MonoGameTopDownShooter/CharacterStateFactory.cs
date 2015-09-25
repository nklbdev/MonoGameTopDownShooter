using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTopDownShooter.HeroStates.CharacterStates;
using MonoGameTopDownShooter.HeroStates.VehicleStates;
using MonoGameTopDownShooter.HeroStates.WeaponStates;
using MonoGameTopDownShooter.States;

namespace MonoGameTopDownShooter
{
    class CharacterStateFactory : ICharacterStateFactory
    {
        private readonly Texture2D _texture;
        private readonly World _world;

        public CharacterStateFactory(GraphicsDevice graphicsDevice, World world)
        {
            _texture = new Texture2D(graphicsDevice, 1, 1);
            _texture.SetData(new[] { Color.White });// fill the texture with white

            _world = world;
        }

        public IState<ICharacter> CreateAliveState(ICharacter character)
        {
            var body = new Body(_world, Vector2.Zero, 0, BodyType.Dynamic);
            FixtureFactory.AttachCircle(50, 1, body);

            //todo: remove this test handler
            body.OnCollision += (a, b, contact) => { character.Die(); return true; };

            return CreateAliveState(character, body);
        }

        public IState<ICharacter> CreateDeadState(ICharacter character)
        {
            return new DeadCharacterState(character, _texture) { Position = character.Position, Rotation = character.Rotation };
        }

        public IState<ICharacter> CreateAliveState(ICharacter character, Body body)
        {
            return new AliveCharacterState(character, new Stateful<IWeapon> { State = new PistolWeaponState(character, _texture) }, new Stateful<IVehicle> { State = new StayingVehicleState(character, body) }, _texture, this);
        }

        public IState<ICharacter> CreateDeadState(IState<ICharacter> state, ICharacter character)
        {
            return new DeadCharacterState(character, _texture) { Position = character.Position, Rotation = character.Rotation };
        }
    }
}