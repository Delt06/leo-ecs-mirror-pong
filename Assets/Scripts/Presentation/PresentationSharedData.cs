using Leopotam.EcsLite;
using Presentation.Interpolation;

namespace Presentation
{
    public class PresentationSharedData
    {
        public readonly InterpolationSettings InterpolationSettings;
        public readonly Prefabs Prefabs;
        public readonly EcsWorld SimulationWorld;

        public PresentationSharedData(InterpolationSettings interpolationSettings, Prefabs prefabs,
            EcsWorld simulationWorld)
        {
            InterpolationSettings = interpolationSettings;
            SimulationWorld = simulationWorld;
            Prefabs = prefabs;
        }
    }
}