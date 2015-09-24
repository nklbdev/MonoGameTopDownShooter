using System.Collections.Generic;

namespace MonoGameTopDownShooter
{
    class UpdateableDispatcher : IDispatcher<IUpdateable>
    {
        private readonly HashSet<IUpdateable> _updateables = new HashSet<IUpdateable>();

        public void Register(IUpdateable item)
        {
            _updateables.Add(item);
        }

        public void Deregister(IUpdateable item)
        {
            _updateables.Remove(item);
        }

        public IEnumerable<IUpdateable> Items
        {
            get { return _updateables; }
        }
    }
}