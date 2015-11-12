using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameProxies;

namespace XTiled
{
    /// <summary>
    /// An XTiled TMX Map
    /// </summary>
    public class Map
    {

        internal static bool EnableRendering;
        internal static Texture2D WhiteTexture;
        internal const float LineThickness = 1.0f;

        /// <summary>
        /// Orientation of the map.
        /// </summary>
        public MapOrientation Orientation;
        /// <summary>
        /// Width, in tiles, of the map
        /// </summary>
        public int Width;
        /// <summary>
        /// Height, in tiles, of the map
        /// </summary>
        public int Height;
        /// <summary>
        /// Pixel width of a single tile
        /// </summary>
        public int TileWidth;
        /// <summary>
        /// Pixel height of a single tile
        /// </summary>
        public int TileHeight;
        /// <summary>
        /// Size of the map in pixels
        /// </summary>
        public Rectangle Bounds;
        /// <summary>
        /// Tilesets associated with this map
        /// </summary>
        public Tileset[] Tilesets;
        /// <summary>
        /// Custom properties
        /// </summary>
        public Dictionary<string, Property> Properties;
        /// <summary>
        /// Ordered collection of tile layers; first is the bottom layer
        /// </summary>
        public TileLayerList TileLayers;
        /// <summary>
        /// Ordered collection of object layers; first is the bottom layer
        /// </summary>
        public ObjectLayerList ObjectLayers;
        /// <summary>
        /// List of all source tiles from tilesets
        /// </summary>
        public Tile[] SourceTiles;
        /// <summary>
        /// True if XTiled loaded tileset textures during map load
        /// </summary>
        public bool LoadTextures;
        /// <summary>
        /// Order of tile and object layers combined, first is the bottom layer
        /// </summary>
        public LayerInfo[] LayerOrder;

        /// <summary>
        /// Enables rendering of map objects
        /// </summary>
        /// <param name="graphicsDevice">The graphics device to us in creating textures to support object rendering</param>
        public static void InitObjectDrawing(GraphicsDevice graphicsDevice)
        {
            WhiteTexture = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            WhiteTexture.SetData(new[] { Color.White });
            EnableRendering = true;
        }

        internal void CreatePolygonTextures()
        {
            foreach (var mapObject in ObjectLayers.SelectMany(ol => ol.MapObjects.Where(mo => mo.Polygon != null)))
                mapObject.Polygon.GenerateTexture(WhiteTexture.GraphicsDevice, Color.White);
        }

        /// <summary>
        /// Draws all visible tile layers
        /// </summary>
        /// <param name="spriteBatch">XNA SpriteBatch instance; SpriteBatch.Begin() must be called before using this method</param>
        /// <param name="region">Region of the map in pixels to draw</param>
        public void Draw(IImmutableSpriteBatch spriteBatch, Rectangle region, float layerDepth = 0f)
        {
            Draw(spriteBatch, ref region, false, layerDepth);
        }

        /// <summary>
        /// Draws all visible tile layers
        /// </summary>
        /// <param name="spriteBatch">XNA SpriteBatch instance; SpriteBatch.Begin() must be called before using this method</param>
        /// <param name="region">Region of the map in pixels to draw</param>
        public void Draw(IImmutableSpriteBatch spriteBatch, ref Rectangle region, float layerDepth = 0f)
        {
            Draw(spriteBatch, ref region, false, layerDepth);
        }

        /// <summary>
        /// Draws all visible tile layers
        /// </summary>
        /// <param name="spriteBatch">XNA SpriteBatch instance; SpriteBatch.Begin() must be called before using this method</param>
        /// <param name="region">Region of the map in pixels to draw</param>
        /// <param name="drawHiddenLayers">If true, draws layers regardless of TileLayer.Visible flag</param>
        public void Draw(IImmutableSpriteBatch spriteBatch, Rectangle region, bool drawHiddenLayers, float layerDepth = 0f)
        {
            Draw(spriteBatch, ref region, drawHiddenLayers, layerDepth);
        }

