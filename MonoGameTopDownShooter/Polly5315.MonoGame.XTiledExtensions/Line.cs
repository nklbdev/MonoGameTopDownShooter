using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Polly.MonoGame.XTiledExtensions
{
    public struct Line
    {
        public Vector2 Start;
        public Vector2 End;
        public float Length;
        public float Angle;

        public static Line FromPoints(Vector2 start, Vector2 end)
        {
            return new Line
            {
                Start = start,
                End = end,
                Length = Convert.ToSingle(Math.Sqrt(Math.Pow(Math.Abs(start.X - end.X), 2.0) + Math.Pow(Math.Abs(start.Y - end.Y), 2.0))),
                Angle = Convert.ToSingle(Math.Atan2(end.Y - start.Y, end.X - start.X))
            };
        }

        public static void Draw(SpriteBatch spriteBatch, Line line, Rectangle region, Texture2D texture, float lineWidth, Color color, float layerDepth)
        {
            var vector2 = Map.Translate(line.Start, region);
            spriteBatch.Draw(texture, vector2, new Rectangle?(), color, line.Angle, Vector2.Zero, new Vector2(line.Length, lineWidth), 0, layerDepth);
        }

        public bool Intersects(Line line)
        {
            return Intersects(ref line);
        }

        public bool Intersects(ref Line line)
        {
            bool result;
            Vector2 intersection;
            Intersects(ref line, out result, out intersection);
            return result;
        }

        public bool Intersects(Line line, out Vector2 intersection)
        {
            return Intersects(ref line, out intersection);
        }

        public bool Intersects(ref Line line, out Vector2 intersection)
        {
            bool result;
            Intersects(ref line, out result, out intersection);
            return result;
        }

        public bool Intersects(Rectangle rect)
        {
            return Intersects(ref rect);
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
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            return Intersects(FromPoints(new Vector2(@rect.Left, @rect.Top), new Vector2(@rect.Right, @rect.Top))) || Intersects(FromPoints(new Vector2(@rect.Right, @rect.Top), new Vector2(@rect.Right, @rect.Bottom))) || (Intersects(FromPoints(new Vector2(@rect.Right, @rect.Bottom), new Vector2(@rect.Left, @rect.Bottom))) || Intersects(FromPoints(new Vector2(@rect.Left, @rect.Bottom), new Vector2(@rect.Left, @rect.Top))));
        }

        public void Intersects(ref Line line, out bool result, out Vector2 intersection)
        {
            result = false;
            intersection = Vector2.Zero;
            var num1 = (line.End.Y - line.Start.Y) * (End.X - Start.X) - (line.End.X - line.Start.X) * (End.Y - Start.Y);
            if (num1 == 0.0)
                return;
            var num2 = (line.End.X - line.Start.X) * (Start.Y - line.Start.Y) - (line.End.Y - line.Start.Y) * (Start.X - line.Start.X);
            var num3 = (End.X - Start.X) * (Start.Y - line.Start.Y) - (End.Y - Start.Y) * (Start.X - line.Start.X);
            var num4 = num2 / num1;
            var num5 = num3 / num1;
            if (num4 < 0.0 || num4 > 1.0 || (num5 < 0.0 || num5 > 1.0))
                return;
            intersection.X = (float) (Start.X + (double)num4 * (End.X - Start.X));
            intersection.Y = (float) (Start.Y + (double)num4 * (End.Y - Start.Y));
            result = true;
        }
    }
}