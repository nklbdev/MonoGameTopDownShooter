using System.Collections.Generic;

namespace GameProject.Infrastructure
{
    public class DrawingMultiplexer : IDrawable
    {
        public DrawingMultiplexer()
        {
            Drawables = new List<IDrawable>();
        }

        public ICollection<IDrawable> Drawables { get; private set; }

        public void Draw()
        {
            foreach (var drawable in Drawables)
                drawable.Draw();
        }
    }
}