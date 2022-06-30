using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Presentation.Interpolation
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
                    _interpolationSettings.InterpolatePosition(transform.position,
                        interpolatedPosition.TargetPosition,
                        interpolatedPosition.TimeSinceLastFrame
                    );
            }
        }
    }
}