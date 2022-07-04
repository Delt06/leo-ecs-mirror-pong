using System.Collections.Generic;
using Leopotam.EcsLite;
using Presentation.Interpolation;
using Simulation.Ids;

namespace Presentation
{
    public class PresentationSharedData
    {
        public readonly InterpolationSettings InterpolationSettings;
        public readonly Prefabs Prefabs;
        public readonly EcsWorld SimulationWorld;
        public readonly Dictionary<SyncedEntityId, int> ViewEntityIds = new();

        public PresentationSharedData(InterpolationSettings interpolationSettings, Prefabs prefabs,
            EcsWorld simulationWorld)
        {
            InterpolationSettings = interpolationSettings;
            SimulationWorld = simulationWorld;
            Prefabs = prefabs;
        }
    }
}