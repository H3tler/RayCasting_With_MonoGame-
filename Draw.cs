using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Monogame1
{
    public class Draw
    {
        private SpriteBatch spritebatch;
        public SpriteBatch SpriteBatch {
            get { 
                return this.spritebatch; 
            } 
            set { } 
        }

        public Draw(SpriteBatch spriteBatch)
        {
            this.spritebatch = spriteBatch;
        }

        public void FillCircle(float radius, Vector2 Mid, Color color, Texture2D Texture)
        {
            for (int i = 0; i <= 360; i++)
            {
                float rad = Geometry.AngleToRadian(i);
                var point = Geometry.GetComps(rad, radius);
                DrawLine(Texture, Mid, new Vector2(point.X + Mid.X, point.Y + Mid.Y), color, 1f);
            }
        }

        public void DrawCircle(Vector2 Mid, float Radius, Color color, Texture2D Texture)
        {
            for (float i = 0; i <= 360f; i += 0.5f)
            {
                float rad1 = Geometry.AngleToRadian(i);
                float rad2 = Geometry.AngleToRadian(i + 0.5f);

                float x1 = (float)Math.Cos(rad1) * Radius;
                float y1 = (float)Math.Sin(rad1) * Radius;

                float x2 = (float)Math.Cos(rad2) * Radius;
                float y2 = (float)Math.Sin(rad2) * Radius;

                Vector2 point1 = new Vector2(x1 + Mid.X, y1 + Mid.Y);
                Vector2 point2 = new Vector2(x2 + Mid.X, y2 + Mid.Y);

                DrawLine(Texture, point1, point2, color, 1f);

            }
        }

        public void DrawLine(Texture2D texture, Vector2 Vec1, Vector2 Vec2, Color color, float scale)
        {
            spritebatch.Draw(texture, Vec1, null, color,
                         (float)Math.Atan2(Vec2.Y - Vec1.Y, Vec2.X - Vec1.X),
                         new Vector2(0f, (float)texture.Height / 2),
                         new Vector2(Vector2.Distance(Vec1, Vec2), scale),
                         SpriteEffects.None, 0f);
        }
    }
}
