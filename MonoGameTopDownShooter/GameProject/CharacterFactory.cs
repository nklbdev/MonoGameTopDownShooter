using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XTiled;

namespace GameProject
{
    public class CharacterFactory : ICharacterFactory
    {
        private readonly Texture2D _aliveTexture;
        private readonly Texture2D _deadTexture;

        public CharacterFactory(Texture2D aliveTexture, Texture2D deadTexture)
        {
            _aliveTexture = aliveTexture;
            _deadTexture = deadTexture;
        }

        public ICharacter CreateCharacter(MapObject mapObject, World world)
        {
            var character = new Character(
                world,
                new Vector2(mapObject.Bounds.X, mapObject.Bounds.Y),
                0,
                _aliveTexture,
                _deadTexture,
                null /*bulletFactory*/);
            return character;
        }
    }
}