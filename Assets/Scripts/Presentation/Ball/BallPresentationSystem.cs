using DELTation.LeoEcsExtensions.Systems.Run;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Presentation.Interpolation;
using Simulation;
using UnityEngine;

namespace Presentation.Ball
{
    public class BallPresentationSystem : EcsSystemBase, IEcsRunSystem, IEcsInitSystem
    {
        private readonly EcsFilterInject<Inc<SimulationState>> _stateFilter = default;
        private readonly EcsFilterInject<Inc<BallView, InterpolatedPosition>> _viewFilter = default;
        private GameObject _ballPrefab;

        public void Init(EcsSystems systems)
        {
            _ballPrefab = systems.GetPresentationData().Prefabs.BallPrefab;
        }

        public void Run(EcsSystems systems)
        {
            foreach (var i in _stateFilter)
            {
                ref var simulationState = ref _stateFilter.Pools.Inc1.Get(i);

                if (simulationState.BallInfo.HasDestroyed)
                {
                    foreach (var iView in _viewFilter)
                    {
                        ref var ballView = ref _viewFilter.Pools.Inc1.Get(iView);
                        Object.Destroy(ballView.GameObject);
                        World.DelEntity(iView);
                    }
                }
                else if (simulationState.BallInfo.Position.HasValue)
                {
                    var position = simulationState.BallInfo.Position.Value;

                    if (_viewFilter.GetEntitiesCount() == 0)
                    {
                        var gameObject = Object.Instantiate(_ballPrefab, position, Quaternion.identity);
                        var entity = World.NewEntity();
                        Add<BallView>(entity).GameObject = gameObject;
                        Add<InterpolatedPosition>(entity);
                        Add<TransformRef>(entity).Transform = gameObject.transform;
                    }

                    foreach (var iView in _viewFilter)
                    {
                        ref var interpolatedPosition = ref _viewFilter.Pools.Inc2.Get(iView);
                        interpolatedPosition.TargetPosition = position;
                        interpolatedPosition.TimeSinceLastFrame = 0f;
                    }
                }
            }
        }
    }
}