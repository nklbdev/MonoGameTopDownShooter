using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameProxies;

namespace XTiled
{
    /// <summary>
    /// A sequence of lines
    /// </summary>
    public class Polyline
    {
        /// <summary>
        /// The points that make up the polyline, in order
        /// </summary>
        public Point[] Points;
        /// <summary>
        /// The lines that make up the polyline, in order
        /// </summary>
        public Line[] Lines;
        /// <summary>
        /// Bounding rectangle of this polyline
        /// </summary>
        public Rectangle Bounds;

        /// <summary>
        /// Creates a Polyline from a list of points and calculates the lines and bounds of the result
        /// </summary>
        /// <param name="points">The list of points that define the polyline</param>
        /// <returns>a Polyline object</returns>
        public static Polyline FromPoints(IEnumerable<Point> points)
        {
            if (points == null)
                throw new ArgumentNullException("points");
            var pointArray = points.ToArray();
            var poly = new Polyline { Points = pointArray };

            if (poly.Points.Length > 1)
            {
                poly.Lines = new Line[poly.Points.Length - 1];
                for (var i = 0; i < poly.Lines.Length; i++)
                {
                    poly.Lines[i] = Line.FromPoints(
                        new Vector2(poly.Points[i].X, poly.Points[i].Y),
                        new Vector2(poly.Points[i + 1].X, poly.Points[i + 1].Y));
                }
            }

            poly.Bounds.X = pointArray.Min(x => x.X);
            poly.Bounds.Y = pointArray.Min(x => x.Y);
            poly.Bounds.Width = pointArray.Max(x => x.X) - pointArray.Min(x => x.X);
            poly.Bounds.Height = pointArray.Max(x => x.Y) - pointArray.Min(x => x.Y);

            return poly;
        }

        /// <summary>
        /// Draws the lines that make up the Polyline
        /// </summary>
        /// <param name="spriteBatch">XNA SpriteBatch instance; SpriteBatch.Begin() must be called before using this method</param>
        /// <param name="region">Region of the map in pixels to draw</param> 
        /// <param name="texture">A texture to use in drawing the lines</param>
        /// <param name="lineWidth">The width of the lines in pixels</param>
        /// <param name="color">The color value to apply to the given texture</param>
        /// <param name="layerDepth">LayerDepth value to pass to SpriteBatch</param>
        public void Draw(IImmutableSpriteBatch spriteBatch, Rectangle region, Texture2D texture, float lineWidth, Color color, float layerDepth)
        {
            foreach (var line in Lines)
                Line.Draw(spriteBatch, line, region, texture, lineWidth, color, layerDepth);
        }

        /// <summary>
        /// Determines if a specificed Rectangle intersects with this Polyline
        /// </summary>
        /// <param name="rect">Rectangle to compare to</param>
        /// <returns>True if the Rectangle intersects</returns>
        public bool Intersects(ref Rectangle rect)
        {
            for (var i = 0; i < Lines.Length; i++)
                if (Lines[i].Intersects(ref rect))
                    return true;

            return false;
        }

        /// <summary>
        /// Determines if a specificed Rectangle intersects with this Polyline
        /// </summary>
        /// <param name="rect">Rectangle to compare to</param>
        /// <returns>True if the Rectangle intersects</returns>
        public bool Intersects(Rectangle rect)
        {
            return Intersects(ref rect);
        }

        /// <summary>
        /// Determines if a specificed Line intersects with this Polyline
        /// </summary>
        /// <param name="line">Line to compare to</param>
        /// <returns>True if the Line intersects</returns>
        public bool Intersects(ref Line line)
        {
            for (var i = 0; i < Lines.Length; i++)
                if (Lines[i].Intersects(ref line))
                    return true;

            return false;
        }

        /// <summary>
        /// Determines if a specificed Line intersects with this Polyline
        /// </summary>
        /// <param name="line">Line to compare to</param>
        /// <returns>True if the Line intersects</returns>
        public bool Intersects(Line line)
        {
            return Intersects(ref line);
        }
    }
}