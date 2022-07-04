using UnityEngine;

namespace Presentation.Interpolation
{
    public struct InterpolatedRotation
    {
        public Quaternion TargetRotation;
        public float TimeSinceLastFrame;
        public bool IsValid;
    }
}