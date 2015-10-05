using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Graphics;
using XTiled;
using Color = Microsoft.Xna.Framework.Color;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace XTiledExtensions
{
    [ContentProcessor(DisplayName = "TMX Map Processor - XTiled")]
    public class TmxContentProcessor : ContentProcessor<XDocument, Map>
    {

        private const uint _flippedHorizontallyFlag = 0x80000000;
        private const uint _flippedVerticallyFlag = 0x40000000;
        private const uint _flippedDiagonallyFlag = 0x20000000;

        [DisplayName("Load Textures")]
        [DefaultValue(true)]
        [Description("If true, XTiled will build textures with the Map.")]
        public bool LoadTextures { get; set; }

        [DisplayName("Texture - Format")]
        [DefaultValue(TextureProcessorOutputFormat.Color)]
        [Description("Texture processor output format if loading textures")]
        public TextureProcessorOutputFormat TextureFormat { get; set; }

        [DisplayName("Texture - Premultiply Alpha")]
        [DefaultValue(true)]
        [Description("If true, texture is converted to premultiplied alpha format")]
        public bool PremultiplyAlpha { get; set; }

        public TmxContentProcessor()
        {
            LoadTextures = true;
            TextureFormat = TextureProcessorOutputFormat.Color;
            PremultiplyAlpha = true;
        }

        public override Map Process(XDocument input, ContentProcessorContext context)
        {
            var culture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            var map = new Map { LoadTextures = LoadTextures };
            var mapTiles = new List<Tile>();
            var gid2Id = new Dictionary<uint, int> { { 0, -1 } };

            var mapDirectory = input.Document.Root.Element("File").Attribute("path").Value;

            var version = Convert.ToDecimal(input.Document.Root.Attribute("version").Value);
            if (version != 1.0M)
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
                foreach (var pElem in input.Document.Root.Element("properties").Elements("property"))
                    map.Properties.Add(pElem.Attribute("name").Value, Property.Create(pElem.Attribute("value").Value));

            var tilesets = new List<Tileset>();
            foreach (var elem in input.Document.Root.Elements("tileset"))
            {
                var t = new Tileset();
                var tElem = elem;
                var firstGid = Convert.ToUInt32(tElem.Attribute("firstgid").Value);
                var fileRoot = mapDirectory;

                if (elem.Attribute("source") != null)
                {
                    fileRoot = Path.Combine(mapDirectory, elem.Attribute("source").Value);
                    var tsx = XDocument.Load(fileRoot);
                    fileRoot = Path.GetDirectoryName(fileRoot);
                    tElem = tsx.Root;
                }

                t.Name = tElem.Attribute("name") == null ? null : tElem.Attribute("name").Value;
                t.TileWidth = tElem.Attribute("tilewidth") == null ? 0 : Convert.ToInt32(tElem.Attribute("tilewidth").Value);
                t.TileHeight = tElem.Attribute("tileheight") == null ? 0 : Convert.ToInt32(tElem.Attribute("tileheight").Value);
                t.Spacing = tElem.Attribute("spacing") == null ? 0 : Convert.ToInt32(tElem.Attribute("spacing").Value);
                t.Margin = tElem.Attribute("margin") == null ? 0 : Convert.ToInt32(tElem.Attribute("margin").Value);

                if (tElem.Element("tileoffset") != null)
                {
                    t.TileOffsetX = Convert.ToInt32(tElem.Element("tileoffset").Attribute("x").Value);
                    t.TileOffsetY = Convert.ToInt32(tElem.Element("tileoffset").Attribute("y").Value);
                }
                else
                {
                    t.TileOffsetX = 0;
                    t.TileOffsetY = 0;
                }

                if (tElem.Element("image") != null)
                {
                    var imgElem = tElem.Element("image");
                    t.ImageFileName = Path.Combine(fileRoot, imgElem.Attribute("source").Value);
                    t.ImageWidth = imgElem.Attribute("width") == null ? -1 : Convert.ToInt32(imgElem.Attribute("width").Value);
                    t.ImageHeight = imgElem.Attribute("height") == null ? -1 : Convert.ToInt32(imgElem.Attribute("height").Value);
                    t.ImageTransparentColor = null;
                    if (imgElem.Attribute("trans") != null)
                    {
                        var sdc = ColorTranslator.FromHtml("#" + imgElem.Attribute("trans").Value.TrimStart('#'));
                        t.ImageTransparentColor = new Color(sdc.R, sdc.G, sdc.B);
                    }

                    if (t.ImageWidth == -1 || t.ImageHeight == -1)
                    {
                        try
                        {
                            var sdi = Image.FromFile(t.ImageFileName);
                            t.ImageHeight = sdi.Height;
                            t.ImageWidth = sdi.Width;
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(string.Format("Image size not set for {0} and error loading file.", t.ImageFileName), ex);
                        }
                    }

                    if (LoadTextures)
                    {
                        var assetName = Path.Combine(
                                Path.GetDirectoryName(context.OutputFilename.Remove(0, context.OutputDirectory.Length)),
                                Path.GetFileNameWithoutExtension(context.OutputFilename),
                                tilesets.Count.ToString("00"));

                        var data = new OpaqueDataDictionary
                        {
                            {"GenerateMipmaps", false},
                            {"ResizeToPowerOfTwo", false},
                            {"PremultiplyAlpha", PremultiplyAlpha},
                            {"TextureFormat", TextureFormat},
                            {"ColorKeyEnabled", t.ImageTransparentColor.HasValue},
                            {"ColorKeyColor", t.ImageTransparentColor ?? Color.Magenta}
                        };
                        context.BuildAsset<TextureContent, TextureContent>(new ExternalReference<TextureContent>(t.ImageFileName),
                            "TextureProcessor", data, "TextureImporter", assetName);
                    }
                }

                var gid = firstGid;
                for (var y = t.Margin; y < t.ImageHeight - t.Margin; y += t.TileHeight + t.Spacing)
                {
                    if (y + t.TileHeight > t.ImageHeight - t.Margin)
                        continue;

                    for (var x = t.Margin; x < t.ImageWidth - t.Margin; x += t.TileWidth + t.Spacing)
                    {
                        if (x + t.TileWidth > t.ImageWidth - t.Margin)
                            continue;

                        var tile = new Tile
                        {
                            Source = new Rectangle(x, y, t.TileWidth, t.TileHeight),
                            Origin = new Vector2(t.TileWidth / 2f, t.TileHeight / 2f),
                            TilesetId = tilesets.Count,
                            Properties = new Dictionary<string, Property>()
                        };
                        mapTiles.Add(tile);

                        gid2Id[gid] = mapTiles.Count - 1;
                        gid++;
                    }
                }

                var tiles = new List<Tile>();
                foreach (var tileElem in tElem.Elements("tile"))
                {
                    var id = Convert.ToUInt32(tileElem.Attribute("id").Value);
                    var tile = mapTiles[gid2Id[id + firstGid]];
                    if (tileElem.Element("properties") != null)
                        foreach (var pElem in tileElem.Element("properties").Elements("property"))
                            tile.Properties.Add(pElem.Attribute("name").Value, Property.Create(pElem.Attribute("value").Value));
                    tiles.Add(tile);
                }
                t.Tiles = tiles.ToArray();

                t.Properties = new Dictionary<string, Property>();
                if (tElem.Element("properties") != null)
                    foreach (var pElem in tElem.Element("properties").Elements("property"))
                        t.Properties.Add(pElem.Attribute("name").Value, Property.Create(pElem.Attribute("value").Value));

                tilesets.Add(t);
            }
            map.Tilesets = tilesets.ToArray();

            var layers = new TileLayerList();
            foreach (var lElem in input.Document.Root.Elements("layer"))
            {
                var l = new TileLayer
                {
                    Name = lElem.Attribute("name") == null ? null : lElem.Attribute("name").Value,
                    Opacity = lElem.Attribute("opacity") == null ? 1.0f : Convert.ToSingle(lElem.Attribute("opacity").Value),
                    Visible = lElem.Attribute("visible") == null,
                    OpacityColor = Color.White
                };

                l.OpacityColor.A = Convert.ToByte(255.0f * l.Opacity);

                l.Properties = new Dictionary<string, Property>();
                if (lElem.Element("properties") != null)
                    foreach (var pElem in lElem.Element("properties").Elements("property"))
                        l.Properties.Add(pElem.Attribute("name").Value, Property.Create(pElem.Attribute("value").Value));

                var tiles = new TileData[map.Orientation == MapOrientation.Orthogonal ? map.Width : map.Height + map.Width - 1][];
                for (var i = 0; i < tiles.Length; i++)
                    tiles[i] = new TileData[map.Orientation == MapOrientation.Orthogonal ? map.Height : map.Height + map.Width - 1];

                if (lElem.Element("data") != null)
                {
                    var gids = new List<uint>();
                    if (lElem.Element("data").Attribute("encoding") != null ||
                        lElem.Element("data").Attribute("compression") != null)
                    {
                        // parse csv formatted data
                        if (lElem.Element("data").Attribute("encoding") != null &&
                            lElem.Element("data").Attribute("encoding").Value.Equals("csv"))
                            gids.AddRange(lElem.Element("data")
                                .Value.Split(",\n\r".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                                .Select(gid => Convert.ToUInt32(gid)));

                        else if (lElem.Element("data").Attribute("encoding") != null &&
                                 lElem.Element("data").Attribute("encoding").Value.Equals("base64"))
                        {
                            var data = Convert.FromBase64String(lElem.Element("data").Value);

                            if (lElem.Element("data").Attribute("compression") == null)
                                // uncompressed data
                                for (var i = 0; i < data.Length; i += sizeof(uint))
                                    gids.Add(BitConverter.ToUInt32(data, i));

                            else if (lElem.Element("data").Attribute("compression").Value.Equals("gzip"))
                            {
                                // gzip data
                                var gz = new GZipStream(new MemoryStream(data), CompressionMode.Decompress);
                                var buffer = new byte[sizeof(uint)];
                                while (gz.Read(buffer, 0, buffer.Length) == buffer.Length)
                                    gids.Add(BitConverter.ToUInt32(buffer, 0));
                            }
                            else if (lElem.Element("data").Attribute("compression").Value.Equals("zlib"))
                            {
                                // zlib data - first two bytes zlib specific and not part of deflate
                                var ms = new MemoryStream(data);
                                ms.ReadByte();
                                ms.ReadByte();
                                var gz = new DeflateStream(ms, CompressionMode.Decompress);
                                var buffer = new byte[sizeof(uint)];
                                while (gz.Read(buffer, 0, buffer.Length) == buffer.Length)
                                    gids.Add(BitConverter.ToUInt32(buffer, 0));
                            }
                            else
                            {
                                throw new NotSupportedException(
                                    string.Format("Compression '{0}' not supported.  XTiled supports gzip or zlib",
                                        lElem.Element("data").Attribute("compression").Value));
                            }
                        }
                        else
                        {
                            throw new NotSupportedException(
                                string.Format("Encoding '{0}' not supported.  XTiled supports csv or base64",
                                    lElem.Element("data").Attribute("encoding").Value));
                        }
                    }
                    else
                        // parse xml formatted data
                        gids.AddRange(lElem.Element("data").Elements("tile").Select(tElem => Convert.ToUInt32(tElem.Attribute("gid").Value)));

                    for (var i = 0; i < gids.Count; i++)
                    {
                        var td = new TileData();
                        var id = gids[i] & ~(_flippedHorizontallyFlag | _flippedVerticallyFlag | _flippedDiagonallyFlag);
                        td.SourceId = gid2Id[id];

                        if (td.SourceId < 0)
                            continue;

                        var isFlippedHorizontally = Convert.ToBoolean(gids[i] & _flippedHorizontallyFlag);
                        var isFlippedVertically = Convert.ToBoolean(gids[i] & _flippedVerticallyFlag);
                        var isFlippedDiagonally = Convert.ToBoolean(gids[i] & _flippedDiagonallyFlag);

                        if (isFlippedHorizontally && isFlippedDiagonally)
                        {
                            td.Rotation = MathHelper.PiOver2;
                            // this works, not 100% why (we are rotating instead of diag flipping, so I guess that's a clue)
                            isFlippedHorizontally = false;
                        }
                        else if (isFlippedVertically && isFlippedDiagonally)
                        {
                            td.Rotation = -MathHelper.PiOver2;
                            // this works, not 100% why (we are rotating instead of diag flipping, so I guess that's a clue)
                            isFlippedVertically = false;
                        }
                        else
                            td.Rotation = 0;

                        if (isFlippedVertically && isFlippedHorizontally)
                            td.Effects = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
                        else if (isFlippedVertically)
                            td.Effects = SpriteEffects.FlipVertically;
                        else if (isFlippedHorizontally)
                            td.Effects = SpriteEffects.FlipHorizontally;
                        else
                            td.Effects = SpriteEffects.None;

                        td.Target.Width = mapTiles[td.SourceId].Source.Width;
                        td.Target.Height = mapTiles[td.SourceId].Source.Height;

                        switch (map.Orientation)
                        {
                            case MapOrientation.Orthogonal:
                                {
                                    var x = i % map.Width;
                                    var y = i / map.Width;
                                    td.Target.X = x * map.TileWidth + Convert.ToInt32(mapTiles[td.SourceId].Origin.X) + map.Tilesets[mapTiles[td.SourceId].TilesetId].TileOffsetX;
                                    td.Target.Y = y * map.TileHeight + Convert.ToInt32(mapTiles[td.SourceId].Origin.Y) + map.Tilesets[mapTiles[td.SourceId].TilesetId].TileOffsetY;
                                    td.Target.Y += map.TileHeight - td.Target.Height;

                                    // adjust for off center origin in odd tiles sizes
                                    if (isFlippedDiagonally && td.Target.Width % 2 == 1)
                                        td.Target.X += 1;

                                    tiles[x][y] = td;
                                }
                                break;
                            case MapOrientation.Isometric:
                                {
                                    var x = map.Height + i % map.Width - (1 * i / map.Width + 1);
                                    var y = i - i / map.Width * map.Width + i / map.Width;
                                    td.Target.X = x * map.TileWidth + Convert.ToInt32(mapTiles[td.SourceId].Origin.X) + map.Tilesets[mapTiles[td.SourceId].TilesetId].TileOffsetX;
                                    td.Target.Y = y * map.TileHeight + Convert.ToInt32(mapTiles[td.SourceId].Origin.Y) + map.Tilesets[mapTiles[td.SourceId].TilesetId].TileOffsetY;
                                    td.Target.Y += map.TileHeight - td.Target.Height;
                                    td.Target.X = td.Target.X / 2 + map.TileWidth / 4;
                                    td.Target.Y = td.Target.Y / 2 + map.TileHeight / 4;

                                    // adjust for off center origin in odd tiles sizes
                                    if (isFlippedDiagonally && td.Target.Width % 2 == 1)
                                        td.Target.X += 1;

                                    tiles[x][y] = td;
                                }
                                break;
                        }
                    }
                }
                l.Tiles = tiles;

                layers.Add(l);
            }
            map.TileLayers = layers;
            map.SourceTiles = mapTiles.ToArray();

            var oLayers = new ObjectLayerList();
            foreach (var olElem in input.Document.Root.Elements("objectgroup"))
            {
                var ol = new ObjectLayer
                {
                    Name = olElem.Attribute("name") == null ? null : olElem.Attribute("name").Value,
                    Opacity = olElem.Attribute("opacity") == null ? 1.0f : Convert.ToSingle(olElem.Attribute("opacity").Value),
                    Visible = olElem.Attribute("visible") == null,
                    OpacityColor = Color.White
                };

                ol.OpacityColor.A = Convert.ToByte(255.0f * ol.Opacity);

                ol.Color = null;
                if (olElem.Attribute("color") != null)
                {
                    var sdc = ColorTranslator.FromHtml("#" + olElem.Attribute("color").Value.TrimStart('#'));
                    ol.Color = new Color(sdc.R, sdc.G, sdc.B, ol.OpacityColor.A);
                }

                ol.Properties = new Dictionary<string, Property>();
                if (olElem.Element("properties") != null)
                    foreach (var pElem in olElem.Element("properties").Elements("property"))
                        ol.Properties.Add(pElem.Attribute("name").Value, Property.Create(pElem.Attribute("value").Value));

                var objects = new List<MapObject>();
                foreach (var oElem in olElem.Elements("object"))
                {
                    var o = new MapObject
                    {
                        Name = oElem.Attribute("name") == null ? null : oElem.Attribute("name").Value,
                        Type = oElem.Attribute("type") == null ? null : oElem.Attribute("type").Value,
                        Bounds =
                        {
                            X = oElem.Attribute("x") == null ? 0 : Convert.ToInt32(oElem.Attribute("x").Value),
                            Y = oElem.Attribute("y") == null ? 0 : Convert.ToInt32(oElem.Attribute("y").Value),
                            Width =
                                oElem.Attribute("width") == null ? 0 : Convert.ToInt32(oElem.Attribute("width").Value),
                            Height =
                                oElem.Attribute("height") == null ? 0 : Convert.ToInt32(oElem.Attribute("height").Value)
                        },
                        TileId = oElem.Attribute("gid") == null ? null : (int?) gid2Id[Convert.ToUInt32(oElem.Attribute("gid").Value)],
                        Visible = oElem.Attribute("visible") == null
                    };

                    if (o.TileId.HasValue)
                    {
                        o.Bounds.X += Convert.ToInt32(mapTiles[o.TileId.Value].Origin.X); // +map.Tilesets[mapTiles[o.TileID.Value].TilesetID].TileOffsetX;
                        o.Bounds.Y -= Convert.ToInt32(mapTiles[o.TileId.Value].Origin.Y); // +map.Tilesets[mapTiles[o.TileID.Value].TilesetID].TileOffsetY;
                        o.Bounds.Width = map.SourceTiles[o.TileId.Value].Source.Width;
                        o.Bounds.Height = map.SourceTiles[o.TileId.Value].Source.Height;
                    }

                    o.Properties = new Dictionary<string, Property>();
                    if (oElem.Element("properties") != null)
                        foreach (var pElem in oElem.Element("properties").Elements("property"))
                            o.Properties.Add(pElem.Attribute("name").Value, Property.Create(pElem.Attribute("value").Value));

                    o.Polygon = null;
                    if (oElem.Element("polygon") != null)
                    {
                        var points = oElem
                            .Element("polygon")
                            .Attribute("points")
                            .Value
                            .Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                            .Select(point => point.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                            .Select(coord => new Point(o.Bounds.X + Convert.ToInt32(coord[0]), o.Bounds.Y + Convert.ToInt32(coord[1])))
                            .ToList();

                        points.Add(points.First()); // connect the last point to the first and close the polygon
                        o.Polygon = Polygon.FromPoints(points);
                        o.Bounds = o.Polygon.Bounds;
                    }

                    o.Polyline = null;
                    if (oElem.Element("polyline") != null)
                    {
                        var points = oElem
                            .Element("polyline")
                            .Attribute("points")
                            .Value
                            .Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                            .Select(point => point.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                            .Select(coord => new Point(o.Bounds.X + Convert.ToInt32(coord[0]), o.Bounds.Y + Convert.ToInt32(coord[1])))
                            .ToList();

                        o.Polyline = Polyline.FromPoints(points);
                        o.Bounds = o.Polyline.Bounds;
                    }

                    objects.Add(o);
                }
                ol.MapObjects = objects.ToArray();

                oLayers.Add(ol);
            }
            map.ObjectLayers = oLayers;

            int layerId = 0, objectId = 0;
            var info = new List<LayerInfo>();
            foreach (var elem in input.Document.Root.Elements())
            {
                if (elem.Name.LocalName.Equals("layer"))
                    info.Add(new LayerInfo { Id = layerId++, LayerType = LayerType.TileLayer });
                else if (elem.Name.LocalName.Equals("objectgroup"))
                    info.Add(new LayerInfo { Id = objectId++, LayerType = LayerType.ObjectLayer });
            }
            map.LayerOrder = info.ToArray();

            Thread.CurrentThread.CurrentCulture = culture;
            return map;
        }
    }
}