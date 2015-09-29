using FarseerPhysics.Dynamics;
using XTiled;

namespace GameProject
{
    internal interface ICharacterFactory
    {
        ICharacter CreateCharacter(MapObject mapObject, World world);
    }
}