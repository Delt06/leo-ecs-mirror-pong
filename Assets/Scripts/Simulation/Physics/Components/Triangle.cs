using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Simulation.Physics.Components
{
    public struct Triangle
    {
        public Vector2 A, B, C;
        public Vector2 CircumcircleCenter;
        public float CircumcircleRadius;

        /// <summary>
        ///     Centroid is a center of mass of a triangle (if mass is distributed evenly)
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 CalculateCentroid(in Triangle t) => (t.A + t.B + t.C) / 3f;

        public static Vector2 CalculateCircumcircleCenter(Vector2 A, Vector2 B, Vector2 C)
        {
            var area = CalculateArea(A, B, C);
            var a2 = (B - C).LengthSquared();
            var b2 = (C - A).LengthSquared();
            var c2 = (B - A).LengthSquared();

            var a = a2 * (b2 + c2 - a2) * A;
            var b = b2 * (c2 + a2 - b2) * B;
            var c = c2 * (a2 + b2 - c2) * C;

            return (a + b + c) / (16 * area * area);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float CalculateArea(in Triangle t) => CalculateArea(t.A, t.B, t.C);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float CalculateArea(Vector2 a, Vector2 b, Vector2 c) =>
            0.5f * Math.Abs(Vector2Ext.Cross(a - b, a - c));

        /// <summary>
        ///     Mass Moment of Inertia.
        ///     Triangle corners should be defined relative to triangle center of mass (centroid)
        /// </summary>
        public static float CalculateMMOI(in Triangle t, float mass)
        {
            var aa = Vector2.Dot(t.A, t.A);
            var bb = Vector2.Dot(t.B, t.B);
            var cc = Vector2.Dot(t.C, t.C);
            var ab = Vector2.Dot(t.A, t.B);
            var bc = Vector2.Dot(t.B, t.C);
            var ca = Vector2.Dot(t.C, t.A);
            return (aa + bb + cc + ab + bc + ca) * mass / 6f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Triangle operator +(in Triangle t, Vector2 v) => new Triangle(t.A + v, t.B + v, t.C + v);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Triangle operator -(in Triangle t, Vector2 v) => new Triangle(t.A - v, t.B - v, t.C - v);

        public Triangle(Vector2 a, Vector2 b, Vector2 c)
        {
            A = a;
            B = b;
            C = c;
            CircumcircleCenter = CalculateCircumcircleCenter(a, b, c);
            CircumcircleRadius = (a - CircumcircleCenter).Length();
        }

        public Vector2 this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return A;
                    case 1: return B;
                    case 2: return C;
                    case 3: return A;
                    default:
                        throw new IndexOutOfRangeException("You tried to access triangle corner at index: " + index);
                }
            }
        }
    }
}