        /// <summary>
        /// Draws all visible tile layers
        /// </summary>
        /// <param name="spriteBatch">XNA SpriteBatch instance; SpriteBatch.Begin() must be called before using this method</param>
        /// <param name="region">Region of the map in pixels to draw</param>
        /// <param name="drawHiddenLayers">If true, draws layers regardless of TileLayer.Visible flag</param>
        public void Draw(IImmutableSpriteBatch spriteBatch, ref Rectangle region, bool drawHiddenLayers, float layerDepth = 0)
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

            for (var l = 0; l < TileLayers.Count; l++)
                if (TileLayers[l].Visible || drawHiddenLayers)
                    DrawLayer(spriteBatch, l, ref region, txMin, txMax, tyMin, tyMax, layerDepth);
        }

        /// <summary>
        /// Draws given tile layer
        /// </summary>
        /// <param name="spriteBatch">XNA SpriteBatch instance; SpriteBatch.Begin() must be called before using this method</param>
        /// <param name="layerId">Index of the layer to draw in the Map.TileLayers collection</param>
        /// <param name="region">Region of the map in pixels to draw</param>
        /// <param name="layerDepth">LayerDepth value to pass to SpriteBatch</param>
        public void DrawLayer(IImmutableSpriteBatch spriteBatch, int layerId, Rectangle region, float layerDepth)
        {
            DrawLayer(spriteBatch, layerId, ref region, layerDepth);
        }

        /// <summary>
        /// Draws given tile layer
        /// </summary>
        /// <param name="spriteBatch">XNA SpriteBatch instance; SpriteBatch.Begin() must be called before using this method</param>
        /// <param name="layerId">Index of the layer to draw in the Map.TileLayers collection</param>
        /// <param name="region">Region of the map in pixels to draw</param>
        /// <param name="layerDepth">LayerDepth value to pass to SpriteBatch</param>
        public void DrawLayer(IImmutableSpriteBatch spriteBatch, int layerId, ref Rectangle region, float layerDepth)
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

        private void DrawLayer(IImmutableSpriteBatch spriteBatch, int layerId, ref Rectangle region, int txMin, int txMax, int tyMin, int tyMax, float layerDepth)
        {
            for (var y = tyMin; y <= tyMax; y++)
            {
                for (var x = txMin; x <= txMax; x++)
                {
                    if (x >= TileLayers[layerId].Tiles.Length ||
                        y >= TileLayers[layerId].Tiles[x].Length ||
                        TileLayers[layerId].Tiles[x][y] == null)
                        continue;

                    var tileTarget = TileLayers[layerId].Tiles[x][y].Target;
                    tileTarget.X = tileTarget.X - region.X;
                    tileTarget.Y = tileTarget.Y - region.Y;

                    spriteBatch.Draw(
                        Tilesets[SourceTiles[TileLayers[layerId].Tiles[x][y].SourceId].TilesetId].Texture,
                        tileTarget,
                        SourceTiles[TileLayers[layerId].Tiles[x][y].SourceId].Source,
                        TileLayers[layerId].OpacityColor,
                        TileLayers[layerId].Tiles[x][y].Rotation,
                        SourceTiles[TileLayers[layerId].Tiles[x][y].SourceId].Origin,
                        TileLayers[layerId].Tiles[x][y].Effects,
                        layerDepth);
                }
            }
        }

        /// <summary>
        /// Draws all objects on the given object layer
        /// </summary>
        /// <param name="spriteBatch">XNA SpriteBatch instance; SpriteBatch.Begin() must be called before using this method</param>
        /// <param name="objectLayerId">Index of the layer to draw in the Map.ObjectLayers collection</param>
        /// <param name="region">Region of the map in pixels to draw</param>
        /// <param name="layerDepth">LayerDepth value to pass to SpriteBatch</param>
        public void DrawObjectLayer(IImmutableSpriteBatch spriteBatch, int objectLayerId, Rectangle region, float layerDepth)
        {
            DrawObjectLayer(spriteBatch, objectLayerId, ref region, layerDepth);
        }

