using DELTation.LeoEcsExtensions.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Presentation.Interpolation
{
    public class PositionInterpolationSystem : IEcsRunSystem, IEcsInitSystem
    {
        private readonly EcsFilterInject<Inc<UnityRef<Transform>, InterpolatedPosition>> _filter = default;
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
                if (!interpolatedPosition.IsValid) continue;

                interpolatedPosition.TimeSinceLastFrame += Time.deltaTime;
                var transform = _filter.Pools.Inc1.Get(i).Object;
                transform.position =
                    _interpolationSettings.InterpolatePosition(transform.position,
                        interpolatedPosition.TargetPosition,
                        interpolatedPosition.TimeSinceLastFrame
                    );
            }
        }
    }
}