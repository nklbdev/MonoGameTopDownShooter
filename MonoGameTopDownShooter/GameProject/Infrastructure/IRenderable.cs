using Microsoft.Xna.Framework;
using MonoGameProxies;

namespace GameProject.Infrastructure
{
    public interface IRenderable
    {
        void Render(IImmutableSpriteBatch spriteBatch, ref Rectangle boundingRectangle);
    }
}