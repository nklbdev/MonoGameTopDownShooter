using System;
using Microsoft.Xna.Framework;

namespace GameProject
{
    public static class Vector2Helpers
    {
        public static Vector2 ToVector(this float angle)
        {
            return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }

        public static float ToAngle(this Vector2 vector)
        {
            return (float)Math.Atan2(vector.Y, vector.X);
        }

        public static float SignedAngle(Vector2 v1, Vector2 v2)
        {
              var perpDot = v1.X * v2.Y - v1.Y * v2.X;
              return (float)Math.Atan2(perpDot, Vector2.Dot(v1, v2));
        }
    }
}
