using System.Numerics;
using System.Runtime.CompilerServices;

namespace Simulation.Physics.Components.Physics
{
    public struct BoundingBox
    {
        public Vector2 Min;
        public Vector2 Max;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BoundingBox(Vector2 min, Vector2 max)
        {
            Min = min;
            Max = max;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BoundingBox ConstructBoundingBox(in BoundingCircle boundingCircle)
        {
            var radiusVector = new Vector2(boundingCircle.Radius);
            return new BoundingBox(boundingCircle.Center - radiusVector, boundingCircle.Center + radiusVector);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BoundingBox ConstructBoundingBox(Vector2 circleCenter, float circleRadius)
        {
            var radiusVector = new Vector2(circleRadius);
            return new BoundingBox(circleCenter - radiusVector, circleCenter + radiusVector);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(in BoundingBox a, in BoundingBox b) => Intersects(a.Min, a.Max, b.Min, b.Max);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(Vector2 minA, Vector2 maxA, in BoundingBox b) =>
            Intersects(minA, maxA, b.Min, b.Max);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(Vector2 minA, Vector2 maxA, Vector2 minB, Vector2 maxB) =>
            maxA.X >= minB.X && maxB.X >= minA.X &&
            maxA.Y >= minB.Y && maxB.Y >= minA.Y;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(Vector2 point, in BoundingBox bbox) => Intersects(point, bbox.Min, bbox.Max);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(Vector2 point, Vector2 min, Vector2 max) =>
            point.X >= min.X && point.X <= max.X &&
            point.Y >= min.Y && point.Y <= max.Y;
    }
}