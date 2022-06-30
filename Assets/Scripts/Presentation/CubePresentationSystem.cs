using DELTation.LeoEcsExtensions.Systems.Run;
using DELTation.LeoEcsExtensions.Utilities;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Simulation;
using UnityEngine;

namespace Presentation
{
    public class CubePresentationSystem : EcsSystemBase, IEcsRunSystem, IEcsInitSystem
    {
        private readonly EcsFilterInject<Inc<SimulationState>> _stateFilter = default;
        private readonly EcsFilterInject<Inc<CubeView>> _viewsFilter = default;
        private InterpolationSettings _interpolationSettings;

        public void Init(EcsSystems systems)
        {
            ref var cubeView = ref World.NewEntityWith<CubeView>();
            cubeView.GameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            _interpolationSettings = systems.GetPresentationData().InterpolationSettings;
        }

        public void Run(EcsSystems systems)
        {
            foreach (var iState in _stateFilter)
            {
                var cubePosition = _stateFilter.Pools.Inc1.Get(iState).CubePosition;
                foreach (var iView in _viewsFilter)
                {
                    ref var cubeView = ref _viewsFilter.Pools.Inc1.Get(iView);
                    cubeView.TargetPosition = cubePosition;
                    cubeView.TimeSinceLastFrame = 0f;
                }
            }

            foreach (var i in _viewsFilter)
            {
                ref var cubeView = ref _viewsFilter.Pools.Inc1.Get(i);
                var transform = cubeView.GameObject.transform;
                cubeView.TimeSinceLastFrame += Time.deltaTime;
                transform.position =
                    Interpolate(transform.position, cubeView.TargetPosition, cubeView.TimeSinceLastFrame);
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