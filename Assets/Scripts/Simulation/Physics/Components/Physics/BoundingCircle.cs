using System.Numerics;
using System.Runtime.CompilerServices;

namespace Simulation.Physics.Components.Physics
{
    public struct BoundingCircle
    {
        public Vector2 Center;
        public float Radius;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(in BoundingCircle a, in BoundingCircle b)
        {
            var squaredDistance = (b.Center - a.Center).LengthSquared();
            var sumRadii = a.Radius + b.Radius;
            return squaredDistance <= sumRadii * sumRadii;
        }
    }
}