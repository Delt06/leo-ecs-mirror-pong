using UnityEngine;

namespace Simulation
{
    public static class VectorExtensions
    {
        public static Vector2 ToUnityVector(this System.Numerics.Vector2 vector) => new(vector.X, vector.Y);
        public static System.Numerics.Vector2 ToNumericsVector(this Vector2 vector) => new(vector.x, vector.y);
    }
}