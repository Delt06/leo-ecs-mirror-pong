using DELTation.LeoEcsExtensions.Systems.Run;
using DELTation.LeoEcsExtensions.Utilities;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Simulation.Paddles;

namespace Simulation
{
    public class ConstructSimulationStateSystem : EcsSystemBase, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<OnPaddleDestroyed>> _paddleDestroyedFilter = default;
        private readonly EcsFilterInject<Inc<Paddle, OwnerId>> _paddlesFilter = default;

        public void Run(EcsSystems systems)
        {
            ref var simulationState = ref World.NewEntityWith<SimulationState>();

            foreach (var i in _paddlesFilter)
            {
                ref var paddle = ref _paddlesFilter.Pools.Inc1.Get(i);
                var ownerId = _paddlesFilter.Pools.Inc2.Get(i);
                var paddleInfo = new PaddleInfo
                {
                    Position = paddle.Position,
                    OwnerId = ownerId.Id,
                };
                simulationState.Paddles.Add(paddleInfo);
            }

            foreach (var i in _paddleDestroyedFilter)
            {
                var onPaddleDestroyed = _paddleDestroyedFilter.Pools.Inc1.Get(i);
                simulationState.DestroyedPaddles.Add(onPaddleDestroyed.OwnerId);
            }
        }
    }
}