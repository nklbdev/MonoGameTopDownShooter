using System.IO;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace Polly.MonoGame.XTiledExtensions
{
 [ContentImporter(".tmx", DefaultProcessor = "TMXContentProcessor", DisplayName = "TMX Map Importer")]
  public class TmxContentImporter : ContentImporter<XDocument>
  {
    public override XDocument Import(string filename, ContentImporterContext context)
    {
      var xdocument = XDocument.Load(filename);
      xdocument.Document.Root.Add(new XElement("File", new object[]
      {
          new XAttribute("name", Path.GetFileName(filename)),
          new XAttribute("path", Path.GetDirectoryName(filename))
      }));
      return xdocument;
    }
  }}
