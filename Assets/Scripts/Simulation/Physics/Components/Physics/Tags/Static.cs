using System.Runtime.CompilerServices;

namespace Simulation.Physics.Components.Physics.Tags
{
    public struct Static : ITag
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly int TypeId() => Id;

        public const int Id = 1;
    }
}