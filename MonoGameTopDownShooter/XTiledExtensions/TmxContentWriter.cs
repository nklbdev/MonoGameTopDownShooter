using System;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Microsoft.Xna.Framework.Graphics;
using XTiled;

namespace XTiledExtensions
{
    [ContentTypeWriter]
    public class TmxContentWriter : ContentTypeWriter<Map>
    {
        protected override void Write(ContentWriter output, Map value)
        {
            output.Write(value.Orientation == MapOrientation.Orthogonal);
            output.Write(value.Width);
            output.Write(value.Height);
            output.Write(value.TileHeight);
            output.Write(value.TileWidth);
            output.Write(value.Bounds.X);
            output.Write(value.Bounds.Y);
            output.Write(value.Bounds.Height);
            output.Write(value.Bounds.Width);
            output.Write(value.LoadTextures);
            output.Write(value.Tilesets.Length);
            foreach (var tileset in value.Tilesets)
            {
                output.Write(tileset.ImageFileName ?? string.Empty);
                output.Write(tileset.ImageHeight);
                output.Write(tileset.ImageWidth);
                output.Write(tileset.Margin);
                output.Write(tileset.Name ?? string.Empty);
                output.Write(tileset.Spacing);
                output.Write(tileset.TileHeight);
                output.Write(tileset.TileOffsetX);
                output.Write(tileset.TileOffsetY);
                output.Write(tileset.TileWidth);
                output.Write(tileset.ImageTransparentColor.HasValue);
                if (tileset.ImageTransparentColor.HasValue)
                {
                    var contentWriter = output;
                    var color = tileset.ImageTransparentColor.Value;
                    // ISSUE: explicit reference operation
                    var num = (int)@color.A;
                    contentWriter.Write((byte)num);
                }
                output.Write(tileset.Tiles.Length);
                foreach (var tile in tileset.Tiles)
                {
                    output.Write(tile.TilesetID);
                    output.Write(tile.Origin.X);
                    output.Write(tile.Origin.Y);
                    output.Write(tile.Source.X);
                    output.Write(tile.Source.Y);
                    output.Write(tile.Source.Height);
                    output.Write(tile.Source.Width);
                    output.Write(tile.Properties.Count);
                    foreach (var keyValuePair in tile.Properties)
                    {
                        output.Write(keyValuePair.Key ?? string.Empty);
                        output.Write(keyValuePair.Value.Value ?? string.Empty);
                    }
                }
                output.Write(tileset.Properties.Count);
                foreach (var keyValuePair in tileset.Properties)
                {
                    output.Write(keyValuePair.Key ?? string.Empty);
                    output.Write(keyValuePair.Value.Value ?? string.Empty);
                }
            }
            output.Write(value.Properties.Count);
            foreach (var keyValuePair in value.Properties)
            {
                output.Write(keyValuePair.Key ?? string.Empty);
                output.Write(keyValuePair.Value.Value ?? string.Empty);
            }
            output.Write(value.TileLayers.Count);
            foreach (var tileLayer in value.TileLayers)
            {
                output.Write(tileLayer.Name ?? string.Empty);
                output.Write(tileLayer.Opacity);
                // ISSUE: explicit reference operation
                output.Write(@tileLayer.OpacityColor.A);
                output.Write(tileLayer.Visible);
                output.Write(tileLayer.Properties.Count);
                foreach (var keyValuePair in tileLayer.Properties)
                {
                    output.Write(keyValuePair.Key ?? string.Empty);
                    output.Write(keyValuePair.Value.Value ?? string.Empty);
                }
                output.Write(tileLayer.Tiles.Length);
                foreach (var tileDataArray in tileLayer.Tiles)
                {
                    output.Write(tileDataArray.Length);
                    foreach (var tileData in tileDataArray)
                    {
                        output.Write(tileData != null);
                        if (tileData != null)
                        {
                            output.Write(tileData.Rotation);
                            output.Write(tileData.SourceID);
                            output.Write(tileData.Target.X);
                            output.Write(tileData.Target.Y);
                            output.Write(tileData.Target.Height);
                            output.Write(tileData.Target.Width);
                            output.Write(((Enum)tileData.Effects).HasFlag((SpriteEffects)1));
                            output.Write(((Enum)tileData.Effects).HasFlag((SpriteEffects)2));
                        }
                    }
                }
            }
            output.Write(value.SourceTiles.Length);
            foreach (var tile in value.SourceTiles)
            {
                output.Write(tile.Origin.X);
                output.Write(tile.Origin.Y);
                output.Write(tile.Source.X);
                output.Write(tile.Source.Y);
                output.Write(tile.Source.Height);
                output.Write(tile.Source.Width);
                output.Write(tile.TilesetID);
                output.Write(tile.Properties.Count);
                foreach (var keyValuePair in tile.Properties)
                {
                    output.Write(keyValuePair.Key ?? string.Empty);
                    output.Write(keyValuePair.Value.Value ?? string.Empty);
                }
            }
            output.Write(value.ObjectLayers.Count);
            foreach (var objectLayer in value.ObjectLayers)
            {
                output.Write(objectLayer.Name ?? string.Empty);
                output.Write(objectLayer.Opacity);
                // ISSUE: explicit reference operation
                output.Write(@objectLayer.OpacityColor.A);
                output.Write(objectLayer.Visible);
                output.Write(objectLayer.Color.HasValue);
                if (objectLayer.Color.HasValue)
                {
                    var contentWriter1 = output;
                    var color1 = objectLayer.Color.Value;
                    // ISSUE: explicit reference operation
                    var num1 = (int)@color1.R;
                    contentWriter1.Write((byte)num1);
                    var contentWriter2 = output;
                    var color2 = objectLayer.Color.Value;
                    // ISSUE: explicit reference operation
                    var num2 = (int)@color2.G;
                    contentWriter2.Write((byte)num2);
                    var contentWriter3 = output;
                    var color3 = objectLayer.Color.Value;
                    // ISSUE: explicit reference operation
                    var num3 = (int)@color3.B;
                    contentWriter3.Write((byte)num3);
                    var contentWriter4 = output;
                    var color4 = objectLayer.Color.Value;
                    // ISSUE: explicit reference operation
                    var num4 = (int)@color4.A;
                    contentWriter4.Write((byte)num4);
                }
                output.Write(objectLayer.MapObjects.Length);
                foreach (var mapObject in objectLayer.MapObjects)
                {
                    output.Write(mapObject.Bounds.X);
                    output.Write(mapObject.Bounds.Y);
                    output.Write(mapObject.Bounds.Height);
                    output.Write(mapObject.Bounds.Width);
                    output.Write(mapObject.Name ?? string.Empty);
                    output.Write(mapObject.Type ?? string.Empty);
                    output.Write(mapObject.Visible);
                    output.Write(mapObject.TileID.HasValue);
                    if (mapObject.TileID.HasValue)
                        output.Write(mapObject.TileID.Value);
                    output.Write(mapObject.Polyline != null);
                    if (mapObject.Polyline != null)
                    {
                        output.Write(mapObject.Polyline.Points.Length);
                        foreach (var point in mapObject.Polyline.Points)
                        {
                            output.Write(point.X);
                            output.Write(point.Y);
                        }
                    }
                    output.Write(mapObject.Polygon != null);
                    if (mapObject.Polygon != null)
                    {
                        output.Write(mapObject.Polygon.Points.Length);
                        foreach (var point in mapObject.Polygon.Points)
                        {
                            output.Write(point.X);
                            output.Write(point.Y);
                        }
                    }
                    output.Write(mapObject.Properties.Count);
                    foreach (var keyValuePair in mapObject.Properties)
                    {
                        output.Write(keyValuePair.Key ?? string.Empty);
                        output.Write(keyValuePair.Value.Value ?? string.Empty);
                    }
                }
                output.Write(objectLayer.Properties.Count);
                foreach (var keyValuePair in objectLayer.Properties)
                {
                    output.Write(keyValuePair.Key ?? string.Empty);
                    output.Write(keyValuePair.Value.Value ?? string.Empty);
                }
            }
            output.Write(value.LayerOrder.Length);
            foreach (var layerInfo in value.LayerOrder)
            {
                output.Write(layerInfo.ID);
                output.Write(layerInfo.LayerType == LayerType.TileLayer);
            }
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            //return "TmxContentReader, XTiledExtensions";
            return typeof(TmxContentReader).AssemblyQualifiedName;
        }

        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            //return "Map, XTiledExtensions";
            return typeof(Map).AssemblyQualifiedName;
        }
    }
}
