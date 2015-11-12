using System;
using System.Collections.Generic;
using GameProject.Entities.Views;
using Microsoft.Xna.Framework;
using MonoGameProxies;

namespace GameProject.Infrastructure
{
    public class Camera : IDrawable
    {
        private readonly ISpriteBatch _spriteBatch;
        private Rectangle _boundingRectangle;

        public ICollection<EntityProcessor<IView>> ViewProcessors { get; set; }

        public Camera(ISpriteBatch spriteBatch)
        {
            if (spriteBatch == null)
                throw new ArgumentNullException("spriteBatch");
            _spriteBatch = spriteBatch;
            _boundingRectangle = new Rectangle(0, 0, 1024, 640);
        }

        public void Draw()
        {
            if (ViewProcessors == null || ViewProcessors.Count <= 0)
                return;
            _spriteBatch.Begin();
            foreach (var processor in ViewProcessors)
                processor.Process(v => v.Render(_spriteBatch, ref _boundingRectangle));
            _spriteBatch.End();
        }
    }
}