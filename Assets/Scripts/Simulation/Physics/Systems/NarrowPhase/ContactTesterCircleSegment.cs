using System.Numerics;
using Leopotam.EcsLite.Di;
using Simulation.Physics.Components.Physics;
using Simulation.Physics.Components.Physics.Events;
using Simulation.Physics.Components.Physics.Shapes;
using Simulation.Physics.Services;

namespace Simulation.Physics.Systems.NarrowPhase
{
    public class ContactTesterCircleSegment : ContactTester<Circle, Segment>
    {
        private EcsPoolInject<Circle> circles;
        private EcsPoolInject<Pose> poses;
        private EcsPoolInject<Segment> segments;

        protected override void TestPair(in PossibleContact<Circle, Segment> possibleContact)
        {
            var circleEntity = possibleContact.BodyA;
            var segmentEntity = possibleContact.BodyB;
            ref var circlePose = ref poses.Value.Get(circleEntity);

            float penetration = default;
            Vector2 normal = default;
            Vector2 point = default;

            if (TestersCommon.CircleSegment(circles.Value.Get(circleEntity).Radius, circlePose.Position,
                    segments.Value.Get(segmentEntity), ref penetration, ref normal, ref point
                )) AddSolverTask(possibleContact, penetration, normal, point);
        }
    }
}