using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Monogame1
{
    public class Boundary
    {

        public static List<Boundary> Boundaries = new List<Boundary>();

        public Vector2 Vec1 {  get; set; }
        public Vector2 Vec2 { get; set; }

        public Boundary(Vector2 vec1, Vector2 vec2)
        {
            this.Vec1 = vec1;
            this.Vec2 = vec2;

            Boundaries.Add(this);
        }
    }
}
