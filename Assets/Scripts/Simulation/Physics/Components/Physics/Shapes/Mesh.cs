using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Simulation.Physics.Components.Physics.Shapes
{
    public struct Mesh : IShape
    {
        public Triangle[] Triangles;
        public Vector2 MassCenter;
        public float TotalArea;

        public Mesh(Triangle[] triangles, Vector2 massCenter, float totalArea)
        {
            Triangles = triangles;
            MassCenter = massCenter;
            TotalArea = totalArea;
        }

        public static Mesh CreateMesh(Triangle[] triangles)
        {
            // mass center of a group of triangles is constant, regardless of specific mass value,
            // but we need to know contribution of each triangle to compute mass center, so let's use some guess mass.
            // Mass of right triangular prism = area * thickness * density, let's take (thickness * density) = 1,
            // so mass equals triangles' area.

            float guessMass = default;
            Vector2 combinedCentroid = default;
            for (var i = 0; i < triangles.Length; ++i)
            {
                var centroid = Triangle.CalculateCentroid(triangles[i]);
                var mass = Triangle.CalculateArea(triangles[i]);
                combinedCentroid += mass * centroid;
                guessMass += mass;
            }

            // mesh points should be defined relative to center of mass
            var massCenter = combinedCentroid / guessMass;
            for (var i = 0; i < triangles.Length; ++i)
            {
                triangles[i] -= massCenter;
            }

            var mesh = new Mesh(triangles, massCenter, guessMass);
            return mesh;
        }

        /// <summary>
        ///     Mass Moment of Inertia
        /// </summary>
        public static float CalculateMMOI(Mesh mesh, float targetMass)
        {
            // mass of right triangular prism = area * thickness * density.
            // We can calculate first guess mass and then scale it towards target mass.
            // After we scaled total mass, found modifier, we need to correctly scale each triangle's mass either.
            // We take (thickness * density) = 1, so guess mass = area.

            float combinedMMOI = default;
            var massModifier = targetMass / mesh.TotalArea;
            var triangles = mesh.Triangles;
            for (var i = 0; i < triangles.Length; ++i)
            {
                var centroid = Triangle.CalculateCentroid(triangles[i]);
                var mass = Triangle.CalculateArea(triangles[i]) * massModifier;
                var mmoi = Triangle.CalculateMMOI(triangles[i] - centroid, mass);
                combinedMMOI += mmoi + mass * centroid.LengthSquared();
            }

            return combinedMMOI;
        }

        /// <summary>
        ///     we assume center of bounding circle is at origin
        /// </summary>
        public static float CalculateBoundingCircleRadius(Triangle[] triangles)
        {
            var maxDistanceSquared = float.NegativeInfinity;
            for (var i = 0; i < triangles.Length; ++i)
            {
                for (var k = 0; k < 3; ++k)
                {
                    var distanceSquared = triangles[i][k].LengthSquared();
                    if (distanceSquared > maxDistanceSquared) maxDistanceSquared = distanceSquared;
                }
            }

            return (float) Math.Sqrt(maxDistanceSquared);
        }

        /// <summary>
        ///     not optimal, but optimality depends on your applications
        /// </summary>
        public static BoundingBox CalculateBoundingBox(Triangle[] triangles, in Pose pose)
        {
            var minX = float.PositiveInfinity;
            var maxX = float.NegativeInfinity;
            var minY = float.PositiveInfinity;
            var maxY = float.NegativeInfinity;

            var translation = pose.Position;
            var rotation = pose.RotationVector;
            for (var i = 0; i < triangles.Length; ++i)
            {
                for (var k = 0; k < 3; ++k)
                {
                    var p = Vector2Ext.Rotate(triangles[i][k], rotation);
                    if (p.X < minX) minX = p.X;
                    if (p.X > maxX) maxX = p.X;
                    if (p.Y < minY) minY = p.Y;
                    if (p.Y > maxY) maxY = p.Y;
                }
            }

            return new BoundingBox(
                translation + new Vector2(minX, minY),
                translation + new Vector2(maxX, maxY)
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly int TypeId() => Id;

        public const int Id = 1;
    }
}