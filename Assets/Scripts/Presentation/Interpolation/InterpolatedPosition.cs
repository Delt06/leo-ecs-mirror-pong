using UnityEngine;

namespace Presentation.Interpolation
{
    public struct InterpolatedPosition
    {
        public Vector3 TargetPosition;
        public float TimeSinceLastFrame;
        public bool IsValid;
    }
}