using Leopotam.EcsLite;

namespace Simulation
{
    public static class EcsSystemsExtensions
    {
        public static SimulationSharedData GetSimulationData(this EcsSystems ecsSystems) =>
            ecsSystems.GetShared<SimulationSharedData>();
    }
}