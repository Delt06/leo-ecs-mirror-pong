using System.Collections.Generic;
using DELTation.LeoEcsExtensions.Components;
using DELTation.LeoEcsExtensions.Services;
using DELTation.LeoEcsExtensions.Systems.Run;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Simulation;
using Simulation.Ids;
using UnityEngine;

namespace Presentation
{
    public class EntityViewPresentationSystem : EcsSystemBase, IEcsRunSystem, IEcsInitSystem
    {
        private readonly EcsFilterInject<Inc<SimulationState>> _stateFilter = default;
        private readonly EcsPoolInject<ViewBackRef> _viewBackRefPool = default;
        private MainEcsWorld _mainEcsWorld;
        private Prefabs _prefabs;
        private Dictionary<SyncedEntityId, int> _viewEntityIds;

        public void Init(EcsSystems systems)
        {
            var presentationData = systems.GetPresentationData();
            _viewEntityIds = presentationData.ViewEntityIds;
            _prefabs = presentationData.Prefabs;
            _mainEcsWorld = new MainEcsWorld(systems.GetWorld());
        }

        public void Run(EcsSystems systems)
        {
            foreach (var i in _stateFilter)
            {
                ref readonly var simulationState = ref _stateFilter.Pools.Inc1.Get(i);
                foreach (var viewId in simulationState.ViewIds)
                {
                    if (_viewEntityIds.ContainsKey(viewId.Id)) continue;

                    var prefab = _prefabs.Resolve(viewId.ViewInfo.Type);
                    var entityView = Object.Instantiate(prefab);
                    entityView.Construct(_mainEcsWorld);
                    var entity = entityView.GetOrCreateEntity();
                    entity.Unpack(out _, out var idx);
                    _viewEntityIds[viewId.Id] = idx;
                }

                foreach (var destroyedEntity in simulationState.DestroyedEntities)
                {
                    if (!_viewEntityIds.TryGetValue(destroyedEntity, out var idx)) continue;
                    var viewBackRef = _viewBackRefPool.Value.Get(idx);
                    viewBackRef.View.Destroy();
                    _viewEntityIds.Remove(destroyedEntity);
                }
            }
        }
    }
}