using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XTiled
{
    public class Polyline
    {
        public Point[] Points;
        public Line[] Lines;
        public Rectangle Bounds;

        public static Polyline FromPoints(IEnumerable<Point> points)
        {
            var polyline = new Polyline {Points = points.ToArray()};
            if (polyline.Points.Length > 1)
            {
                polyline.Lines = new Line[polyline.Points.Length - 1];
                for (var index = 0; index < polyline.Lines.Length; ++index)
                    polyline.Lines[index] = Line.FromPoints(new Vector2(polyline.Points[index].X, polyline.Points[index].Y), new Vector2(polyline.Points[index + 1].X, polyline.Points[index + 1].Y));
            }
            polyline.Bounds.X = points.Min(x => x.X);
            polyline.Bounds.Y = points.Min(x => x.Y);
            polyline.Bounds.Width = (points.Max(x => x.X) - points.Min(x => x.X));
            polyline.Bounds.Height = (points.Max(x => x.Y) - points.Min(x => x.Y));
            return polyline;
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle region, Texture2D texture, float lineWidth, Color color, float layerDepth)
        {
            for (var index = 0; index < Lines.Length; ++index)
                Line.Draw(spriteBatch, Lines[index], region, texture, lineWidth, color, layerDepth);
        }

        public bool Intersects(ref Rectangle rect)
        {
            for (var index = 0; index < Lines.Length; ++index)
            {
                if (Lines[index].Intersects(ref rect))
                    return true;
            }
            return false;
        }

        public bool Intersects(Rectangle rect)
        {
            return Intersects(ref rect);
        }

        public bool Intersects(ref Line line)
        {
            for (var index = 0; index < Lines.Length; ++index)
            {
                if (Lines[index].Intersects(ref line))
                    return true;
            }
            return false;
        }

        public bool Intersects(Line line)
        {
            return Intersects(ref line);
        }
    }
}