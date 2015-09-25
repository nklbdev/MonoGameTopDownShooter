using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameTopDownShooter
{
    public static class SpriteBatchExtensions
    {
        public static void DrawRectangle(this SpriteBatch sb, Vector2 position, Vector2 size, Color color, Texture2D texture)
        {
            sb.Draw(texture,
                new Rectangle(// rectangle defines shape of line and position of start of line
                    (int)position.X,
                    (int)position.Y,
                    (int)size.X,
                    (int)size.Y),
                null,
                color, //colour of line
                0,     //angle of line (calulated above)
                new Vector2(0, 0), // point in line about which to rotate
                SpriteEffects.None,
                0);

        }

        public static void DrawRay(this SpriteBatch sb, Vector2 start, float rotation, float length, Color color, Texture2D texture)
        {
            sb.Draw(texture,
                new Rectangle(// rectangle defines shape of line and position of start of line
                    (int)start.X,
                    (int)start.Y,
                    (int) length, //sb will strech the texture to fill this rectangle
                    1), //width of line, change this to make thicker line
                null,
                color, //colour of line
                rotation,     //angle of line (calulated above)
                new Vector2(0, 0), // point in line about which to rotate
                SpriteEffects.None,
                0);

        }

        public static void DrawLine(this SpriteBatch sb, Vector2 start, Vector2 end, Color color, Texture2D texture)
        {
            var edge = end - start;
            // calculate angle to rotate line
            var angle = (float)Math.Atan2(edge.Y, edge.X);


            sb.Draw(texture,
                new Rectangle(// rectangle defines shape of line and position of start of line
                    (int)start.X,
                    (int)start.Y,
                    (int)edge.Length(), //sb will strech the texture to fill this rectangle
                    1), //width of line, change this to make thicker line
                null,
                color, //colour of line
                angle,     //angle of line (calulated above)
                new Vector2(0, 0), // point in line about which to rotate
                SpriteEffects.None,
                0);

        }
    }
}
