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
            foreach (var ts in value.Tilesets)
            {
                output.Write(ts.ImageFileName ?? string.Empty);
                output.Write(ts.ImageHeight);
                output.Write(ts.ImageWidth);
                output.Write(ts.Margin);
                output.Write(ts.Name ?? string.Empty);
                output.Write(ts.Spacing);
                output.Write(ts.TileHeight);
                output.Write(ts.TileOffsetX);
                output.Write(ts.TileOffsetY);
                output.Write(ts.TileWidth);

                output.Write(ts.ImageTransparentColor.HasValue);
                if (ts.ImageTransparentColor.HasValue)
                    output.Write(ts.ImageTransparentColor.Value.A);

                output.Write(ts.Tiles.Length);
                foreach (var t in ts.Tiles)
                {
                    output.Write(t.TilesetId);
                    output.Write(t.Origin.X);
                    output.Write(t.Origin.Y);
                    output.Write(t.Source.X);
                    output.Write(t.Source.Y);
                    output.Write(t.Source.Height);
                    output.Write(t.Source.Width);

                    output.Write(t.Properties.Count);
                    foreach (var kv in t.Properties)
                    {
                        output.Write(kv.Key ?? string.Empty);
                        output.Write(kv.Value.Value ?? string.Empty);
                    }
                }

                output.Write(ts.Properties.Count);
                foreach (var kv in ts.Properties)
                {
                    output.Write(kv.Key ?? string.Empty);
                    output.Write(kv.Value.Value ?? string.Empty);
                }
            }

            output.Write(value.Properties.Count);
            foreach (var kv in value.Properties)
            {
                output.Write(kv.Key ?? string.Empty);
                output.Write(kv.Value.Value ?? string.Empty);
            }

            output.Write(value.TileLayers.Count);
            foreach (var layer in value.TileLayers)
            {
                output.Write(layer.Name ?? string.Empty);
                output.Write(layer.Opacity);
                output.Write(layer.OpacityColor.A);
                output.Write(layer.Visible);

                output.Write(layer.Properties.Count);
                foreach (var kv in layer.Properties)
                {
                    output.Write(kv.Key ?? string.Empty);
                    output.Write(kv.Value.Value ?? string.Empty);
                }

                output.Write(layer.Tiles.Length);
                foreach (var row in layer.Tiles)
                {
                    output.Write(row.Length);
                    foreach (var tile in row)
                    {
                        output.Write(tile != null);

                        if (tile == null)
                            continue;

                        output.Write(tile.Rotation);
                        output.Write(tile.SourceId);
                        output.Write(tile.Target.X);
                        output.Write(tile.Target.Y);
                        output.Write(tile.Target.Height);
                        output.Write(tile.Target.Width);
                        output.Write(tile.Effects.HasFlag(SpriteEffects.FlipHorizontally));
                        output.Write(tile.Effects.HasFlag(SpriteEffects.FlipVertically));
                    }
                }
            }

            output.Write(value.SourceTiles.Length);
            foreach (var t in value.SourceTiles)
            {
                output.Write(t.Origin.X);
                output.Write(t.Origin.Y);
                output.Write(t.Source.X);
                output.Write(t.Source.Y);
                output.Write(t.Source.Height);
                output.Write(t.Source.Width);
                output.Write(t.TilesetId);

                output.Write(t.Properties.Count);
                foreach (var kv in t.Properties)
                {
                    output.Write(kv.Key ?? string.Empty);
                    output.Write(kv.Value.Value ?? string.Empty);
                }
            }

            output.Write(value.ObjectLayers.Count);
            foreach (var ol in value.ObjectLayers)
            {
                output.Write(ol.Name ?? string.Empty);
                output.Write(ol.Opacity);
                output.Write(ol.OpacityColor.A);
                output.Write(ol.Visible);

                output.Write(ol.Color.HasValue);
                if (ol.Color.HasValue)
                {
                    output.Write(ol.Color.Value.R);
                    output.Write(ol.Color.Value.G);
                    output.Write(ol.Color.Value.B);
                    output.Write(ol.Color.Value.A);
                }

                output.Write(ol.MapObjects.Length);
                foreach (var m in ol.MapObjects)
                {
                    output.Write(m.Bounds.X);
                    output.Write(m.Bounds.Y);
                    output.Write(m.Bounds.Height);
                    output.Write(m.Bounds.Width);
                    output.Write(m.Name ?? string.Empty);
                    output.Write(m.Type ?? string.Empty);
                    output.Write(m.Visible);

                    output.Write(m.TileId.HasValue);
                    if (m.TileId.HasValue)
                        output.Write(m.TileId.Value);

                    output.Write(m.Polyline != null);
                    if (m.Polyline != null)
                    {
                        output.Write(m.Polyline.Points.Length);
                        foreach (var p in m.Polyline.Points)
                        {
                            output.Write(p.X);
                            output.Write(p.Y);
                        }
                    }

                    output.Write(m.Polygon != null);
                    if (m.Polygon != null)
                    {
                        output.Write(m.Polygon.Points.Length);
                        foreach (var p in m.Polygon.Points)
                        {
                            output.Write(p.X);
                            output.Write(p.Y);
                        }
                    }

                    output.Write(m.Properties.Count);
                    foreach (var kv in m.Properties)
                    {
                        output.Write(kv.Key ?? string.Empty);
                        output.Write(kv.Value.Value ?? string.Empty);
                    }
                }

                output.Write(ol.Properties.Count);
                foreach (var kv in ol.Properties)
                {
                    output.Write(kv.Key ?? string.Empty);
                    output.Write(kv.Value.Value ?? string.Empty);
                }
            }

            output.Write(value.LayerOrder.Length);
            foreach (var lo in value.LayerOrder)
            {
                output.Write(lo.Id);
                output.Write(lo.LayerType == LayerType.TileLayer);
            }

        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(TmxContentReader).AssemblyQualifiedName;
        }

        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return typeof(Map).AssemblyQualifiedName;
        }
    }
}
