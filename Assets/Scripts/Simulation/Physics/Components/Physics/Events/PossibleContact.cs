using Simulation.Physics.Components.Physics.Shapes;

namespace Simulation.Physics.Components.Physics.Events
{
    public struct PossibleContact<ShapeA, ShapeB>
        where ShapeA : struct, IShape
        where ShapeB : struct, IShape
    {
        public int BodyA;
        public int BodyB;
        public int TagA;
        public int TagB;
    }
}