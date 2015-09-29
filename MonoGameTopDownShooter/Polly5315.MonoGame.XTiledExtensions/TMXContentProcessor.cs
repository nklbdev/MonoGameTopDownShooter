using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using System.IO.Compression;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Graphics;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Polly.MonoGame.XTiledExtensions
{
    [ContentProcessor(DisplayName = "TMX Map - XTiled")]
    public class TmxContentProcessor : ContentProcessor<XDocument, Map>
    {
        //private const uint FLIPPED_HORIZONTALLY_FLAG = 2147483648U;
        //private const uint FLIPPED_VERTICALLY_FLAG = 1073741824U;
        //private const uint FLIPPED_DIAGONALLY_FLAG = 536870912U;

        [DefaultValue(true)]
        [Description("If true, XTiled will build textures with the Map.")]
        [DisplayName("Load Textures")]
        public bool LoadTextures { get; set; }

        [Description("Texture processor output format if loading textures")]
        [DefaultValue(TextureProcessorOutputFormat.Color)]
        [DisplayName("Texture - Format")]
        public TextureProcessorOutputFormat TextureFormat { get; set; }

        [DisplayName("Texture - Premultiply Alpha")]
        [DefaultValue(true)]
        [Description("If true, texture is converted to premultiplied alpha format")]
        public bool PremultiplyAlpha { get; set; }

        public TmxContentProcessor()
        {
            LoadTextures = true;
            TextureFormat = (TextureProcessorOutputFormat)1;
            PremultiplyAlpha = true;
        }

        public override Map Process(XDocument input, ContentProcessorContext context)
        {
            var currentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            var map = new Map { LoadTextures = LoadTextures };
            var list1 = new List<Tile>();
            var dictionary = new Dictionary<uint, int> { { 0U, -1 } };
            var path1 = input.Document.Root.Element("File").Attribute("path").Value;
            if (Convert.ToDecimal(input.Document.Root.Attribute("version").Value) != new Decimal(10, 0, 0, false, 1))
                throw new NotSupportedException("XTiled only supports TMX maps version 1.0");
            switch (input.Document.Root.Attribute("orientation").Value)
            {
                case "orthogonal":
                    map.Orientation = MapOrientation.Orthogonal;
                    break;
                case "isometric":
                    map.Orientation = MapOrientation.Isometric;
                    break;
                default:
                    throw new NotSupportedException("XTiled supports only orthogonal or isometric maps");
            }
            map.Width = Convert.ToInt32(input.Document.Root.Attribute("width").Value);
            map.Height = Convert.ToInt32(input.Document.Root.Attribute("height").Value);
            map.TileWidth = Convert.ToInt32(input.Document.Root.Attribute("tilewidth").Value);
            map.TileHeight = Convert.ToInt32(input.Document.Root.Attribute("tileheight").Value);
            map.Bounds = new Rectangle(0, 0, map.Width * map.TileWidth, map.Height * map.TileHeight);
            map.Properties = new Dictionary<string, Property>();
            if (input.Document.Root.Element("properties") != null)
            {
                foreach (var xelement in input.Document.Root.Element("properties").Elements("property"))
                    map.Properties.Add(xelement.Attribute("name").Value, Property.Create(xelement.Attribute("value").Value));
            }
            var list2 = new List<Tileset>();
            foreach (var xelement1 in input.Document.Root.Elements("tileset"))
            {
                var tileset = new Tileset();
                var xelement2 = xelement1;
                var num1 = Convert.ToUInt32(xelement2.Attribute("firstgid").Value);
                if (xelement1.Attribute("source") != null)
                    xelement2 = XDocument.Load(xelement1.Attribute("source").Value).Root;
                tileset.Name = xelement2.Attribute("name") == null ? null : xelement2.Attribute("name").Value;
                tileset.TileWidth = xelement2.Attribute("tilewidth") == null ? 0 : Convert.ToInt32(xelement2.Attribute("tilewidth").Value);
                tileset.TileHeight = xelement2.Attribute("tileheight") == null ? 0 : Convert.ToInt32(xelement2.Attribute("tileheight").Value);
                tileset.Spacing = xelement2.Attribute("spacing") == null ? 0 : Convert.ToInt32(xelement2.Attribute("spacing").Value);
                tileset.Margin = xelement2.Attribute("margin") == null ? 0 : Convert.ToInt32(xelement2.Attribute("margin").Value);
                if (xelement2.Element("tileoffset") != null)
                {
                    tileset.TileOffsetX = Convert.ToInt32(xelement2.Element("tileoffset").Attribute("x").Value);
                    tileset.TileOffsetY = Convert.ToInt32(xelement2.Element("tileoffset").Attribute("y").Value);
                }
                else
                {
                    tileset.TileOffsetX = 0;
                    tileset.TileOffsetY = 0;
                }
                if (xelement2.Element("image") != null)
                {
                    var xelement3 = xelement2.Element("image");
                    tileset.ImageFileName = Path.Combine(path1, xelement3.Attribute("source").Value);
                    tileset.ImageWidth = xelement3.Attribute("width") == null ? -1 : Convert.ToInt32(xelement3.Attribute("width").Value);
                    tileset.ImageHeight = xelement3.Attribute("height") == null ? -1 : Convert.ToInt32(xelement3.Attribute("height").Value);
                    tileset.ImageTransparentColor = new Microsoft.Xna.Framework.Color?();
                    if (xelement3.Attribute("trans") != null)
                    {
                        var color = ColorTranslator.FromHtml("#" + xelement3.Attribute("trans").Value.TrimStart('#'));
                        tileset.ImageTransparentColor = new Microsoft.Xna.Framework.Color(color.R, color.G, color.B);
                    }
                    if (!(tileset.ImageWidth != -1 && tileset.ImageHeight != -1))
                    {
                        try
                        {
                            var image = Image.FromFile(tileset.ImageFileName);
                            tileset.ImageHeight = image.Height;
                            tileset.ImageWidth = image.Width;
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(
                                string.Format("Image size not set for {0} and error loading file.",
                                    tileset.ImageFileName), ex);
                        }
                    }
                    if (LoadTextures)
                    {
                        var str = Path.Combine(Path.GetDirectoryName(context.OutputFilename.Remove(0, context.OutputDirectory.Length)), Path.GetFileNameWithoutExtension(context.OutputFilename), list2.Count.ToString("00"));
                        var opaqueDataDictionary = new OpaqueDataDictionary
                        {
                            {"GenerateMipmaps", false},
                            {"ResizeToPowerOfTwo", false},
                            {"PremultiplyAlpha", PremultiplyAlpha ? 1 : 0},
                            {"TextureFormat", TextureFormat},
                            {"ColorKeyEnabled", tileset.ImageTransparentColor.HasValue ? 1 : 0},
                            {"ColorKeyColor", tileset.ImageTransparentColor ?? Microsoft.Xna.Framework.Color.Magenta }
                        };
                        context.BuildAsset<TextureContent, TextureContent>(new ExternalReference<TextureContent>(tileset.ImageFileName), "TextureProcessor", opaqueDataDictionary, "TextureImporter", str);
                    }
                }
                var index = num1;
                var num2 = tileset.Margin;
                while (num2 < tileset.ImageHeight - tileset.Margin)
                {
                    if (num2 + tileset.TileHeight <= tileset.ImageHeight - tileset.Margin)
                    {
                        var num3 = tileset.Margin;
                        while (num3 < tileset.ImageWidth - tileset.Margin)
                        {
                            if (num3 + tileset.TileWidth <= tileset.ImageWidth - tileset.Margin)
                            {
                                list1.Add(new Tile
                                {
                                    Source = new Rectangle(num3, num2, tileset.TileWidth, tileset.TileHeight),
                                    Origin = new Vector2(tileset.TileWidth / 2, tileset.TileHeight / 2),
                                    TilesetID = list2.Count,
                                    Properties = new Dictionary<string, Property>()
                                });
                                dictionary[index] = list1.Count - 1;
                                ++index;
                            }
                            num3 += tileset.TileWidth + tileset.Spacing;
                        }
                    }
                    num2 += tileset.TileHeight + tileset.Spacing;
                }
                var list3 = new List<Tile>();
                foreach (var xelement3 in xelement2.Elements("tile"))
                {
                    var num3 = Convert.ToUInt32(xelement3.Attribute("id").Value);
                    var tile = list1[dictionary[num3 + num1]];
                    if (xelement3.Element("properties") != null)
                    {
                        foreach (var xelement4 in xelement3.Element("properties").Elements("property"))
                            tile.Properties.Add(xelement4.Attribute("name").Value, Property.Create(xelement4.Attribute("value").Value));
                    }
                    list3.Add(tile);
                }
                tileset.Tiles = list3.ToArray();
                tileset.Properties = new Dictionary<string, Property>();
                if (xelement2.Element("properties") != null)
                {
                    foreach (var xelement3 in xelement2.Element("properties").Elements("property"))
                        tileset.Properties.Add(xelement3.Attribute("name").Value, Property.Create(xelement3.Attribute("value").Value));
                }
                list2.Add(tileset);
            }
            map.Tilesets = list2.ToArray();
            var tileLayerList = new TileLayerList();
            foreach (var xelement1 in input.Document.Root.Elements("layer"))
            {
                var tileLayer = new TileLayer
                {
                    Name = xelement1.Attribute("name") == null ? null : xelement1.Attribute("name").Value,
                    Opacity = xelement1.Attribute("opacity") == null ? 1f : Convert.ToSingle(xelement1.Attribute("opacity").Value),
                    Visible = xelement1.Attribute("visible") == null || xelement1.Attribute("visible").Equals("1"),
                    OpacityColor = Microsoft.Xna.Framework.Color.White
                };
                // ISSUE: explicit reference operation
                @tileLayer.OpacityColor.A = Convert.ToByte(byte.MaxValue * tileLayer.Opacity);
                tileLayer.Properties = new Dictionary<string, Property>();
                if (xelement1.Element("properties") != null)
                {
                    foreach (var xelement2 in xelement1.Element("properties").Elements("property"))
                        tileLayer.Properties.Add(xelement2.Attribute("name").Value, Property.Create(xelement2.Attribute("value").Value));
                }
                var tileDataArray = new TileData[map.Orientation == MapOrientation.Orthogonal ? map.Width : map.Height + map.Width - 1][];
                for (var index = 0; index < tileDataArray.Length; ++index)
                    tileDataArray[index] = new TileData[map.Orientation == MapOrientation.Orthogonal ? map.Height : map.Height + map.Width - 1];
                if (xelement1.Element("data") != null)
                {
                    var list3 = new List<uint>();
                    if (xelement1.Element("data").Attribute("encoding") != null || xelement1.Element("data").Attribute("compression") != null)
                    {
                        if (xelement1.Element("data").Attribute("encoding") != null && xelement1.Element("data").Attribute("encoding").Value.Equals("csv"))
                        {
                            list3.AddRange(xelement1.Element("data").Value.Split(",\n\r".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(str => Convert.ToUInt32(str)));
                        }
                        else
                        {
                            if (xelement1.Element("data").Attribute("encoding") == null || !xelement1.Element("data").Attribute("encoding").Value.Equals("base64"))
                                throw new NotSupportedException(string.Format("Encoding '{0}' not supported.  XTiled supports csv or base64", xelement1.Element("data").Attribute("encoding").Value));
                            var buffer1 = Convert.FromBase64String(xelement1.Element("data").Value);
                            if (xelement1.Element("data").Attribute("compression") == null)
                            {
                                var startIndex = 0;
                                while (startIndex < buffer1.Length)
                                {
                                    list3.Add(BitConverter.ToUInt32(buffer1, startIndex));
                                    startIndex += 4;
                                }
                            }
                            else if (xelement1.Element("data").Attribute("compression").Value.Equals("gzip"))
                            {
                                var gzipStream = new GZipStream(new MemoryStream(buffer1), CompressionMode.Decompress);
                                var buffer2 = new byte[4];
                                while (gzipStream.Read(buffer2, 0, buffer2.Length) == buffer2.Length)
                                    list3.Add(BitConverter.ToUInt32(buffer2, 0));
                            }
                            else
                            {
                                if (!xelement1.Element("data").Attribute("compression").Value.Equals("zlib"))
                                    throw new NotSupportedException(string.Format("Compression '{0}' not supported.  XTiled supports gzip or zlib", xelement1.Element("data").Attribute("compression").Value));
                                var memoryStream = new MemoryStream(buffer1);
                                memoryStream.ReadByte();
                                memoryStream.ReadByte();
                                var deflateStream = new DeflateStream(memoryStream, CompressionMode.Decompress);
                                var buffer2 = new byte[4];
                                while (deflateStream.Read(buffer2, 0, buffer2.Length) == buffer2.Length)
                                    list3.Add(BitConverter.ToUInt32(buffer2, 0));
                            }
                        }
                    }
                    else
                    {
                        list3.AddRange(xelement1.Element("data").Elements("tile").Select(xelement2 => Convert.ToUInt32(xelement2.Attribute("gid").Value)));
                    }
                    for (var index1 = 0; index1 < list3.Count; ++index1)
                    {
                        var tileData = new TileData();
                        var index2 = list3[index1] & 536870911U;
                        tileData.SourceID = dictionary[index2];
                        if (tileData.SourceID >= 0)
                        {
                            var flag1 = Convert.ToBoolean(list3[index1] & int.MinValue);
                            var flag2 = Convert.ToBoolean(list3[index1] & 1073741824U);
                            var flag3 = Convert.ToBoolean(list3[index1] & 536870912U);
                            if (flag3)
                            {
                                tileData.Rotation = 1.570796f;
                                flag1 = false;
                            }
                            else
                                tileData.Rotation = 0.0f;
                            tileData.Effects = !flag2 || !flag1 ? (!flag2 ? (!flag1 ? 0 : (SpriteEffects)1) : (SpriteEffects)2) : (SpriteEffects)3;
                            tileData.Target.Width = list1[tileData.SourceID].Source.Width;
                            tileData.Target.Height = list1[tileData.SourceID].Source.Height;
                            if (map.Orientation == MapOrientation.Orthogonal)
                            {
                                var index3 = index1 % map.Width;
                                var index4 = index1 / map.Width;
                                tileData.Target.X = (index3 * map.TileWidth + Convert.ToInt32(list1[tileData.SourceID].Origin.X) + map.Tilesets[list1[tileData.SourceID].TilesetID].TileOffsetX);
                                tileData.Target.Y = (index4 * map.TileHeight + Convert.ToInt32(list1[tileData.SourceID].Origin.Y) + map.Tilesets[list1[tileData.SourceID].TilesetID].TileOffsetY);
                                // ISSUE: explicit reference operation
                                // ISSUE: variable of a reference type
                                var local1 = @tileData.Target;
                                // ISSUE: explicit reference operation
                                var num1 = local1.Y + (map.TileHeight - tileData.Target.Height);
                                // ISSUE: explicit reference operation
                                local1.Y = num1;
                                if (flag3 && tileData.Target.Width % 2 == 1)
                                {
                                    // ISSUE: explicit reference operation
                                    // ISSUE: variable of a reference type
                                    var local2 = @tileData.Target;
                                    // ISSUE: explicit reference operation
                                    var num2 = local2.X + 1;
                                    // ISSUE: explicit reference operation
                                    local2.X = num2;
                                }
                                tileDataArray[index3][index4] = tileData;
                            }
                            else if (map.Orientation == MapOrientation.Isometric)
                            {
                                var index3 = map.Height + index1 % map.Width - (index1 / map.Width + 1);
                                var index4 = index1 - index1 / map.Width * map.Width + index1 / map.Width;
                                tileData.Target.X = (index3 * map.TileWidth + Convert.ToInt32(list1[tileData.SourceID].Origin.X) + map.Tilesets[list1[tileData.SourceID].TilesetID].TileOffsetX);
                                tileData.Target.Y = (index4 * map.TileHeight + Convert.ToInt32(list1[tileData.SourceID].Origin.Y) + map.Tilesets[list1[tileData.SourceID].TilesetID].TileOffsetY);
                                // ISSUE: explicit reference operation
                                // ISSUE: variable of a reference type
                                var local1 = @tileData.Target;
                                // ISSUE: explicit reference operation
                                var num1 = local1.Y + (map.TileHeight - tileData.Target.Height);
                                // ISSUE: explicit reference operation
                                local1.Y = num1;
                                tileData.Target.X = (tileData.Target.X / 2 + map.TileWidth / 4);
                                tileData.Target.Y = (tileData.Target.Y / 2 + map.TileHeight / 4);
                                if (flag3 && tileData.Target.Width % 2 == 1)
                                {
                                    // ISSUE: explicit reference operation
                                    // ISSUE: variable of a reference type
                                    var local2 = @tileData.Target;
                                    // ISSUE: explicit reference operation
                                    var num2 = local2.X + 1;
                                    // ISSUE: explicit reference operation
                                    local2.X = num2;
                                }
                                tileDataArray[index3][index4] = tileData;
                            }
                        }
                    }
                }
                tileLayer.Tiles = tileDataArray;
                tileLayerList.Add(tileLayer);
            }
            map.TileLayers = tileLayerList;
            map.SourceTiles = list1.ToArray();
            var objectLayerList = new ObjectLayerList();
            foreach (var xelement1 in input.Document.Root.Elements("objectgroup"))
            {
                var objectLayer = new ObjectLayer
                {
                    Name = xelement1.Attribute("name") == null ? null : xelement1.Attribute("name").Value,
                    Opacity = xelement1.Attribute("opacity") == null ? 1f : Convert.ToSingle(xelement1.Attribute("opacity").Value),
                    Visible = xelement1.Attribute("visible") == null || xelement1.Attribute("visible").Equals("1"),
                    OpacityColor = Microsoft.Xna.Framework.Color.White
                };
                // ISSUE: explicit reference operation
                @objectLayer.OpacityColor.A = Convert.ToByte(byte.MaxValue * objectLayer.Opacity);
                objectLayer.Color = new Microsoft.Xna.Framework.Color?();
                if (xelement1.Attribute("color") != null)
                {
                    var color = ColorTranslator.FromHtml("#" + xelement1.Attribute("color").Value.TrimStart('#'));
                    // ISSUE: explicit reference operation
                    objectLayer.Color = new Microsoft.Xna.Framework.Color(color.R, color.G, color.B, @objectLayer.OpacityColor.A);
                }
                objectLayer.Properties = new Dictionary<string, Property>();
                if (xelement1.Element("properties") != null)
                {
                    foreach (var xelement2 in xelement1.Element("properties").Elements("property"))
                        objectLayer.Properties.Add(xelement2.Attribute("name").Value, Property.Create(xelement2.Attribute("value").Value));
                }
                var list3 = new List<MapObject>();
                foreach (var xelement2 in xelement1.Elements("object"))
                {
                    var mapObject = new MapObject
                    {
                        Name = xelement2.Attribute("name") == null ? null : xelement2.Attribute("name").Value,
                        Type = xelement2.Attribute("type") == null ? null : xelement2.Attribute("type").Value,
                        Bounds = {
                            X = xelement2.Attribute("x") == null ? 0 : Convert.ToInt32(xelement2.Attribute("x").Value),
                            Y = xelement2.Attribute("y") == null ? 0 : Convert.ToInt32(xelement2.Attribute("y").Value),
                            Width = xelement2.Attribute("width") == null ? 0 : Convert.ToInt32(xelement2.Attribute("width").Value),
                            Height = xelement2.Attribute("height") == null ? 0 : Convert.ToInt32(xelement2.Attribute("height").Value)
                        }
                    };
                    mapObject.TileID = xelement2.Attribute("gid") == null ? new int?() : dictionary[Convert.ToUInt32(xelement2.Attribute("gid").Value)];
                    mapObject.Visible = xelement2.Attribute("visible") == null || xelement2.Attribute("visible").Equals("1");
                    if (mapObject.TileID.HasValue)
                    {
                        // ISSUE: explicit reference operation
                        // ISSUE: variable of a reference type
                        var local1 = @mapObject.Bounds;
                        // ISSUE: explicit reference operation
                        var num1 = local1.X + Convert.ToInt32(list1[mapObject.TileID.Value].Origin.X);
                        // ISSUE: explicit reference operation
                        local1.X = num1;
                        // ISSUE: explicit reference operation
                        // ISSUE: variable of a reference type
                        var local2 = @mapObject.Bounds;
                        // ISSUE: explicit reference operation
                        var num2 = local2.Y - Convert.ToInt32(list1[mapObject.TileID.Value].Origin.Y);
                        // ISSUE: explicit reference operation
                        local2.Y = num2;
                        mapObject.Bounds.Width = map.SourceTiles[mapObject.TileID.Value].Source.Width;
                        mapObject.Bounds.Height = map.SourceTiles[mapObject.TileID.Value].Source.Height;
                    }
                    mapObject.Properties = new Dictionary<string, Property>();
                    if (xelement2.Element("properties") != null)
                    {
                        foreach (var xelement3 in xelement2.Element("properties").Elements("property"))
                            mapObject.Properties.Add(xelement3.Attribute("name").Value, Property.Create(xelement3.Attribute("value").Value));
                    }
                    mapObject.Polygon = null;
                    if (xelement2.Element("polygon") != null)
                    {
                        var list4 = new List<Microsoft.Xna.Framework.Point>();
                        foreach (var str in xelement2.Element("polygon").Attribute("points").Value.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                        {
                            var strArray = str.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            list4.Add(new Microsoft.Xna.Framework.Point(mapObject.Bounds.X + Convert.ToInt32(strArray[0]), mapObject.Bounds.Y + Convert.ToInt32(strArray[1])));
                        }
                        list4.Add(list4.First());
                        mapObject.Polygon = Polygon.FromPoints(list4);
                        mapObject.Bounds = mapObject.Polygon.Bounds;
                    }
                    mapObject.Polyline = null;
                    if (xelement2.Element("polyline") != null)
                    {
                        var list4 = new List<Microsoft.Xna.Framework.Point>();
                        foreach (var str in xelement2.Element("polyline").Attribute("points").Value.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                        {
                            var strArray = str.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            list4.Add(new Microsoft.Xna.Framework.Point(mapObject.Bounds.X + Convert.ToInt32(strArray[0]), mapObject.Bounds.Y + Convert.ToInt32(strArray[1])));
                        }
                        mapObject.Polyline = Polyline.FromPoints(list4);
                        mapObject.Bounds = mapObject.Polyline.Bounds;
                    }
                    list3.Add(mapObject);
                }
                objectLayer.MapObjects = list3.ToArray();
                objectLayerList.Add(objectLayer);
            }
            map.ObjectLayers = objectLayerList;
            var num4 = 0;
            var num5 = 0;
            var list5 = new List<LayerInfo>();
            foreach (var xelement in input.Document.Root.Elements())
            {
                if (xelement.Name.LocalName.Equals("layer"))
                    list5.Add(new LayerInfo
                    {
                        ID = num4++,
                        LayerType = LayerType.TileLayer
                    });
                else if (xelement.Name.LocalName.Equals("objectgroup"))
                    list5.Add(new LayerInfo
                    {
                        ID = num5++,
                        LayerType = LayerType.ObjectLayer
                    });
            }
            map.LayerOrder = list5.ToArray();
            Thread.CurrentThread.CurrentCulture = currentCulture;
            return map;
        }
    }
}
