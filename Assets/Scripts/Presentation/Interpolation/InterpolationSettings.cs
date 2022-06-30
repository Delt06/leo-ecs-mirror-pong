using System;

namespace Presentation.Interpolation
{
    [Serializable]
    public struct InterpolationSettings
    {
        public float InterpolateTime;
        public float SnapTime;
        public bool Extrapolate;
    }
}