using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Simulation
{
    public class JumpRotationSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<Cube>> _cubeFilter = default;
        private readonly EcsFilterInject<Inc<ClientInput>> _inputFilter = default;

        public void Run(EcsSystems systems)
        {
            foreach (var iInput in _inputFilter)
            {
                var clientInput = _inputFilter.Pools.Inc1.Get(iInput);
                if (!clientInput.Jump) continue;

                foreach (var iCube in _cubeFilter)
                {
                    ref var cube = ref _cubeFilter.Pools.Inc1.Get(iCube);
                    cube.Rotation = Quaternion.Euler(15, 10, 5) * cube.Rotation;
                }
            }
        }
    }
}