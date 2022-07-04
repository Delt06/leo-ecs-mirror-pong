using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Simulation.Physics.Components.Physics
{
    public struct Pose
    {
        public Vector2 Position;
        public Vector2 RotationVector;
        public float Rotation;

        public Pose(Vector2 position)
        {
            Position = position;
            Rotation = 0;
            RotationVector = Vector2.UnitX;
        }

        public Pose(Vector2 position, float rotation)
        {
            Position = position;
            Rotation = rotation;
            RotationVector = GetRotationVector(rotation);
        }

        public static Vector2 GetRotationVector(float angle) =>
            new Vector2((float) Math.Cos(angle), (float) Math.Sin(angle));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Transform(Vector2 point, in Pose pose) =>
            Vector2Ext.Rotate(point, pose.RotationVector) + pose.Position;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 TransformInverse(Vector2 point, in Pose pose) =>
            Vector2Ext.RotateInverse(point - pose.Position, pose.RotationVector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 TransformDirection(Vector2 direction, in Pose pose) =>
            Vector2Ext.Rotate(direction, pose.RotationVector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 TransformDirectionInverse(Vector2 direction, in Pose pose) =>
            Vector2Ext.RotateInverse(direction, pose.RotationVector);
    }
}