        /// <summary>
        /// Draws all objects on the given object layer
        /// </summary>
        /// <param name="spriteBatch">XNA SpriteBatch instance; SpriteBatch.Begin() must be called before using this method</param>
        /// <param name="objectLayerId">Index of the layer to draw in the Map.ObjectLayers collection</param>
        /// <param name="region">Region of the map in pixels to draw</param>
        /// <param name="layerDepth">LayerDepth value to pass to SpriteBatch</param>
        public void DrawObjectLayer(IImmutableSpriteBatch spriteBatch, int objectLayerId, ref Rectangle region, float layerDepth)
        {
            if (WhiteTexture == null)
            {
                throw new Exception("Map.InitObjectDrawing must be called before Map is loaded to enable object rendering");
            }

            for (var o = 0; o < ObjectLayers[objectLayerId].MapObjects.Length; o++)
                if (region.Contains(ObjectLayers[objectLayerId].MapObjects[o].Bounds) ||
                    region.Intersects(ObjectLayers[objectLayerId].MapObjects[o].Bounds))
                    DrawMapObject(spriteBatch, objectLayerId, o, ref region, layerDepth);
        }

        /// <summary>
        /// Method to draw a MapObject that represents a tile object
        /// </summary>
        /// <param name="spriteBatch">XNA SpriteBatch instance; SpriteBatch.Begin() must be called before using this method</param>
        /// <param name="objectLayerId">Index of the layer to draw in the Map.ObjectLayers collection</param>
        /// <param name="objectId">Index of the object to draw in the Map.ObjectLayers.MapObjects collection</param>
        /// <param name="region">Region of the map in pixels to draw</param>
        /// <param name="layerDepth">LayerDepth value to pass to SpriteBatch</param>
        /// <param name="color">Color of the object</param>
        public void DrawTileObject(IImmutableSpriteBatch spriteBatch, int objectLayerId, int objectId, Rectangle region, float layerDepth, Color color)
        {
            DrawTileObject(spriteBatch, objectLayerId, objectId, ref region, layerDepth, ref color);
        }

        /// <summary>
        /// Method to draw a MapObject that represents a tile object
        /// </summary>
        /// <param name="spriteBatch">XNA SpriteBatch instance; SpriteBatch.Begin() must be called before using this method</param>
        /// <param name="objectLayerId">Index of the layer to draw in the Map.ObjectLayers collection</param>
        /// <param name="objectId">Index of the object to draw in the Map.ObjectLayers.MapObjects collection</param>
        /// <param name="region">Region of the map in pixels to draw</param>
        /// <param name="layerDepth">LayerDepth value to pass to SpriteBatch</param>
        /// <param name="color">Color of the object</param>
        public void DrawTileObject(IImmutableSpriteBatch spriteBatch, int objectLayerId, int objectId, ref Rectangle region, float layerDepth, ref Color color)
        {
            spriteBatch.Draw(
                Tilesets[SourceTiles[ObjectLayers[objectLayerId].MapObjects[objectId].TileId.Value].TilesetId].Texture,
                Translate(ObjectLayers[objectLayerId].MapObjects[objectId].Bounds, region),
                SourceTiles[ObjectLayers[objectLayerId].MapObjects[objectId].TileId.Value].Source,
                color,
                0,
                SourceTiles[ObjectLayers[objectLayerId].MapObjects[objectId].TileId.Value].Origin,
                SpriteEffects.None,
                layerDepth);
        }

        /// <summary>
        /// Method to draw a MapObject
        /// </summary>
        /// <param name="spriteBatch">XNA SpriteBatch instance; SpriteBatch.Begin() must be called before using this method</param>
        /// <param name="objectLayerId">Index of the layer to draw in the Map.ObjectLayers collection</param>
        /// <param name="objectId">Index of the object to draw in the Map.ObjectLayers.MapObjects collection</param>
        /// <param name="region">Region of the map in pixels to draw</param>
        /// <param name="layerDepth">LayerDepth value to pass to SpriteBatch</param>
        public void DrawMapObject(IImmutableSpriteBatch spriteBatch, int objectLayerId, int objectId, Rectangle region, float layerDepth)
        {
            DrawMapObject(spriteBatch, objectLayerId, objectId, ref region, layerDepth);
        }

