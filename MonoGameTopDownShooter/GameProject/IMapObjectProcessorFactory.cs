using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Content;

namespace GameProject
{
    public interface IMapObjectProcessorFactory
    {
        IMapObjectProcessor Create(IMyUpdater updater, IMyDrawer drawer, World world, ContentManager contentManager);
    }
}