using System.Runtime.CompilerServices;

namespace Simulation.Physics.Components.Physics.Shapes
{
    public struct Circle : IShape
    {
        public float Radius;

        /// <summary>
        ///     Mass Moment of Inertia
        /// </summary>
        public static float CalculateMMOI(float radius, float mass) => 0.5f * mass * radius * radius;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly int TypeId() => Id;

        public const int Id = 0;
    }
}