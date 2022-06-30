using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Presentation.Interpolation
{
    public class RotationInterpolationSystem : IEcsRunSystem, IEcsInitSystem
    {
        private readonly EcsFilterInject<Inc<TransformRef, InterpolatedRotation>> _filter = default;
        private InterpolationSettings _interpolationSettings;

        public void Init(EcsSystems systems)
        {
            _interpolationSettings = systems.GetPresentationData().InterpolationSettings;
        }

        public void Run(EcsSystems systems)
        {
            foreach (var i in _filter)
            {
                ref var interpolatedRotation = ref _filter.Pools.Inc2.Get(i);
                interpolatedRotation.TimeSinceLastFrame += Time.deltaTime;
                var transform = _filter.Pools.Inc1.Get(i).Transform;
                transform.rotation =
                    _interpolationSettings.InterpolateRotation(transform.rotation,
                        interpolatedRotation.TargetRotation,
                        interpolatedRotation.TimeSinceLastFrame
                    );
            }
        }
    }
}