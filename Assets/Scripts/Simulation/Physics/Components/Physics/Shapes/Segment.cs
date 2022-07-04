using System.Numerics;
using System.Runtime.CompilerServices;

namespace Simulation.Physics.Components.Physics.Shapes
{
    public struct Segment : IShape
    {
        public Vector2 A, B;

        public Segment(Vector2 a, Vector2 b)
        {
            A = a;
            B = b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 GetCenter(in Segment segment) => (segment.A + segment.B) * 0.5f;

        /// <summary>
        ///     Mass Moment of Inertia
        /// </summary>
        public static float CalculateMMOI(float lengthSquared, float mass) => mass * lengthSquared / 12f;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly int TypeId() => Id;

        public const int Id = 2;
    }
}