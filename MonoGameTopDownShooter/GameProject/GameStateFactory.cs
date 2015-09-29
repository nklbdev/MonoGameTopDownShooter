using System;
using System.Linq;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using XTiled;

namespace GameProject
{
    class GameStateFactory : IGameStateFactory
    {
        private readonly ICharacterFactory _characterFactory;

        public GameStateFactory(ICharacterFactory characterFactory)
        {
            if (characterFactory == null)
                throw new ArgumentNullException("characterFactory");
            _characterFactory = characterFactory;
        }

        public IGameState CreateLevelState(Map map)
        {
            if (map == null)
                throw new ArgumentNullException("map");
            var world = new World(Vector2.Zero);

            var state = new LevelGameState(world, map);

            var first = true;
            foreach (var obj in map.ObjectLayers["characters"].MapObjects)
            {
                var character = _characterFactory.CreateCharacter(obj, world);
                state.AddCharacter(character);
                if (first)
                {
                    var controller = new CharacterController(character);
                    state.AddController(controller);
                    first = false;
                }
            }

            var wallBody = new Body(world, Vector2.Zero);
            foreach (var obj in map.ObjectLayers["walls"].MapObjects)
                FixtureFactory.AttachChainShape(
                    new Vertices(obj.Polyline.Points.Select(p => new Vector2(p.X, p.Y))),
                    wallBody);

            return state;
        }
    }
}