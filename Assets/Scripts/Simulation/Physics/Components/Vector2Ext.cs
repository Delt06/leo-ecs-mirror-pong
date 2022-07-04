using System.Numerics;
using System.Runtime.CompilerServices;

namespace Simulation.Physics.Components
{
    public static class Vector2Ext
    {
        /// <summary>
        ///     a and b lay in XY plane, so result vector has only Z value
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Cross(Vector2 a, Vector2 b) => a.X * b.Y - b.X * a.Y;

        /// <summary>
        ///     a has only Z value, b lays in XY plane
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Cross(float aZ, Vector2 b) => new Vector2(-b.Y * aZ, aZ * b.X);

        /// <summary>
        ///     a lays in XY plane, b has only Z value
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Cross(Vector2 a, float bZ) => new Vector2(a.Y * bZ, -bZ * a.X);

        /// <summary>
        ///     Dot(RightPerp(a), b)
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float CrossPseudo(Vector2 a, Vector2 b) => a.Y * b.X - a.X * b.Y;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 NormalizeSafe(Vector2 v) => IsZero(v) ? v : Vector2.Normalize(v);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Rotate(Vector2 vector, Vector2 rotation) =>
            new Vector2(
                rotation.X * vector.X - rotation.Y * vector.Y,
                rotation.Y * vector.X + rotation.X * vector.Y
            );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 RotateInverse(Vector2 vector, Vector2 rotation) =>
            new Vector2(
                rotation.X * vector.X + rotation.Y * vector.Y,
                -rotation.Y * vector.X + rotation.X * vector.Y
            );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 LeftPerp(Vector2 vector) => new Vector2(-vector.Y, vector.X);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 RightPerp(Vector2 vector) => new Vector2(vector.Y, -vector.X);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsZero(Vector2 vector) => vector.X == 0 && vector.Y == 0;
    }
}