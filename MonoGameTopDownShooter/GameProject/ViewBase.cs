using System;
using Microsoft.Xna.Framework;
using MonoGameProxies;

namespace GameProject
{
    public abstract class ViewBase : EntityBase, IView
    {
        public void Render(IImmutableSpriteBatch spriteBatch, ref Rectangle boundingRectangle)
        {
            if (IsDestroyed)
                throw new InvalidOperationException("Cannot render destroyed view");
            OnRender(spriteBatch, ref boundingRectangle);
        }

        public virtual void OnRender(IImmutableSpriteBatch spriteBatch, ref Rectangle boundingRectangle)
        { }
    }
}