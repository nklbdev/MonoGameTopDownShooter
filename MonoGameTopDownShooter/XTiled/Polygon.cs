using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XTiled
{
    public class Polygon
    {
        public Texture2D Texture;
        public Point[] Points;
        public Line[] Lines;
        public Rectangle Bounds;

        public static Polygon FromPoints(IEnumerable<Point> points)
        {
            var polygon = new Polygon
            {
                Points = points.ToArray(),
                Bounds =
                {
                    X = points.Min(x => x.X),
                    Y = points.Min(x => x.Y),
                    Width = (points.Max(x => x.X) - points.Min(x => x.X)),
                    Height = (points.Max(x => x.Y) - points.Min(x => x.Y))
                }
            };
            polygon.Points = points.ToArray();
            if (polygon.Points.Length <= 1)
                return polygon;
            polygon.Lines = new Line[polygon.Points.Length - 1];
            for (var index = 0; index < polygon.Lines.Length; ++index)
                polygon.Lines[index] = Line.FromPoints(new Vector2(polygon.Points[index].X, polygon.Points[index].Y), new Vector2(polygon.Points[index + 1].X, polygon.Points[index + 1].Y));
            return polygon;
        }

        public void GenerateTexture(GraphicsDevice graphicsDevice, Color color)
        {
            var colorArray = new Color[Bounds.Width * Bounds.Height];
            // ISSUE: explicit reference operation
            for (var index1 = Bounds.Y; index1 < Bounds.Bottom; ++index1)
            {
                // ISSUE: explicit reference operation
                for (var index2 = Bounds.X; index2 < Bounds.Right; ++index2)
                {
                    var index3 = index2 - Bounds.X + (index1 - Bounds.Y) * Bounds.Width;
                    colorArray[index3] = !Contains(new Vector2(index2, index1)) ? Color.Transparent : color;
                }
            }
            Texture = new Texture2D(graphicsDevice, Bounds.Width, Bounds.Height, false, 0);
            Texture.SetData(colorArray);
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle region, Texture2D texture, float lineWidth, Color color, float layerDepth)
        {
            foreach (var line in Lines)
                Line.Draw(spriteBatch, line, region, texture, lineWidth, color, layerDepth);
        }

        public void DrawFilled(SpriteBatch spriteBatch, Rectangle region, Texture2D texture, float lineWidth, Color color, Color fillColor, float layerDepth)
        {
            foreach (var lines in Lines)
                Line.Draw(spriteBatch, lines, region, texture, lineWidth, color, layerDepth);
            spriteBatch.Draw(Texture, Map.Translate(Bounds, region), new Rectangle?(), fillColor, 0.0f, Vector2.Zero, 0, layerDepth);
        }

        public bool Contains(ref Vector2 vector)
        {
            var flag = false;
            foreach (var line in Lines)
            {
                if (!(vector.Y > (double) Math.Min(line.Start.Y, line.End.Y)) ||
                    !(vector.Y <= (double) Math.Max(line.Start.Y, line.End.Y)) ||
                    (!(vector.X <= (double) Math.Max(line.Start.X, line.End.X)) || line.Start.Y == line.End.Y))
                    continue;
                var num = (vector.Y - line.Start.Y) * (line.End.X - line.Start.X) / (line.End.Y - line.Start.Y) + line.Start.X;
                if (line.Start.X == line.End.X || vector.X <= (double)num)
                    flag = !flag;
            }
            return flag;
        }

        public bool Contains(Vector2 vector)
        {
            return Contains(ref vector);
        }

        public bool Contains(ref Point point)
        {
            var vector = new Vector2(point.X, point.Y);
            return Contains(ref vector);
        }

        public bool Contains(Point point)
        {
            return Contains(ref point);
        }

        public bool Contains(ref Line line)
        {
            if (Contains(ref line.Start))
                return Contains(ref line.End);
            return false;
        }

        public bool Contains(Line line)
        {
            return Contains(ref line);
        }

        public bool Contains(ref Rectangle rect)
        {
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            if (Contains(new Vector2(@rect.Left, @rect.Top)) && Contains(new Vector2(@rect.Left, @rect.Bottom)) && Contains(new Vector2(@rect.Right, @rect.Top)))
            {
                // ISSUE: explicit reference operation
                // ISSUE: explicit reference operation
                return Contains(new Vector2(@rect.Right, @rect.Bottom));
            }
            return false;
        }

        public bool Contains(Rectangle rect)
        {
            return Contains(ref rect);
        }

        public bool Intersects(ref Rectangle rect)
        {
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            var num = (Contains(new Vector2(@rect.Left, @rect.Top)) ? 1 : 0) + (Contains(new Vector2(@rect.Left, @rect.Bottom)) ? 1 : 0) + (Contains(new Vector2(@rect.Right, @rect.Top)) ? 1 : 0) + (Contains(new Vector2(@rect.Right, @rect.Bottom)) ? 1 : 0);
            if (num > 0)
                return num < 4;
            return false;
        }

        public bool Intersects(Rectangle rect)
        {
            return Intersects(ref rect);
        }

        public bool Intersects(ref Line line)
        {
            return (Contains(line.Start) ? 1 : 0) + (Contains(line.End) ? 1 : 0) == 1;
        }

        public bool Intersects(Line line)
        {
            return Intersects(ref line);
        }
    }
}