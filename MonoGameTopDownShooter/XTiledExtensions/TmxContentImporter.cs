using System.IO;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace XTiledExtensions
{
    [ContentImporter(".tmx", DefaultProcessor = "TmxContentProcessor", DisplayName = "TMX Map Importer - XTiled")]
    public class TmxContentImporter : ContentImporter<XDocument>
    {
        public override XDocument Import(string filename, ContentImporterContext context)
        {
            var doc = XDocument.Load(filename);
            doc.Document.Root.Add(new XElement("File",
                new XAttribute("name", Path.GetFileName(filename)),
                new XAttribute("path", Path.GetDirectoryName(filename))));

            return doc;
        }
    }
}