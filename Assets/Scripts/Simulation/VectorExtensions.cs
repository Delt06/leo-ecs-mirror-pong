using UnityEngine;

namespace Simulation
{
    public static class VectorExtensions
    {
        public static Vector2 ToUnityVector(this System.Numerics.Vector2 vector) => new Vector2(vector.X, vector.Y);
    }
}