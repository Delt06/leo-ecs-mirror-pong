using System.Numerics;
using Leopotam.EcsLite.Di;
using Simulation.Physics.Components.Physics;
using Simulation.Physics.Components.Physics.Events;
using Simulation.Physics.Components.Physics.Shapes;
using Simulation.Physics.Services;

namespace Simulation.Physics.Systems.NarrowPhase
{
    public class ContactTesterCircleCircle : ContactTester<Circle, Circle>
    {
        private EcsPoolInject<Circle> circles;
        private EcsPoolInject<Pose> poses;

        protected override void TestPair(in PossibleContact<Circle, Circle> possibleContact)
        {
            var A = possibleContact.BodyA;
            var B = possibleContact.BodyB;
            ref var poseA = ref poses.Value.Get(A);
            ref var poseB = ref poses.Value.Get(B);

            float penetration = default;
            Vector2 normal = default;
            Vector2 point = default;

            if (TestersCommon.CircleCircle(circles.Value.Get(A).Radius, poseA.Position, circles.Value.Get(B).Radius,
                    poseB.Position, ref penetration, ref normal, ref point
                )) AddSolverTask(possibleContact, penetration, normal, point);
        }
    }
}