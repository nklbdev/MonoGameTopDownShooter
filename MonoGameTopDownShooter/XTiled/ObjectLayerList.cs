using System.Collections.Generic;
using System.Linq;

namespace XTiled
{
    public class ObjectLayerList : List<ObjectLayer>
    {
        public ObjectLayer this[string name]
        {
            get
            {
                return this.FirstOrDefault(x => x.Name.Equals(name));
            }
        }
    }
}