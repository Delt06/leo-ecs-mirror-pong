using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using Simulation.Physics.Components;
using Simulation.Physics.Components.Physics.Shapes;

namespace Simulation.Physics.Services
{
    public static class TestersCommon
    {
        public static bool CircleCircle(float radiusA, Vector2 centerA, float radiusB, Vector2 centerB,
            ref float penetration, ref Vector2 normal, ref Vector2 point)
        {
            var direction = centerA - centerB;
            var directionLengthSquared = direction.LengthSquared();
            var sumRadii = radiusA + radiusB;
            if (directionLengthSquared > sumRadii * sumRadii) return false;

            var directionLength = (float) Math.Sqrt(directionLengthSquared);
            if (directionLength != 0)
            {
                penetration = sumRadii - directionLength;
                normal = direction / directionLength;
                point = centerA - direction * 0.5f;
                return true;
            }

            // Circles are on same position
            // Choose random (but consistent) values
            penetration = radiusA;
            normal = Vector2.UnitX;
            point = centerA;
            return true;
        }

        public static bool CircleSegment(float circleRadius, Vector2 circleCenter, in Segment segment,
            ref float penetration, ref Vector2 normal, ref Vector2 point)
        {
            var closestPoint = ClosestPointSegment(circleCenter, segment.A, segment.B);
            var direction = circleCenter - closestPoint;
            var directionLengthSquared = direction.LengthSquared();
            if (directionLengthSquared > circleRadius * circleRadius) return false;

            var directionLength = (float) Math.Sqrt(directionLengthSquared);
            penetration = circleRadius - directionLength;
            normal = direction / directionLength;
            point = circleCenter - normal * (circleRadius - penetration * 0.5f);
            return true;
        }

        public static Vector2 ClosestPointSegment(Vector2 point, Vector2 a, Vector2 b)
        {
            var ab = b - a;
            var t = Vector2.Dot(point - a, ab) / Vector2.Dot(ab, ab);
            t = Clamp(t, 0f, 1f);
            return a + t * ab;
        }

        public static float PointSegmentDistance(Vector2 point, Vector2 a, Vector2 b)
        {
            var closestPoint = ClosestPointSegment(point, a, b);
            return (point - closestPoint).Length();
        }

        /// <summary>
        ///     normal directs towards point
        /// </summary>
        public static float PointSegmentDistance(Vector2 point, Vector2 a, Vector2 b, out Vector2 normal)
        {
            var closestPoint = ClosestPointSegment(point, a, b);
            var direction = point - closestPoint;
            var distance = direction.Length();
            normal = direction / distance;
            return distance;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Clamp(float val, float low, float high) => Math.Max(low, Math.Min(val, high));

        public static bool CircleTriangleBoundingCircles(float circleRadius, Vector2 circleCenter, in Triangle triangle)
        {
            var triangleCenter = triangle.CircumcircleCenter;
            var maxDistance = triangle.CircumcircleRadius + circleRadius;
            return (circleCenter - triangleCenter).LengthSquared() <= maxDistance * maxDistance;
        }

        public static bool CircleTriangle(float circleRadius, Vector2 circleCenter, in Triangle triangle,
            ref float penetration, ref Vector2 normal, ref Vector2 point)
        {
            var closestPoint = ClosestPointTriangle(circleCenter, triangle.A, triangle.B, triangle.C);
            var direction = circleCenter - closestPoint;
            var directionLengthSquared = direction.LengthSquared();
            if (directionLengthSquared > circleRadius * circleRadius) return false;

            var directionLength = (float) Math.Sqrt(directionLengthSquared);
            penetration = circleRadius - directionLength;
            normal = direction / directionLength;
            point = circleCenter - normal * (circleRadius - penetration * 0.5f);
            return true;
        }

        public static Vector2 ClosestPointTriangle(Vector2 point, Vector2 a, Vector2 b, Vector2 c)
        {
            float v, w;

            var ab = b - a;
            var ac = c - a;
            var ap = point - a;
            var d1 = Vector2.Dot(ab, ap);
            var d2 = Vector2.Dot(ac, ap);
            if (d1 <= 0f && d2 <= 0f) return a;

            var bp = point - b;
            var d3 = Vector2.Dot(ab, bp);
            var d4 = Vector2.Dot(ac, bp);
            if (d3 >= 0f && d4 <= d3) return b;

            var vc = d1 * d4 - d3 * d2;
            if (vc <= 0f && d1 >= 0f && d3 <= 0f)
            {
                v = d1 / (d1 - d3);
                return a + v * ab;
            }

            var cp = point - c;
            var d5 = Vector2.Dot(ab, cp);
            var d6 = Vector2.Dot(ac, cp);
            if (d6 >= 0f && d5 <= d6) return c;

            var vb = d5 * d2 - d1 * d6;
            if (vb <= 0f && d2 >= 0f && d6 <= 0f)
            {
                w = d2 / (d2 - d6);
                return a + w * ac;
            }

            var va = d3 * d6 - d5 * d4;
            if (va <= 0f && d4 - d3 >= 0f && d5 - d6 >= 0f)
            {
                w = (d4 - d3) / (d4 - d3 + (d5 - d6));
                return b + w * (c - b);
            }

            var denom = 1f / (va + vb + vc);
            v = vb * denom;
            w = vc * denom;
            return a + ab * v + ac * w;
        }

        public static bool PointInTriangle(Vector2 p, Vector2 a, Vector2 b, Vector2 c)
        {
            var pab = Vector2Ext.CrossPseudo(p - a, b - a);
            var pbc = Vector2Ext.CrossPseudo(p - b, c - b);
            if (pab * pbc < 0) return false;
            var pca = Vector2Ext.CrossPseudo(p - c, a - c);
            return pab * pca >= 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float SignedTriangleArea(Vector2 a, Vector2 b, Vector2 c) =>
            (a.X - c.X) * (b.Y - c.Y) - (a.Y - c.Y) * (b.X - c.X);

        public static bool SegmentSegment(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
        {
            var a1 = SignedTriangleArea(a, b, d);
            var a2 = SignedTriangleArea(a, b, c);
            if (a1 * a2 < 0f)
            {
                var a3 = SignedTriangleArea(c, d, a);
                var a4 = a3 + a2 - a1;
                if (a3 * a4 < 0f) return true;
            }

            return false;
        }

        public static bool SegmentSegment(Vector2 a, Vector2 b, Vector2 c, Vector2 d, ref Vector2 p)
        {
            var a1 = SignedTriangleArea(a, b, d);
            var a2 = SignedTriangleArea(a, b, c);
            if (a1 * a2 < 0f)
            {
                var a3 = SignedTriangleArea(c, d, a);
                var a4 = a3 + a2 - a1;
                if (a3 * a4 < 0f)
                {
                    var t = a3 / (a3 - a4);
                    p = a + t * (b - a);
                    return true;
                }
            }

            return false;
        }
    }
}