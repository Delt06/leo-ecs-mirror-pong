using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Simulation.Physics.Components.Physics;

namespace Simulation.Physics.Utils
{
    public class ResetTempPosesSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<Pose, TempPose>> _filter = default;

        public void Run(EcsSystems systems)
        {
            foreach (var i in _filter)
            {
                _filter.Pools.Inc1.Del(i);
                _filter.Pools.Inc2.Del(i);
            }
        }
    }
}