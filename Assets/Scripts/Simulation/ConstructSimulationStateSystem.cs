using DELTation.LeoEcsExtensions.Systems.Run;
using DELTation.LeoEcsExtensions.Utilities;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Simulation.Ids;
using Simulation.Physics.Components.Physics;
using Simulation.Physics.Components.Physics.Shapes;

namespace Simulation
{
    public class ConstructSimulationStateSystem : EcsSystemBase, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<OnSyncedEntityDestroyed>> _onSyncedEntityDestroyedFilter = default;
        private readonly EcsFilterInject<Inc<SyncedEntityId, Pose>> _positionFilter = default;
        private readonly EcsFilterInject<Inc<SyncedEntityId, Segment>> _segmentFilter = default;
        private readonly EcsFilterInject<Inc<SyncedEntityId, ViewInfo>> _viewFilter = default;

        public void Run(EcsSystems systems)
        {
            ref var simulationState = ref World.NewEntityWith<SimulationState>();

            foreach (var i in _onSyncedEntityDestroyedFilter)
            {
                var syncedEntityId = _onSyncedEntityDestroyedFilter.Pools.Inc1.Get(i).Id;
                simulationState.DestroyedEntities.Add(syncedEntityId);
            }

            foreach (var i in _positionFilter)
            {
                var id = _positionFilter.Pools.Inc1.Get(i);
                var position = _positionFilter.Pools.Inc2.Get(i).Position;
                simulationState.Positions.Add(new EntityPosition(id, position.ToUnityVector()));
            }

            foreach (var i in _segmentFilter)
            {
                var id = _segmentFilter.Pools.Inc1.Get(i);
                var segment = _segmentFilter.Pools.Inc2.Get(i);
                var position = (segment.A + segment.B) * 0.5f;
                simulationState.Positions.Add(new EntityPosition(id, position.ToUnityVector()));
            }

            foreach (var i in _viewFilter)
            {
                var id = _viewFilter.Pools.Inc1.Get(i);
                var viewInfo = _viewFilter.Pools.Inc2.Get(i);
                simulationState.ViewIds.Add(new EntityViewInfo(id, viewInfo));
            }
        }
    }
}