        /// <summary>
        /// Method to draw a MapObject
        /// </summary>
        /// <param name="spriteBatch">XNA SpriteBatch instance; SpriteBatch.Begin() must be called before using this method</param>
        /// <param name="objectLayerId">Index of the layer to draw in the Map.ObjectLayers collection</param>
        /// <param name="objectId">Index of the object to draw in the Map.ObjectLayers.MapObjects collection</param>
        /// <param name="region">Region of the map in pixels to draw</param>
        /// <param name="layerDepth">LayerDepth value to pass to SpriteBatch</param>
        public void DrawMapObject(IImmutableSpriteBatch spriteBatch, int objectLayerId, int objectId, ref Rectangle region, float layerDepth)
        {
            var color = ObjectLayers[objectLayerId].Color ?? ObjectLayers[objectLayerId].OpacityColor;
            var fillColor = color;
            fillColor.A /= 4;

            if (ObjectLayers[objectLayerId].MapObjects[objectId].Polyline != null)
                ObjectLayers[objectLayerId].MapObjects[objectId].Polyline.Draw(spriteBatch, region, WhiteTexture, LineThickness, color, layerDepth);
            else if (ObjectLayers[objectLayerId].MapObjects[objectId].Polygon != null)
                ObjectLayers[objectLayerId].MapObjects[objectId].Polygon.DrawFilled(spriteBatch, region, WhiteTexture, LineThickness, color, fillColor, layerDepth);
            else if (ObjectLayers[objectLayerId].MapObjects[objectId].TileId.HasValue)
                DrawTileObject(spriteBatch, objectLayerId, objectId, ref region, layerDepth, ref color);
            else
                DrawRectangle(spriteBatch, ref ObjectLayers[objectLayerId].MapObjects[objectId].Bounds, ref region, layerDepth, ref color, ref fillColor);
        }

        /// <summary>
        /// Method to draw a Rectangle
        /// </summary>
        /// <param name="spriteBatch">XNA SpriteBatch instance; SpriteBatch.Begin() must be called before using this method</param>
        /// <param name="rect">The Rectangle to draw, in map pixels</param>
        /// <param name="region">Region of the map in pixels currently visible</param>
        /// <param name="layerDepth">LayerDepth value to pass to SpriteBatch</param>
        /// <param name="linecolor">Color of the Rectangle border</param>
        /// <param name="fillColor">Color to fill the Rectangle with</param>
        public static void DrawRectangle(IImmutableSpriteBatch spriteBatch, Rectangle rect, Rectangle region, float layerDepth, Color linecolor, Color fillColor)
        {
            DrawRectangle(spriteBatch, ref rect, ref region, layerDepth, ref linecolor, ref fillColor);
        }

        /// <summary>
        /// Method to draw a Rectangle
        /// </summary>
        /// <param name="spriteBatch">XNA SpriteBatch instance; SpriteBatch.Begin() must be called before using this method</param>
        /// <param name="rect">The Rectangle to draw, in map pixels</param>
        /// <param name="region">Region of the map in pixels currently visible</param>
        /// <param name="layerDepth">LayerDepth value to pass to SpriteBatch</param>
        /// <param name="linecolor">Color of the Rectangle border</param>
        /// <param name="fillColor">Color to fill the Rectangle with</param>
        public static void DrawRectangle(IImmutableSpriteBatch spriteBatch, ref Rectangle rect, ref Rectangle region, float layerDepth, ref Color linecolor, ref Color fillColor)
        {
            if (WhiteTexture == null)
            {
                throw new Exception("Map.InitObjectDrawing must be called before Map is loaded to enable object rendering");
            }

            var target = Translate(rect, region);
            spriteBatch.Draw(WhiteTexture, target, null, fillColor, 0, Vector2.Zero, SpriteEffects.None, layerDepth);
            Line.Draw(spriteBatch, Line.FromPoints(new Vector2(rect.Right, rect.Top), new Vector2(rect.Left, rect.Top)), region, WhiteTexture, LineThickness, linecolor, layerDepth);
            Line.Draw(spriteBatch, Line.FromPoints(new Vector2(rect.Left, rect.Top), new Vector2(rect.Left, rect.Bottom)), region, WhiteTexture, LineThickness, linecolor, layerDepth);
            Line.Draw(spriteBatch, Line.FromPoints(new Vector2(rect.Left, rect.Bottom), new Vector2(rect.Right, rect.Bottom)), region, WhiteTexture, LineThickness, linecolor, layerDepth);
            Line.Draw(spriteBatch, Line.FromPoints(new Vector2(rect.Right, rect.Bottom), new Vector2(rect.Right, rect.Top)), region, WhiteTexture, LineThickness, linecolor, layerDepth);
        }

