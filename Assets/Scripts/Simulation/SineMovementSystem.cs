using DELTation.LeoEcsExtensions.Systems.Run;
using DELTation.LeoEcsExtensions.Utilities;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Simulation
{
    public class SineMovementSystem : EcsSystemBase, IEcsRunSystem, IEcsInitSystem
    {
        private readonly EcsFilterInject<Inc<Cube>> _filter = default;
        private SimulationSharedData _simulationData;

        public void Init(EcsSystems systems)
        {
            _simulationData = systems.GetSimulationData();
            ref var cube = ref World.NewEntityWith<Cube>();
            cube.Rotation = Quaternion.identity;
        }

        public void Run(EcsSystems systems)
        {
            foreach (var i in _filter)
            {
                ref var cubeData = ref _filter.Pools.Inc1.Get(i);
                cubeData.Position = new Vector3(0, Mathf.Sin(cubeData.Time), 0f);
                cubeData.Time += _simulationData.Dt;
            }
        }
    }
}