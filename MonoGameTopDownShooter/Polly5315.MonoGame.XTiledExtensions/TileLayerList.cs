using System.Collections.Generic;
using System.Linq;

namespace Polly.MonoGame.XTiledExtensions
{
    public class TileLayerList : List<TileLayer>
    {
        public TileLayer this[string name]
        {
            get
            {
                return this.FirstOrDefault(x => x.Name.Equals(name));
            }
        }
    }
}