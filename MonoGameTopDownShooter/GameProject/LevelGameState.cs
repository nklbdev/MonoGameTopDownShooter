using System;
using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XTiled;

namespace GameProject
{
    public class LevelGameState : IGameState
    {
        private readonly World _world;
        private readonly Map _map;
        private readonly HashSet<ICharacter> _characters = new HashSet<ICharacter>();
        private readonly HashSet<CharacterController> _characterControllers = new HashSet<CharacterController>();

        public LevelGameState(World world, Map map)
        {
            if (world == null)
                throw new ArgumentNullException("world");
            if (map == null)
                throw new ArgumentNullException("map");
            _world = world;
            _map = map;
        }

        public void AddCharacter(ICharacter character)
        {
            _characters.Add(character);
        }

        public void RemoveCharacter(ICharacter character)
        {
            _characters.Remove(character);
        }

        public void Update(float elapsedSeconds)
        {
            _world.Step(elapsedSeconds);
            foreach (var controller in _characterControllers)
                controller.Update(elapsedSeconds);
            foreach (var character in _characters)
                character.Update(elapsedSeconds);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            _map.Draw(spriteBatch, new Rectangle(0, 0, 500, 500));
            foreach (var character in _characters)
                character.Draw(spriteBatch, gameTime);
        }

        public void AddController(CharacterController controller)
        {
            _characterControllers.Add(controller);
        }
    }
}