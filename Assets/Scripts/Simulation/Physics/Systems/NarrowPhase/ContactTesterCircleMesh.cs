using System.Numerics;
using Leopotam.EcsLite.Di;
using Simulation.Physics.Components.Physics;
using Simulation.Physics.Components.Physics.Events;
using Simulation.Physics.Components.Physics.Shapes;
using Simulation.Physics.Services;

namespace Simulation.Physics.Systems.NarrowPhase
{
    public class ContactTesterCircleMesh : ContactTester<Circle, Mesh>
    {
        private EcsPoolInject<Circle> circles;
        private EcsPoolInject<Mesh> meshes;
        private EcsPoolInject<Pose> poses;

        protected override void TestPair(in PossibleContact<Circle, Mesh> possibleContact)
        {
            var circleEntity = possibleContact.BodyA;
            var meshEntity = possibleContact.BodyB;
            ref var circlePose = ref poses.Value.Get(circleEntity);
            ref var meshPose = ref poses.Value.Get(meshEntity);

            CircleMesh(circles.Value.Get(circleEntity), circlePose.Position, meshes.Value.Get(meshEntity), meshPose,
                possibleContact
            );
        }

        private void CircleMesh(Circle circle, Vector2 circleCenter, Mesh mesh, in Pose meshPose,
            in PossibleContact<Circle, Mesh> possibleContact)
        {
            var triangles = mesh.Triangles;
            var circleRadius = circle.Radius;
            var circleCenterMeshSpace = Pose.TransformInverse(circleCenter, meshPose);

            for (var i = 0; i < triangles.Length; ++i)
            {
                float penetration = default;
                Vector2 normal = default;
                Vector2 point = default;

                if (TestersCommon.CircleTriangleBoundingCircles(circleRadius, circleCenterMeshSpace, triangles[i])
                    && TestersCommon.CircleTriangle(circleRadius, circleCenterMeshSpace, triangles[i], ref penetration,
                        ref normal, ref point
                    ))
                {
                    var normalWorldSpace = Pose.TransformDirection(normal, meshPose);
                    var pointWorldSpace = Pose.Transform(point, meshPose);
                    AddSolverTask(possibleContact, penetration, normalWorldSpace, pointWorldSpace);
                }
            }
        }
    }
}