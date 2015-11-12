using Microsoft.Xna.Framework;
using MonoGameProxies;

namespace GameProject
{
    public interface IRenderable
    {
        void Render(IImmutableSpriteBatch spriteBatch, ref Rectangle boundingRectangle);
    }
}