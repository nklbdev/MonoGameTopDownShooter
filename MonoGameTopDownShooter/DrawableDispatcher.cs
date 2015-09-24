using System.Collections.Generic;

namespace MonoGameTopDownShooter
{
    class DrawableDispatcher : IDispatcher<IDrawable>
    {
        private readonly HashSet<IDrawable> _updateables = new HashSet<IDrawable>();

        public void Register(IDrawable item)
        {
            _updateables.Add(item);
        }

        public void Deregister(IDrawable item)
        {
            _updateables.Remove(item);
        }

        public IEnumerable<IDrawable> Items
        {
            get { return _updateables; }
        }
    }
}