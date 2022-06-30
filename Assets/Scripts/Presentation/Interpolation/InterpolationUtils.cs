using UnityEngine;

namespace Presentation.Interpolation
{
    public static class InterpolationUtils
    {
        public static Vector3 InterpolatePosition(this in InterpolationSettings interpolationSettings, Vector3 current,
            Vector3 target, float timeSinceLastFrame)
        {
            if (timeSinceLastFrame > interpolationSettings.SnapTime)
                return target;
            var t = timeSinceLastFrame / interpolationSettings.InterpolateTime;
            return interpolationSettings.Extrapolate
                ? Vector3.LerpUnclamped(current, target, t)
                : Vector3.Lerp(current, target, t);
        }

        public static Quaternion InterpolateRotation(this in InterpolationSettings interpolationSettings,
            Quaternion current,
            Quaternion target, float timeSinceLastFrame)
        {
            if (timeSinceLastFrame > interpolationSettings.SnapTime)
                return target;
            var t = timeSinceLastFrame / interpolationSettings.InterpolateTime;
            return interpolationSettings.Extrapolate
                ? Quaternion.SlerpUnclamped(current, target, t)
                : Quaternion.Slerp(current, target, t);
        }
    }
}