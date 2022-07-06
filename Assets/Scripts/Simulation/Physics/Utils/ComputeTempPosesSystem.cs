using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Simulation.Physics.Components.Physics;
using Simulation.Physics.Components.Physics.Shapes;

namespace Simulation.Physics.Utils
{
    public class ComputeTempPosesSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<Segment>, Exc<Pose, TempPose>> _filter = default;
        private readonly EcsPoolInject<Pose> _poses = default;
        private readonly EcsPoolInject<TempPose> _tempPoses = default;

        public void Run(EcsSystems systems)
        {
            foreach (var i in _filter)
            {
                ref var segment = ref _filter.Pools.Inc1.Get(i);
                _poses.Value.Add(i).Position = (segment.A + segment.B) * 0.5f;
                _tempPoses.Value.Add(i);
            }
        }
    }
}