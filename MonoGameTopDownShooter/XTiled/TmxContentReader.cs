using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace XTiled
{
    /// <summary/>
    public sealed class TmxContentReader : ContentTypeReader<Map>
    {
        /// <summary/>
        protected override Map Read(ContentReader input, Map existingInstance)
        {
            var m = new Map();
            int props;

            m.Orientation = input.ReadBoolean() ? MapOrientation.Orthogonal : MapOrientation.Isometric;
            m.Width = input.ReadInt32();
            m.Height = input.ReadInt32();
            m.TileHeight = input.ReadInt32();
            m.TileWidth = input.ReadInt32();
            m.Bounds.X = input.ReadInt32();
            m.Bounds.Y = input.ReadInt32();
            m.Bounds.Height = input.ReadInt32();
            m.Bounds.Width = input.ReadInt32();
            m.LoadTextures = input.ReadBoolean();

            m.Tilesets = new Tileset[input.ReadInt32()];
            for (var i = 0; i < m.Tilesets.Length; i++)
            {
                m.Tilesets[i] = new Tileset
                {
                    ImageFileName = input.ReadString(),
                    ImageHeight = input.ReadInt32(),
                    ImageWidth = input.ReadInt32(),
                    Margin = input.ReadInt32(),
                    Name = input.ReadString(),
                    Spacing = input.ReadInt32(),
                    TileHeight = input.ReadInt32(),
                    TileOffsetX = input.ReadInt32(),
                    TileOffsetY = input.ReadInt32(),
                    TileWidth = input.ReadInt32()
                };

                if (input.ReadBoolean())
                {
                    var c = Color.White;
                    c.A = input.ReadByte();
                    m.Tilesets[i].ImageTransparentColor = c;
                }

                m.Tilesets[i].Tiles = new Tile[input.ReadInt32()];
                for (var j = 0; j < m.Tilesets[i].Tiles.Length; j++)
                {
                    m.Tilesets[i].Tiles[j] = new Tile
                    {
                        TilesetId = input.ReadInt32(),
                        Origin =
                        {
                            X = input.ReadSingle(),
                            Y = input.ReadSingle()
                        },
                        Source =
                        {
                            X = input.ReadInt32(),
                            Y = input.ReadInt32(),
                            Height = input.ReadInt32(),
                            Width = input.ReadInt32()
                        }
                    };

                    props = input.ReadInt32();
                    m.Tilesets[i].Tiles[j].Properties = new Dictionary<string, Property>();
                    for (var p = 0; p < props; p++)
                    {
                        m.Tilesets[i].Tiles[j].Properties.Add(input.ReadString(), Property.Create(input.ReadString()));
                    }
                }

                props = input.ReadInt32();
                m.Tilesets[i].Properties = new Dictionary<string, Property>(props);
                for (var p = 0; p < props; p++)
                {
                    m.Tilesets[i].Properties.Add(input.ReadString(), Property.Create(input.ReadString()));
                }
            }

            props = input.ReadInt32();
            m.Properties = new Dictionary<string, Property>(props);
            for (var p = 0; p < props; p++)
            {
                m.Properties.Add(input.ReadString(), Property.Create(input.ReadString()));
            }

            m.TileLayers = new TileLayerList();
            var tileLayers = input.ReadInt32();
            for (var i = 0; i < tileLayers; i++)
            {

                m.TileLayers.Add(new TileLayer());
                m.TileLayers[i].Name = input.ReadString();
                m.TileLayers[i].Opacity = input.ReadSingle();
                m.TileLayers[i].OpacityColor = Color.White;
                m.TileLayers[i].OpacityColor.A = input.ReadByte();
                m.TileLayers[i].Visible = input.ReadBoolean();

                props = input.ReadInt32();
                m.TileLayers[i].Properties = new Dictionary<string, Property>(props);
                for (var p = 0; p < props; p++)
                {
                    m.TileLayers[i].Properties.Add(input.ReadString(), Property.Create(input.ReadString()));
                }

                m.TileLayers[i].Tiles = new TileData[input.ReadInt32()][];
                for (var x = 0; x < m.TileLayers[i].Tiles.Length; x++)
                {
                    m.TileLayers[i].Tiles[x] = new TileData[input.ReadInt32()];
                    for (var y = 0; y < m.TileLayers[i].Tiles[x].Length; y++)
                    {

                        if (input.ReadBoolean())
                        {
                            m.TileLayers[i].Tiles[x][y] = new TileData
                            {
                                Rotation = input.ReadSingle(),
                                SourceId = input.ReadInt32(),
                                Target =
                                {
                                    X = input.ReadInt32(),
                                    Y = input.ReadInt32(),
                                    Height = input.ReadInt32(),
                                    Width = input.ReadInt32()
                                }
                            };

                            var isFlippedHorizontally = input.ReadBoolean();
                            var isFlippedVertically = input.ReadBoolean();

                            if (isFlippedVertically && isFlippedHorizontally)
                                m.TileLayers[i].Tiles[x][y].Effects = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
                            else if (isFlippedVertically)
                                m.TileLayers[i].Tiles[x][y].Effects = SpriteEffects.FlipVertically;
                            else if (isFlippedHorizontally)
                                m.TileLayers[i].Tiles[x][y].Effects = SpriteEffects.FlipHorizontally;
                            else
                                m.TileLayers[i].Tiles[x][y].Effects = SpriteEffects.None;
                        }
                        else
                            m.TileLayers[i].Tiles[x][y] = null;
                    }
                }
            }

            m.SourceTiles = new Tile[input.ReadInt32()];
            for (var i = 0; i < m.SourceTiles.Length; i++)
            {
                m.SourceTiles[i] = new Tile
                {
                    Origin =
                    {
                        X = input.ReadSingle(),
                        Y = input.ReadSingle()
                    },
                    Source =
                    {
                        X = input.ReadInt32(),
                        Y = input.ReadInt32(),
                        Height = input.ReadInt32(),
                        Width = input.ReadInt32()
                    },
                    TilesetId = input.ReadInt32()
                };

                props = input.ReadInt32();
                m.SourceTiles[i].Properties = new Dictionary<string, Property>(props);
                for (var p = 0; p < props; p++)
                {
                    m.SourceTiles[i].Properties.Add(input.ReadString(), Property.Create(input.ReadString()));
                }
            }

            var olayers = input.ReadInt32();
            m.ObjectLayers = new ObjectLayerList();
            for (var i = 0; i < olayers; i++)
            {

                m.ObjectLayers.Add(new ObjectLayer());
                m.ObjectLayers[i].Name = input.ReadString();
                m.ObjectLayers[i].Opacity = input.ReadSingle();
                m.ObjectLayers[i].OpacityColor = Color.White;
                m.ObjectLayers[i].OpacityColor.A = input.ReadByte();
                m.ObjectLayers[i].Visible = input.ReadBoolean();

                if (input.ReadBoolean())
                {
                    var c = Color.White;
                    c.R = input.ReadByte();
                    c.G = input.ReadByte();
                    c.B = input.ReadByte();
                    c.A = input.ReadByte();
                    m.ObjectLayers[i].Color = c;
                }
                else
                    m.ObjectLayers[i].Color = null;

                m.ObjectLayers[i].MapObjects = new MapObject[input.ReadInt32()];
                for (var mo = 0; mo < m.ObjectLayers[i].MapObjects.Length; mo++)
                {

                    m.ObjectLayers[i].MapObjects[mo] = new MapObject
                    {
                        Bounds =
                        {
                            X = input.ReadInt32(),
                            Y = input.ReadInt32(),
                            Height = input.ReadInt32(),
                            Width = input.ReadInt32()
                        },
                        Name = input.ReadString(),
                        Type = input.ReadString(),
                        Visible = input.ReadBoolean()
                    };

                    if (input.ReadBoolean())
                        m.ObjectLayers[i].MapObjects[mo].TileId = input.ReadInt32();
                    else
                        m.ObjectLayers[i].MapObjects[mo].TileId = null;

                    if (input.ReadBoolean())
                    {
                        var points = new Point[input.ReadInt32()];
                        for (var p = 0; p < points.Length; p++)
                        {
                            points[p].X = input.ReadInt32();
                            points[p].Y = input.ReadInt32();
                        }
                        m.ObjectLayers[i].MapObjects[mo].Polyline = Polyline.FromPoints(points);
                    }
                    else
                        m.ObjectLayers[i].MapObjects[mo].Polyline = null;

                    if (input.ReadBoolean())
                    {
                        var points = new Point[input.ReadInt32()];
                        for (var p = 0; p < points.Length; p++)
                        {
                            points[p].X = input.ReadInt32();
                            points[p].Y = input.ReadInt32();
                        }
                        m.ObjectLayers[i].MapObjects[mo].Polygon = Polygon.FromPoints(points);
                    }
                    else
                        m.ObjectLayers[i].MapObjects[mo].Polygon = null;

                    props = input.ReadInt32();
                    m.ObjectLayers[i].MapObjects[mo].Properties = new Dictionary<string, Property>();
                    for (var p = 0; p < props; p++)
                    {
                        m.ObjectLayers[i].MapObjects[mo].Properties.Add(input.ReadString(), Property.Create(input.ReadString()));
                    }
                }

                props = input.ReadInt32();
                m.ObjectLayers[i].Properties = new Dictionary<string, Property>(props);
                for (var p = 0; p < props; p++)
                {
                    m.ObjectLayers[i].Properties.Add(input.ReadString(), Property.Create(input.ReadString()));
                }
            }

            m.LayerOrder = new LayerInfo[input.ReadInt32()];
            for (var li = 0; li < m.LayerOrder.Length; li++)
            {
                m.LayerOrder[li].Id = input.ReadInt32();
                m.LayerOrder[li].LayerType = input.ReadBoolean() ? LayerType.TileLayer : LayerType.ObjectLayer;
            }

            if (m.LoadTextures)
            {
                for (var i = 0; i < m.Tilesets.Length; i++)
                {
                    m.Tilesets[i].Texture = input.ContentManager.Load<Texture2D>(string.Format("{0}/{1:00}", input.AssetName, i));
                }
            }

            if (Map.EnableRendering)
                m.CreatePolygonTextures();

            return m;
        }
    }
}
