using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace paintRacer
{
    public struct Line
    {
        public float m;
        public float c;

        public float x;

        public bool isConstant()
        {
            return float.IsNaN(m) || float.IsInfinity(m);
        }


        public Line(Vector2 p1, Vector2 p2)
        {
            Vector2 delta = p2 - p1;
            this.m = delta.Y / delta.X;
            this.c = p1.Y - p1.X * m;
            x = (float.IsNaN(m) || float.IsInfinity(m)) ? p1.X : float.NaN;
        }

        public float calculate(float x)
        {
            if (isConstant() && x == this.x)
                return x;
            return m * x + c;
        }

        public override string ToString()
        {
            return isConstant() ? "x = " + x : "y = " + m + " * x + " + c;
        }
    }
}
