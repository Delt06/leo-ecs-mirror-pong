using DELTation.LeoEcsExtensions.Utilities;
using Leopotam.EcsLite;
using Presentation.Interpolation;
using Simulation;

namespace Presentation
{
    public class PresentationSharedData
    {
        private readonly EcsWorld _simulationWorld;
        public readonly InterpolationSettings InterpolationSettings;

        public PresentationSharedData(InterpolationSettings interpolationSettings, EcsWorld simulationWorld)
        {
            InterpolationSettings = interpolationSettings;
            _simulationWorld = simulationWorld;
        }

        public void SendClientInputLocally(ClientInput input) =>
            _simulationWorld.NewEntityWith<ClientInput>() = input;
    }
}