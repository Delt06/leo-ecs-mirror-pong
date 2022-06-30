using System;

namespace Presentation
{
    [Serializable]
    public struct InterpolationSettings
    {
        public float InterpolateTime;
        public float SnapTime;
        public bool Extrapolate;
    }
}