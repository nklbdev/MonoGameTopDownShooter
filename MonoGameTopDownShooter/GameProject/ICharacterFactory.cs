using FarseerPhysics.Dynamics;
using XTiled;

namespace GameProject
{
    internal interface ICharacterFactory
    {
        ITank CreateCharacter(MapObject mapObject, World world);
    }
}