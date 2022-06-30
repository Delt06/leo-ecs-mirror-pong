using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Simulation;

namespace Mirror
{
    public class SendServerStateSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<SimulationState>> _filter = default;

        public void Run(EcsSystems systems)
        {
            foreach (var i in _filter)
            {
                ref var simulationState = ref _filter.Pools.Inc1.Get(i);
                NetworkServer.SendToAll(new ServerStateMessage
                    {
                        SimulationState = simulationState,
                    }
                );
            }
        }
    }
}