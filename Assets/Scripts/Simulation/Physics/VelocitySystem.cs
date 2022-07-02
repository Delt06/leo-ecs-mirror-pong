using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Simulation.Physics
{
    public class VelocitySystem : IEcsRunSystem, IEcsInitSystem
    {
        private readonly EcsFilterInject<Inc<Position, Velocity>> _filter = default;
        private SimulationSharedData _simulationData;

        public void Init(EcsSystems systems)
        {
            _simulationData = systems.GetSimulationData();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var i in _filter)
            {
                ref var position = ref _filter.Pools.Inc1.Get(i);
                ref readonly var velocity = ref _filter.Pools.Inc2.Get(i);
                position.Value += velocity.Value * _simulationData.Dt;
            }
        }
    }
}