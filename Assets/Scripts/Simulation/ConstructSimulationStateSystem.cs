using DELTation.LeoEcsExtensions.Systems.Run;
using DELTation.LeoEcsExtensions.Utilities;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Simulation
{
    public class ConstructSimulationStateSystem : EcsSystemBase, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<Cube>> _cubesFilter = default;

        public void Run(EcsSystems systems)
        {
            foreach (var i in _cubesFilter)
            {
                ref var cubeData = ref _cubesFilter.Pools.Inc1.Get(i);
                ref var simulationState = ref World.NewEntityWith<SimulationState>();
                simulationState.CubePosition = cubeData.Position;
            }
        }
    }
}