using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Polly.MonoGame.XTiledExtensions
{
    public sealed class MapReader : ContentTypeReader<Map>
    {
        protected override Map Read(ContentReader input, Map existingInstance)
        {
            var map = new Map
            {
                Orientation = input.ReadBoolean() ? MapOrientation.Orthogonal : MapOrientation.Isometric,
                Width = input.ReadInt32(),
                Height = input.ReadInt32(),
                TileHeight = input.ReadInt32(),
                TileWidth = input.ReadInt32(),
                Bounds =
                {
                    X = input.ReadInt32(),
                    Y = input.ReadInt32(),
                    Height = input.ReadInt32(),
                    Width = input.ReadInt32()
                },
                LoadTextures = input.ReadBoolean(),
                Tilesets = new Tileset[input.ReadInt32()]
            };
            for (var index1 = 0; index1 < map.Tilesets.Length; ++index1)
            {
                map.Tilesets[index1] = new Tileset
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
                    var white = Color.White;
                    // ISSUE: explicit reference operation
                    @white.A = input.ReadByte();
                    map.Tilesets[index1].ImageTransparentColor = white;
                }
                map.Tilesets[index1].Tiles = new Tile[input.ReadInt32()];
                for (var index2 = 0; index2 < map.Tilesets[index1].Tiles.Length; ++index2)
                {
                    map.Tilesets[index1].Tiles[index2] = new Tile
                    {
                        TilesetID = input.ReadInt32(),
                        Origin = {X = input.ReadSingle(), Y = input.ReadSingle()},
                        Source =
                        {
                            X = input.ReadInt32(),
                            Y = input.ReadInt32(),
                            Height = input.ReadInt32(),
                            Width = input.ReadInt32()
                        }
                    };
                    var num = input.ReadInt32();
                    map.Tilesets[index1].Tiles[index2].Properties = new Dictionary<string, Property>();
                    for (var index3 = 0; index3 < num; ++index3)
                        map.Tilesets[index1].Tiles[index2].Properties.Add(input.ReadString(), Property.Create(input.ReadString()));
                }
                var capacity = input.ReadInt32();
                map.Tilesets[index1].Properties = new Dictionary<string, Property>(capacity);
                for (var index2 = 0; index2 < capacity; ++index2)
                    map.Tilesets[index1].Properties.Add(input.ReadString(), Property.Create(input.ReadString()));
            }
            var capacity1 = input.ReadInt32();
            map.Properties = new Dictionary<string, Property>(capacity1);
            for (var index = 0; index < capacity1; ++index)
                map.Properties.Add(input.ReadString(), Property.Create(input.ReadString()));
            map.TileLayers = new TileLayerList();
            var num1 = input.ReadInt32();
            for (var index1 = 0; index1 < num1; ++index1)
            {
                map.TileLayers.Add(new TileLayer());
                map.TileLayers[index1].Name = input.ReadString();
                map.TileLayers[index1].Opacity = input.ReadSingle();
                map.TileLayers[index1].OpacityColor = Color.White;
                // ISSUE: explicit reference operation
                @map.TileLayers[index1].OpacityColor.A = input.ReadByte();
                map.TileLayers[index1].Visible = input.ReadBoolean();
                var capacity2 = input.ReadInt32();
                map.TileLayers[index1].Properties = new Dictionary<string, Property>(capacity2);
                for (var index2 = 0; index2 < capacity2; ++index2)
                    map.TileLayers[index1].Properties.Add(input.ReadString(), Property.Create(input.ReadString()));
                map.TileLayers[index1].Tiles = new TileData[input.ReadInt32()][];
                for (var index2 = 0; index2 < map.TileLayers[index1].Tiles.Length; ++index2)
                {
                    map.TileLayers[index1].Tiles[index2] = new TileData[input.ReadInt32()];
                    for (var index3 = 0; index3 < map.TileLayers[index1].Tiles[index2].Length; ++index3)
                    {
                        if (input.ReadBoolean())
                        {
                            map.TileLayers[index1].Tiles[index2][index3] = new TileData
                            {
                                Rotation = input.ReadSingle(),
                                SourceID = input.ReadInt32(),
                                Target =
                                {
                                    X = input.ReadInt32(),
                                    Y = input.ReadInt32(),
                                    Height = input.ReadInt32(),
                                    Width = input.ReadInt32()
                                }
                            };
                            var flag1 = input.ReadBoolean();
                            var flag2 = input.ReadBoolean();
                            if (flag2 && flag1)
                                map.TileLayers[index1].Tiles[index2][index3].Effects = (SpriteEffects)3;
                            else if (flag2)
                                map.TileLayers[index1].Tiles[index2][index3].Effects = (SpriteEffects)2;
                            else if (flag1)
                                map.TileLayers[index1].Tiles[index2][index3].Effects = (SpriteEffects)1;
                            else
                                map.TileLayers[index1].Tiles[index2][index3].Effects = 0;
                        }
                        else
                            map.TileLayers[index1].Tiles[index2][index3] = null;
                    }
                }
            }
            map.SourceTiles = new Tile[input.ReadInt32()];
            for (var index1 = 0; index1 < map.SourceTiles.Length; ++index1)
            {
                map.SourceTiles[index1] = new Tile
                {
                    Origin = {X = input.ReadSingle(), Y = input.ReadSingle()},
                    Source =
                    {
                        X = input.ReadInt32(),
                        Y = input.ReadInt32(),
                        Height = input.ReadInt32(),
                        Width = input.ReadInt32()
                    },
                    TilesetID = input.ReadInt32()
                };
                var capacity2 = input.ReadInt32();
                map.SourceTiles[index1].Properties = new Dictionary<string, Property>(capacity2);
                for (var index2 = 0; index2 < capacity2; ++index2)
                    map.SourceTiles[index1].Properties.Add(input.ReadString(), Property.Create(input.ReadString()));
            }
            var num2 = input.ReadInt32();
            map.ObjectLayers = new ObjectLayerList();
            for (var index1 = 0; index1 < num2; ++index1)
            {
                map.ObjectLayers.Add(new ObjectLayer());
                map.ObjectLayers[index1].Name = input.ReadString();
                map.ObjectLayers[index1].Opacity = input.ReadSingle();
                map.ObjectLayers[index1].OpacityColor = Color.White;
                // ISSUE: explicit reference operation
                @map.ObjectLayers[index1].OpacityColor.A = (input.ReadByte());
                map.ObjectLayers[index1].Visible = input.ReadBoolean();
                if (input.ReadBoolean())
                {
                    var white = Color.White;
                    // ISSUE: explicit reference operation
                    @white.R = (input.ReadByte());
                    // ISSUE: explicit reference operation
                    @white.G = (input.ReadByte());
                    // ISSUE: explicit reference operation
                    @white.B = (input.ReadByte());
                    // ISSUE: explicit reference operation
                    @white.A = (input.ReadByte());
                    map.ObjectLayers[index1].Color = white;
                }
                else
                    map.ObjectLayers[index1].Color = new Color?();
                map.ObjectLayers[index1].MapObjects = new MapObject[input.ReadInt32()];
                for (var index2 = 0; index2 < map.ObjectLayers[index1].MapObjects.Length; ++index2)
                {
                    map.ObjectLayers[index1].MapObjects[index2] = new MapObject
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
                        map.ObjectLayers[index1].MapObjects[index2].TileID = input.ReadInt32();
                    else
                        map.ObjectLayers[index1].MapObjects[index2].TileID = new int?();
                    if (input.ReadBoolean())
                    {
                        var pointArray = new Point[input.ReadInt32()];
                        for (var index3 = 0; index3 < pointArray.Length; ++index3)
                        {
                            pointArray[index3].X = input.ReadInt32();
                            pointArray[index3].Y = input.ReadInt32();
                        }
                        map.ObjectLayers[index1].MapObjects[index2].Polyline = Polyline.FromPoints(pointArray);
                    }
                    else
                        map.ObjectLayers[index1].MapObjects[index2].Polyline = null;
                    if (input.ReadBoolean())
                    {
                        var pointArray = new Point[input.ReadInt32()];
                        for (var index3 = 0; index3 < pointArray.Length; ++index3)
                        {
                            pointArray[index3].X = input.ReadInt32();
                            pointArray[index3].Y = input.ReadInt32();
                        }
                        map.ObjectLayers[index1].MapObjects[index2].Polygon = Polygon.FromPoints(pointArray);
                    }
                    else
                        map.ObjectLayers[index1].MapObjects[index2].Polygon = null;
                    var num3 = input.ReadInt32();
                    map.ObjectLayers[index1].MapObjects[index2].Properties = new Dictionary<string, Property>();
                    for (var index3 = 0; index3 < num3; ++index3)
                        map.ObjectLayers[index1].MapObjects[index2].Properties.Add(input.ReadString(), Property.Create(input.ReadString()));
                }
                var capacity2 = input.ReadInt32();
                map.ObjectLayers[index1].Properties = new Dictionary<string, Property>(capacity2);
                for (var index2 = 0; index2 < capacity2; ++index2)
                    map.ObjectLayers[index1].Properties.Add(input.ReadString(), Property.Create(input.ReadString()));
            }
            map.LayerOrder = new LayerInfo[input.ReadInt32()];
            for (var index = 0; index < map.LayerOrder.Length; ++index)
            {
                map.LayerOrder[index].ID = input.ReadInt32();
                map.LayerOrder[index].LayerType = input.ReadBoolean() ? LayerType.TileLayer : LayerType.ObjectLayer;
            }
            if (map.LoadTextures)
            {
                for (var index = 0; index < map.Tilesets.Length; ++index)
                    map.Tilesets[index].Texture = input.ContentManager.Load<Texture2D>(string.Format("{0}/{1:00}", input.AssetName, index));
            }
            if (Map._enableRendering)
                map.CreatePolygonTextures();
            return map;
        }
    }
}
