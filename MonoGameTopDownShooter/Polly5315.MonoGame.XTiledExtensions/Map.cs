using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Polly.MonoGame.XTiledExtensions
{
    public class Map
    {
        internal const float _lineThickness = 1f;
        internal static bool _enableRendering;
        internal static Texture2D _whiteTexture;
        public MapOrientation Orientation;
        public int Width;
        public int Height;
        public int TileWidth;
        public int TileHeight;
        public Rectangle Bounds;
        public Tileset[] Tilesets;
        public Dictionary<string, Property> Properties;
        public TileLayerList TileLayers;
        public ObjectLayerList ObjectLayers;
        public Tile[] SourceTiles;
        public bool LoadTextures;
        public LayerInfo[] LayerOrder;

        public static void InitObjectDrawing(GraphicsDevice graphicsDevice)
        {
            _whiteTexture = new Texture2D(graphicsDevice, 1, 1, false, 0);
            _whiteTexture.SetData(new Color[1]
            {
                Color.White
            });
            _enableRendering = true;
        }

        internal void CreatePolygonTextures()
        {
            for (var index1 = 0; index1 < ObjectLayers.Count; ++index1)
            {
                for (var index2 = 0; index2 < ObjectLayers[index1].MapObjects.Length; ++index2)
                {
                    if (ObjectLayers[index1].MapObjects[index2].Polygon != null)
                        ObjectLayers[index1].MapObjects[index2].Polygon.GenerateTexture(_whiteTexture.GraphicsDevice, Color.White);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle region)
        {
            Draw(spriteBatch, ref region, false);
        }

        public void Draw(SpriteBatch spriteBatch, ref Rectangle region)
        {
            Draw(spriteBatch, ref region, false);
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle region, bool drawHiddenLayers)
        {
            Draw(spriteBatch, ref region, drawHiddenLayers);
        }

        public void Draw(SpriteBatch spriteBatch, ref Rectangle region, bool drawHiddenLayers)
        {
            var txMin = region.X / TileWidth;
            var txMax = (region.X + region.Width) / TileWidth;
            var tyMin = region.Y / TileHeight;
            var tyMax = (region.Y + region.Height) / TileHeight;
            if (Orientation == MapOrientation.Isometric)
            {
                tyMax = tyMax * 2 + 1;
                txMax = txMax * 2 + 1;
            }
            for (var layerId = 0; layerId < TileLayers.Count; ++layerId)
            {
                if (TileLayers[layerId].Visible || drawHiddenLayers)
                    DrawLayer(spriteBatch, layerId, ref region, txMin, txMax, tyMin, tyMax, 0.0f);
            }
        }

        public void DrawLayer(SpriteBatch spriteBatch, int layerId, Rectangle region, float layerDepth)
        {
            DrawLayer(spriteBatch, layerId, ref region, layerDepth);
        }

        public void DrawLayer(SpriteBatch spriteBatch, int layerId, ref Rectangle region, float layerDepth)
        {
            var txMin = region.X / TileWidth;
            var txMax = (region.X + region.Width) / TileWidth;
            var tyMin = region.Y / TileHeight;
            var tyMax = (region.Y + region.Height) / TileHeight;
            if (Orientation == MapOrientation.Isometric)
            {
                tyMax = tyMax * 2 + 1;
                txMax = txMax * 2 + 1;
            }
            DrawLayer(spriteBatch, layerId, ref region, txMin, txMax, tyMin, tyMax, layerDepth);
        }

        private void DrawLayer(SpriteBatch spriteBatch, int layerId, ref Rectangle region, int txMin, int txMax, int tyMin, int tyMax, float layerDepth)
        {
            for (var index1 = tyMin; index1 <= tyMax; ++index1)
            {
                for (var index2 = txMin; index2 <= txMax; ++index2)
                {
                    if (index2 < TileLayers[layerId].Tiles.Length && index1 < TileLayers[layerId].Tiles[index2].Length && TileLayers[layerId].Tiles[index2][index1] != null)
                    {
                        var rectangle = TileLayers[layerId].Tiles[index2][index1].Target;
                        rectangle.X = rectangle.X - region.X;
                        rectangle.Y = rectangle.Y - region.Y;
                        spriteBatch.Draw(Tilesets[SourceTiles[TileLayers[layerId].Tiles[index2][index1].SourceID].TilesetID].Texture, rectangle, SourceTiles[TileLayers[layerId].Tiles[index2][index1].SourceID].Source, TileLayers[layerId].OpacityColor, TileLayers[layerId].Tiles[index2][index1].Rotation, SourceTiles[TileLayers[layerId].Tiles[index2][index1].SourceID].Origin, TileLayers[layerId].Tiles[index2][index1].Effects, layerDepth);
                    }
                }
            }
        }

        public void DrawObjectLayer(SpriteBatch spriteBatch, int objectLayerId, Rectangle region, float layerDepth)
        {
            DrawObjectLayer(spriteBatch, objectLayerId, ref region, layerDepth);
        }

        public void DrawObjectLayer(SpriteBatch spriteBatch, int objectLayerId, ref Rectangle region, float layerDepth)
        {
            if (_whiteTexture == null)
                throw new Exception("Map.InitObjectDrawing must be called before Map is loaded to enable object rendering");
            for (var objectId = 0; objectId < ObjectLayers[objectLayerId].MapObjects.Length; ++objectId)
            {
                // ISSUE: explicit reference operation
                // ISSUE: explicit reference operation
                if (@region.Contains(ObjectLayers[objectLayerId].MapObjects[objectId].Bounds) || @region.Intersects(ObjectLayers[objectLayerId].MapObjects[objectId].Bounds))
                    DrawMapObject(spriteBatch, objectLayerId, objectId, ref region, layerDepth);
            }
        }

        public void DrawTileObject(SpriteBatch spriteBatch, int objectLayerId, int objectId, Rectangle region, float layerDepth, Color color)
        {
            DrawTileObject(spriteBatch, objectLayerId, objectId, ref region, layerDepth, ref color);
        }

        public void DrawTileObject(SpriteBatch spriteBatch, int objectLayerId, int objectId, ref Rectangle region, float layerDepth, ref Color color)
        {
            spriteBatch.Draw(Tilesets[SourceTiles[ObjectLayers[objectLayerId].MapObjects[objectId].TileID.Value].TilesetID].Texture, Translate(ObjectLayers[objectLayerId].MapObjects[objectId].Bounds, region), SourceTiles[ObjectLayers[objectLayerId].MapObjects[objectId].TileID.Value].Source, color, 0.0f, SourceTiles[ObjectLayers[objectLayerId].MapObjects[objectId].TileID.Value].Origin, 0, layerDepth);
        }

        public void DrawMapObject(SpriteBatch spriteBatch, int objectLayerId, int objectId, Rectangle region, float layerDepth)
        {
            DrawMapObject(spriteBatch, objectLayerId, objectId, ref region, layerDepth);
        }

        public void DrawMapObject(SpriteBatch spriteBatch, int objectLayerId, int objectId, ref Rectangle region, float layerDepth)
        {
            var color = ObjectLayers[objectLayerId].Color ?? ObjectLayers[objectLayerId].OpacityColor;
            var fillColor = color;
            // ISSUE: explicit reference operation
            // ISSUE: variable of a reference type
            var local = @fillColor;
            int num = (byte)(local.A / 4U);
            local.A = ((byte)num);
            if (ObjectLayers[objectLayerId].MapObjects[objectId].Polyline != null)
                ObjectLayers[objectLayerId].MapObjects[objectId].Polyline.Draw(spriteBatch, region, _whiteTexture, 1f, color, layerDepth);
            else if (ObjectLayers[objectLayerId].MapObjects[objectId].Polygon != null)
                ObjectLayers[objectLayerId].MapObjects[objectId].Polygon.DrawFilled(spriteBatch, region, _whiteTexture, 1f, color, fillColor, layerDepth);
            else if (ObjectLayers[objectLayerId].MapObjects[objectId].TileID.HasValue)
                DrawTileObject(spriteBatch, objectLayerId, objectId, ref region, layerDepth, ref color);
            else
                DrawRectangle(spriteBatch, ref ObjectLayers[objectLayerId].MapObjects[objectId].Bounds, ref region, layerDepth, ref color, ref fillColor);
        }

        public static void DrawRectangle(SpriteBatch spriteBatch, Rectangle rect, Rectangle region, float layerDepth, Color linecolor, Color fillColor)
        {
            DrawRectangle(spriteBatch, ref rect, ref region, layerDepth, ref linecolor, ref fillColor);
        }

        public static void DrawRectangle(SpriteBatch spriteBatch, ref Rectangle rect, ref Rectangle region, float layerDepth, ref Color linecolor, ref Color fillColor)
        {
            if (_whiteTexture == null)
                throw new Exception("Map.InitObjectDrawing must be called before Map is loaded to enable object rendering");
            var rectangle = Translate(rect, region);
            spriteBatch.Draw(_whiteTexture, rectangle, new Rectangle?(), fillColor, 0.0f, Vector2.Zero, 0, layerDepth);
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            Line.Draw(spriteBatch, Line.FromPoints(new Vector2(@rect.Right, @rect.Top), new Vector2(@rect.Left, @rect.Top)), region, _whiteTexture, 1f, linecolor, layerDepth);
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            Line.Draw(spriteBatch, Line.FromPoints(new Vector2(@rect.Left, @rect.Top), new Vector2(@rect.Left, @rect.Bottom)), region, _whiteTexture, 1f, linecolor, layerDepth);
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            Line.Draw(spriteBatch, Line.FromPoints(new Vector2(@rect.Left, @rect.Bottom), new Vector2(@rect.Right, @rect.Bottom)), region, _whiteTexture, 1f, linecolor, layerDepth);
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            Line.Draw(spriteBatch, Line.FromPoints(new Vector2(@rect.Right, @rect.Bottom), new Vector2(@rect.Right, @rect.Top)), region, _whiteTexture, 1f, linecolor, layerDepth);
        }

        public static Rectangle Translate(Rectangle location, Rectangle relativeTo)
        {
            location.X = location.X - relativeTo.X;
            location.Y = location.Y - relativeTo.Y;
            return location;
        }

        public static void Translate(ref Rectangle location, ref Rectangle relativeTo)
        {
            location.X = location.X - relativeTo.X;
            location.Y = location.Y - relativeTo.Y;
        }

        public static Point Translate(Point location, Rectangle relativeTo)
        {
            location.X = location.X - relativeTo.X;
            location.Y = location.Y - relativeTo.Y;
            return location;
        }

        public static void Translate(ref Point location, ref Rectangle relativeTo)
        {
            location.X = location.X - relativeTo.X;
            location.Y = location.Y - relativeTo.Y;
        }

        public static Vector2 Translate(Vector2 location, Rectangle relativeTo)
        {
            location.X = (float) (location.X - (double)relativeTo.X);
            location.Y = (float) (location.Y - (double)relativeTo.Y);
            return location;
        }

        public static void Translate(ref Vector2 location, ref Rectangle relativeTo)
        {
            location.X = (float) (location.X - (double)relativeTo.X);
            location.Y = (float) (location.Y - (double)relativeTo.Y);
        }

        public IEnumerable<MapObject> GetObjectsInRegion(Rectangle region)
        {
            return GetObjectsInRegion(ref region);
        }

        public IEnumerable<MapObject> GetObjectsInRegion(ref Rectangle region)
        {
            var list = new List<MapObject>();
            for (var objectLayerId = 0; objectLayerId < ObjectLayers.Count; ++objectLayerId)
                list.AddRange(GetObjectsInRegion(objectLayerId, ref region));
            return list;
        }

        public IEnumerable<MapObject> GetObjectsInRegion(int objectLayerId, Rectangle region)
        {
            return GetObjectsInRegion(objectLayerId, ref region);
        }

        public IEnumerable<MapObject> GetObjectsInRegion(int objectLayerId, ref Rectangle region)
        {
            var list = new List<MapObject>();
            for (var index = 0; index < ObjectLayers[objectLayerId].MapObjects.Length; ++index)
            {
                // ISSUE: explicit reference operation
                // ISSUE: explicit reference operation
                if (@region.Contains(ObjectLayers[objectLayerId].MapObjects[index].Bounds) || @region.Intersects(ObjectLayers[objectLayerId].MapObjects[index].Bounds))
                    list.Add(ObjectLayers[objectLayerId].MapObjects[index]);
            }
            return list;
        }

        public IEnumerable<TileData> GetTilesInRegion(Rectangle region)
        {
            return GetTilesInRegion(ref region);
        }

        public IEnumerable<TileData> GetTilesInRegion(ref Rectangle region)
        {
            var list = new List<TileData>();
            var num1 = region.X / TileWidth;
            var num2 = (region.X + region.Width) / TileWidth;
            var num3 = region.Y / TileHeight;
            var num4 = (region.Y + region.Height) / TileHeight;
            if (Orientation == MapOrientation.Isometric)
            {
                num4 = num4 * 2 + 1;
                num2 = num2 * 2 + 1;
            }
            for (var index1 = 0; index1 < TileLayers.Count; ++index1)
            {
                for (var index2 = num3; index2 <= num4; ++index2)
                {
                    for (var index3 = num1; index3 <= num2; ++index3)
                    {
                        if (TileLayers[index1].Tiles[index3][index2] != null)
                            list.Add(TileLayers[index1].Tiles[index3][index2]);
                    }
                }
            }
            return list;
        }

        public IEnumerable<TileData> GetTilesInRegion(int tileLayerId, Rectangle region)
        {
            return GetTilesInRegion(tileLayerId, ref region);
        }

        public IEnumerable<TileData> GetTilesInRegion(int tileLayerId, ref Rectangle region)
        {
            var list = new List<TileData>();
            var num1 = region.X / TileWidth;
            var num2 = (region.X + region.Width) / TileWidth;
            var num3 = region.Y / TileHeight;
            var num4 = (region.Y + region.Height) / TileHeight;
            if (Orientation == MapOrientation.Isometric)
            {
                num4 = num4 * 2 + 1;
                num2 = num2 * 2 + 1;
            }
            for (var index1 = num3; index1 <= num4; ++index1)
            {
                for (var index2 = num1; index2 <= num2; ++index2)
                {
                    if (TileLayers[tileLayerId].Tiles[index2][index1] != null)
                        list.Add(TileLayers[tileLayerId].Tiles[index2][index1]);
                }
            }
            return list;
        }
    }
}