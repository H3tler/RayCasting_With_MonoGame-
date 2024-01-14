using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Monogame1
{
    public static class Geometry
    {
              
        public static float GetRotation(Vector2 vec, Vector2 vec2)
        {
            double theta = Math.Atan2(vec.Y - vec2.Y, vec.X - vec2.X);

            return (float)theta;
        }

        public static float AngleToRadian(float angle)
        {
            return angle * (float)(Math.PI / 180);
        }

        public static Vector2 VectorRotation(float angle, float radius)
        {
            float x = (float)Math.Cos(angle) * radius;
            float y = (float)Math.Sin(angle) * radius;
            return new Vector2(x, y);
        }

        public static Vector2 LineIntersect(Vector2 p, float angle, Vector2 wall1, Vector2 wall2)
        {
            Vector2 dir = VectorRotation(angle, 1);

            float x1 = wall1.X;
            float x2 = wall2.X;
            float x3 = p.X;
            float x4 = p.X + dir.X;

            float y1 = wall1.Y;
            float y2 = wall2.Y;
            float y3 = p.Y;
            float y4 = p.Y + dir.Y;

            double den = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);

            double t = ((x1 - x3) * (y3 - y4) - (y1 - y3) * (x3 - x4)) / den;

            double u = -((x1 - x2) * (y1 - y3) - (y1 - y2) * (x1 - x3)) / den;

            if (t > 0 && t < 1 && u > 0)
            {
                double newx = x1 + (t * (x2 - x1));
                double newy = y1 + (t * (y2 - y1));

                return new Vector2((float)newx, (float)newy);
            }
            else
            {
                return p;
            }

        }
    }
}