        /// <summary>
        /// Translates a location to screen space
        /// </summary>
        /// <param name="location">The location in map pixel coordinates</param>
        /// <param name="relativeTo">Region of the map that is on screen</param>
        /// <returns>A location converted to screen space</returns>
        public static Rectangle Translate(Rectangle location, Rectangle relativeTo)
        {
            location.X = location.X - relativeTo.X;
            location.Y = location.Y - relativeTo.Y;
            return location;
        }

        /// <summary>
        /// Translates a location to screen space
        /// </summary>
        /// <param name="location">The location in map pixel coordinates</param>
        /// <param name="relativeTo">Region of the map that is on screen</param>
        public static void Translate(ref Rectangle location, ref Rectangle relativeTo)
        {
            location.X = location.X - relativeTo.X;
            location.Y = location.Y - relativeTo.Y;
        }

        /// <summary>
        /// Translates a location to screen space
        /// </summary>
        /// <param name="location">The location in map pixel coordinates</param>
        /// <param name="relativeTo">Region of the map that is on screen</param>
        /// <returns>A location converted to screen space</returns>
        public static Point Translate(Point location, Rectangle relativeTo)
        {
            location.X = location.X - relativeTo.X;
            location.Y = location.Y - relativeTo.Y;
            return location;
        }

        /// <summary>
        /// Translates a location to screen space
        /// </summary>
        /// <param name="location">The location in map pixel coordinates</param>
        /// <param name="relativeTo">Region of the map that is on screen</param>
        public static void Translate(ref Point location, ref Rectangle relativeTo)
        {
            location.X = location.X - relativeTo.X;
            location.Y = location.Y - relativeTo.Y;
        }

        /// <summary>
        /// Translates a location to screen space
        /// </summary>
        /// <param name="location">The location in map pixel coordinates</param>
        /// <param name="relativeTo">Region of the map that is on screen</param>
        /// <returns>A location converted to screen space</returns>
        public static Vector2 Translate(Vector2 location, Rectangle relativeTo)
        {
            location.X = location.X - relativeTo.X;
            location.Y = location.Y - relativeTo.Y;
            return location;
        }

        /// <summary>
        /// Translates a location to screen space
        /// </summary>
        /// <param name="location">The location in map pixel coordinates</param>
        /// <param name="relativeTo">Region of the map that is on screen</param>
        public static void Translate(ref Vector2 location, ref Rectangle relativeTo)
        {
            location.X = location.X - relativeTo.X;
            location.Y = location.Y - relativeTo.Y;
        }

        /// <summary>
        /// Returns a collection of MapObjects inside the given region
        /// </summary>
        /// <param name="region">The region, in pixles, to check</param>
        /// <returns>Collection of matching MapObjects</returns>
        public IEnumerable<MapObject> GetObjectsInRegion(Rectangle region)
        {
            return GetObjectsInRegion(ref region);
        }

        /// <summary>
        /// Returns a collection of MapObjects inside the given region
        /// </summary>
        /// <param name="region">The region, in pixles, to check</param>
        /// <returns>Collection of matching MapObjects</returns>
        public IEnumerable<MapObject> GetObjectsInRegion(ref Rectangle region)
        {
            var results = new List<MapObject>();

            for (var i = 0; i < ObjectLayers.Count; i++)
                results.AddRange(GetObjectsInRegion(i, ref region));

            return results;
        }

