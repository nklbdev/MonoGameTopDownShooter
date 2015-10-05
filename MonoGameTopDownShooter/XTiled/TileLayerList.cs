using System.Collections.Generic;
using System.Linq;

namespace XTiled
{
    /// <summary>
    /// List of TileLayers, indexable by id or name
    /// </summary>
    public class TileLayerList : List<TileLayer>
    {
        /// <summary>
        /// Gets the layer with the given name; read-only propery.
        /// </summary>
        /// <param name="name">Name of the layer</param>
        public TileLayer this[string name]
        {
            get
            {
                return this.FirstOrDefault(x => x.Name.Equals(name));
            }
        }
    }
}