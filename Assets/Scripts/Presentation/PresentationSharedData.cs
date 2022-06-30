using Leopotam.EcsLite;
using Presentation.Interpolation;

namespace Presentation
{
    public class PresentationSharedData
    {
        public readonly InterpolationSettings InterpolationSettings;
        public readonly EcsWorld SimulationWorld;

        public PresentationSharedData(InterpolationSettings interpolationSettings, EcsWorld simulationWorld)
        {
            InterpolationSettings = interpolationSettings;
            SimulationWorld = simulationWorld;
        }
    }
}