        /// <summary>
        /// Returns a collection of MapObjects inside the given region
        /// </summary>
        /// <param name="objectLayerId">The object layer to check</param>
        /// <param name="region">The region, in pixles, to check</param>
        /// <returns>Collection of matching MapObjects</returns>
        public IEnumerable<MapObject> GetObjectsInRegion(int objectLayerId, Rectangle region)
        {
            return GetObjectsInRegion(objectLayerId, ref region);
        }

        /// <summary>
        /// Returns a collection of MapObjects inside the given region
        /// </summary>
        /// <param name="objectLayerId">The object layer to check</param>
        /// <param name="region">The region, in pixles, to check</param>
        /// <returns>Collection of matching MapObjects</returns>
        public IEnumerable<MapObject> GetObjectsInRegion(int objectLayerId, ref Rectangle region)
        {
            var results = new List<MapObject>();

            foreach (var mapObject in ObjectLayers[objectLayerId].MapObjects)
                if (region.Contains(mapObject.Bounds) || region.Intersects(mapObject.Bounds)) results.Add(mapObject);

            return results;
        }

        /// <summary>
        /// Returns a collection of TileData inside the given region
        /// </summary>
        /// <param name="region">The region, in pixles, to check</param>
        /// <returns>Collection of matching TileData</returns>
        public IEnumerable<TileData> GetTilesInRegion(Rectangle region)
        {
            return GetTilesInRegion(ref region);
        }

        /// <summary>
        /// Returns a collection of TileData inside the given region
        /// </summary>
        /// <param name="region">The region, in pixles, to check</param>
        /// <returns>Collection of matching TileData</returns>
        public IEnumerable<TileData> GetTilesInRegion(ref Rectangle region)
        {
            var result = new List<TileData>();

            var txMin = region.X / TileWidth;
            var txMax = (region.X + region.Width) / TileWidth;
            var tyMin = region.Y / TileHeight;
            var tyMax = (region.Y + region.Height) / TileHeight;

            if (Orientation == MapOrientation.Isometric)
            {
                tyMax = tyMax * 2 + 1;
                txMax = txMax * 2 + 1;
            }

            foreach (var tileLayer in TileLayers)
                for (var y = tyMin; y <= tyMax; y++)
                    for (var x = txMin; x <= txMax; x++)
                        if (tileLayer.Tiles[x][y] != null)
                            result.Add(tileLayer.Tiles[x][y]);

            return result;
        }

        /// <summary>
        /// Returns a collection of TileData inside the given region
        /// </summary>
        /// <param name="tileLayerId">The tile layer to check</param>
        /// <param name="region">The region, in pixles, to check</param>
        /// <returns>Collection of matching TileData</returns>
        public IEnumerable<TileData> GetTilesInRegion(int tileLayerId, Rectangle region)
        {
            return GetTilesInRegion(tileLayerId, ref region);
        }

        /// <summary>
        /// Returns a collection of TileData inside the given region
        /// </summary>
        /// <param name="tileLayerId">The tile layer to check</param>
        /// <param name="region">The region, in pixles, to check</param>
        /// <returns>Collection of matching TileData</returns>
        public IEnumerable<TileData> GetTilesInRegion(int tileLayerId, ref Rectangle region)
        {
            var result = new List<TileData>();

            var txMin = region.X / TileWidth;
            var txMax = (region.X + region.Width) / TileWidth;
            var tyMin = region.Y / TileHeight;
            var tyMax = (region.Y + region.Height) / TileHeight;

            if (Orientation == MapOrientation.Isometric)
            {
                tyMax = tyMax * 2 + 1;
                txMax = txMax * 2 + 1;
            }

            for (var y = tyMin; y <= tyMax; y++)
                for (var x = txMin; x <= txMax; x++)
                    if (TileLayers[tileLayerId].Tiles[x][y] != null)
                        result.Add(TileLayers[tileLayerId].Tiles[x][y]);

            return result;
        }
    }
}