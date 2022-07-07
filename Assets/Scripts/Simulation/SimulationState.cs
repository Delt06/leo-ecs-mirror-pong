using System.Collections.Generic;
using Leopotam.EcsLite;
using Simulation.Ids;
using Simulation.Score;

namespace Simulation
{
    public struct SimulationState : IEcsAutoReset<SimulationState>
    {
        public List<EntityPosition> Positions;
        public List<EntityViewInfo> ViewIds;
        public List<SyncedEntityId> DestroyedEntities;
        public List<EntityScore> Scores;

        public void AutoReset(ref SimulationState c)
        {
            c.Positions ??= new List<EntityPosition>();
            c.Positions.Clear();

            c.ViewIds ??= new List<EntityViewInfo>();
            c.ViewIds.Clear();

            c.DestroyedEntities ??= new List<SyncedEntityId>();
            c.DestroyedEntities.Clear();
            
            c.Scores ??= new List<EntityScore>();
            c.Scores.Clear();
        }
    }
}