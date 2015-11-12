using System.Collections.Generic;

namespace GameProject
{
    public class UpdationMultiplexer : IUpdateable
    {
        public UpdationMultiplexer()
        {
            Updateables = new List<IUpdateable>();
        }

        public ICollection<IUpdateable> Updateables { get; private set; }

        public void Update(float deltaTime)
        {
            foreach (var updateable in Updateables)
                updateable.Update(deltaTime);
        }
    }
}