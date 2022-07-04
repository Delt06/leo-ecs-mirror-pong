using System;
using System.Collections.Generic;
using System.Numerics;
using Leopotam.EcsLite.Di;
using Simulation.Physics.Components;
using Simulation.Physics.Components.Physics;
using Simulation.Physics.Components.Physics.Events;
using Simulation.Physics.Components.Physics.Shapes;
using Simulation.Physics.Services;

namespace Simulation.Physics.Systems.NarrowPhase
{
    public class ContactTesterMeshSegment : ContactTester<Mesh, Segment>
    {
        private EcsPoolInject<Mesh> meshes;
        private EcsPoolInject<Pose> poses;
        private EcsPoolInject<Segment> segments;

        protected override void TestPair(in PossibleContact<Mesh, Segment> possibleContact)
        {
            var meshEntity = possibleContact.BodyA;
            var segmentEntity = possibleContact.BodyB;
            var mesh = meshes.Value.Get(meshEntity);
            ref var meshPose = ref poses.Value.Get(meshEntity);
            ref var segment = ref segments.Value.Get(segmentEntity);

            MeshSegment(mesh, meshPose, segment, possibleContact);
        }

        private void MeshSegment(Mesh mesh, in Pose meshPose, in Segment segment,
            in PossibleContact<Mesh, Segment> possibleContact)
        {
            const float parallelityEpsilon = 0.1f;
            const float closeEpsilon = 0.1f;

            var a = Pose.TransformInverse(segment.A, meshPose);
            var b = Pose.TransformInverse(segment.B, meshPose);

            var triangles = mesh.Triangles;
            var segmentIndices = new List<int>(3);
            for (var i = 0; i < triangles.Length; ++i)
            {
                ref var triangle = ref triangles[i];

                // check for edge-edge contacts
                var edgeEdgeContact = false;
                for (var k = 0; k < 3; ++k)
                {
                    if (GetParallelity(b - a, triangle[k + 1] - triangle[k]) < parallelityEpsilon)
                    {
                        var vertex = triangle[(k + 2) % 3];
                        var triangleHeight = TestersCommon.PointSegmentDistance(vertex, triangle[k], triangle[k + 1]);
                        var toSegment = TestersCommon.PointSegmentDistance(vertex, a, b, out var normal);
                        var penetration = triangleHeight - toSegment;

                        if (penetration > 0 && penetration < closeEpsilon)
                        {
                            // we need a better contact point
                            var contactPoint = (triangle[k] + triangle[k + 1]) * 0.5f;
                            var normalWorldSpace = Pose.TransformDirection(normal, meshPose);
                            var pointWorldSpace = Pose.Transform(contactPoint, meshPose);

                            AddSolverTask(possibleContact, penetration, normalWorldSpace, pointWorldSpace);
                            edgeEdgeContact = true;
                            break;
                        }
                    }
                }

                // check for vertex-edge contacts
                if (!edgeEdgeContact)
                {
                    for (var k = 0; k < 3; ++k)
                    {
                        if (TestersCommon.SegmentSegment(a, b, triangle[k], triangle[k + 1])) segmentIndices.Add(k);
                    }

                    if (segmentIndices.Count == 1)
                    {
                        // vertex-edge contact, segment as a vertex, edge is from triangle
                        var i_0 = segmentIndices[0];
                        var inTriangle = TestersCommon.PointInTriangle(a, triangle.A, triangle.B, triangle.C) ? a : b;
                        var penetration = TestersCommon.PointSegmentDistance(inTriangle, triangle[i_0],
                            triangle[i_0 + 1], out var normal
                        );
                        var normalWorldSpace = Pose.TransformDirection(normal, meshPose);
                        var pointWorldSpace = Pose.Transform(inTriangle, meshPose);

                        AddSolverTask(possibleContact, penetration, normalWorldSpace, pointWorldSpace);
                    }
                    else if (segmentIndices.Count == 2)
                    {
                        // vertex-edge contact, vertex is from triangle, segment as an edge
                        var i_0 = segmentIndices[0];
                        var i_1 = segmentIndices[1];
                        var i_vertex = i_0 + 1 == i_1 ? i_1 : i_0;

                        var vertex = triangle[i_vertex];
                        var penetration = TestersCommon.PointSegmentDistance(vertex, a, b, out var invNormal);
                        var normalWorldSpace = Pose.TransformDirection(-invNormal, meshPose);
                        var pointWorldSpace = Pose.Transform(vertex, meshPose);

                        AddSolverTask(possibleContact, penetration, normalWorldSpace, pointWorldSpace);
                    }

                    segmentIndices.Clear();
                }
            }
        }

        private static float GetParallelity(Vector2 pq, Vector2 ab) => Math.Abs(Vector2Ext.Cross(pq, ab));
    }
}