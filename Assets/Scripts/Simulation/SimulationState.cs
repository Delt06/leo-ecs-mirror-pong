using System.Collections.Generic;
using Leopotam.EcsLite;
using Simulation.Ids;

namespace Simulation
{
    public struct SimulationState : IEcsAutoReset<SimulationState>
    {
        public List<EntityPosition> Positions;
        public List<EntityViewInfo> ViewIds;
        public List<SyncedEntityId> DestroyedEntities;

        public void AutoReset(ref SimulationState c)
        {
            c.Positions ??= new List<EntityPosition>();
            c.Positions.Clear();

            c.ViewIds ??= new List<EntityViewInfo>();
            c.ViewIds.Clear();

            c.DestroyedEntities ??= new List<SyncedEntityId>();
            c.DestroyedEntities.Clear();
        }
    }
}