using System.Collections.Generic;
using DELTation.LeoEcsExtensions.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Presentation.Interpolation;
using Simulation;
using Simulation.Ids;
using UnityEngine;

namespace Presentation
{
    public class PositionPresentationSystem : IEcsRunSystem, IEcsInitSystem
    {
        private readonly EcsPoolInject<InterpolatedPosition> _interpolatedPositionsPool = default;
        private readonly EcsFilterInject<Inc<SimulationState>> _stateFilter = default;
        private readonly EcsPoolInject<UnityRef<Transform>> _transformsPool = default;
        private IReadOnlyDictionary<SyncedEntityId, int> _viewEntityIds;

        public void Init(EcsSystems systems)
        {
            _viewEntityIds = systems.GetPresentationData().ViewEntityIds;
        }

        public void Run(EcsSystems systems)
        {
            foreach (var i in _stateFilter)
            {
                ref readonly var simulationState = ref _stateFilter.Pools.Inc1.Get(i);
                foreach (var entityPosition in simulationState.Positions)
                {
                    if (!IsValidFloat(entityPosition.Position.x) ||
                        !IsValidFloat(entityPosition.Position.y))
                        continue;

                    var syncedEntityId = entityPosition.Id;
                    if (!_viewEntityIds.TryGetValue(syncedEntityId, out var id))
                    {
                        Debug.LogWarning("Entity " + syncedEntityId + " does not have a view.");
                        continue;
                    }

                    ref var interpolatedPosition = ref _interpolatedPositionsPool.Value.Get(id);
                    Vector3 targetPosition = entityPosition.Position;

                    if (!interpolatedPosition.IsValid)
                    {
                        interpolatedPosition.IsValid = true;
                        var transform = _transformsPool.Value.Get(id).Object;
                        transform.position = targetPosition;
                    }

                    interpolatedPosition.TargetPosition = targetPosition;
                    interpolatedPosition.TimeSinceLastFrame = 0f;
                }
            }
        }

        private bool IsValidFloat(float value) => !float.IsNaN(value) && !float.IsInfinity(value);
    }
}