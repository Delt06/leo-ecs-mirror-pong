using System.Collections.Generic;
using DELTation.LeoEcsExtensions.Systems.Run;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Presentation.Interpolation;
using Simulation;
using UnityEngine;

namespace Presentation
{
    public class PaddlePresentationSystem : EcsSystemBase, IEcsRunSystem, IEcsInitSystem
    {
        private readonly EcsFilterInject<Inc<SimulationState>> _stateFilter = default;
        private readonly Dictionary<uint, int> _viewsByOwnerId = new();
        private GameObject _paddlePrefab;

        public void Init(EcsSystems systems)
        {
            _paddlePrefab = systems.GetPresentationData().Prefabs.PaddlePrefab;
        }

        public void Run(EcsSystems systems)
        {
            foreach (var iState in _stateFilter)
            {
                var simulationState = _stateFilter.Pools.Inc1.Get(iState);

                foreach (var paddle in simulationState.Paddles)
                {
                    if (!_viewsByOwnerId.TryGetValue(paddle.OwnerId, out var entity))
                    {
                        var gameObject = Object.Instantiate(_paddlePrefab, paddle.Position, Quaternion.identity);
                        entity = World.NewEntity();
                        Add<TransformRef>(entity).Transform = gameObject.transform;
                        Add<PaddleView>(entity).GameObject = gameObject;
                        Add<InterpolatedPosition>(entity).TargetPosition = paddle.Position;
                        _viewsByOwnerId.Add(paddle.OwnerId, entity);
                    }
                    else
                    {
                        ref var interpolatedPosition = ref Get<InterpolatedPosition>(entity);
                        interpolatedPosition.TargetPosition = paddle.Position;
                        interpolatedPosition.TimeSinceLastFrame = 0f;
                    }
                }

                foreach (var destroyedPaddle in simulationState.DestroyedPaddles)
                {
                    if (!_viewsByOwnerId.TryGetValue(destroyedPaddle, out var entity)) continue;
                    Object.Destroy(Get<PaddleView>(entity).GameObject);
                    World.DelEntity(entity);
                    _viewsByOwnerId.Remove(destroyedPaddle);
                }
            }
        }
    }
}