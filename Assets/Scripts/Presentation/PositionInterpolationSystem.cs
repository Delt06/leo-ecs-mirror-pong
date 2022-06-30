using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Presentation
{
    public class PositionInterpolationSystem : IEcsRunSystem, IEcsInitSystem
    {
        private readonly EcsFilterInject<Inc<TransformRef, InterpolatedPosition>> _filter = default;
        private InterpolationSettings _interpolationSettings;

        public void Init(EcsSystems systems)
        {
            _interpolationSettings = systems.GetPresentationData().InterpolationSettings;
        }

        public void Run(EcsSystems systems)
        {
            foreach (var i in _filter)
            {
                ref var interpolatedPosition = ref _filter.Pools.Inc2.Get(i);
                interpolatedPosition.TimeSinceLastFrame += Time.deltaTime;
                var transform = _filter.Pools.Inc1.Get(i).Transform;
                transform.position =
                    Interpolate(transform.position, interpolatedPosition.TargetPosition,
                        interpolatedPosition.TimeSinceLastFrame
                    );
            }
        }

        private Vector3 Interpolate(Vector3 current, Vector3 target, float timeSinceLastFrame)
        {
            if (timeSinceLastFrame > _interpolationSettings.SnapTime)
                return target;
            var t = timeSinceLastFrame / _interpolationSettings.InterpolateTime;
            return _interpolationSettings.Extrapolate
                ? Vector3.LerpUnclamped(current, target, t)
                : Vector3.Lerp(current, target, t);
        }
    }
}