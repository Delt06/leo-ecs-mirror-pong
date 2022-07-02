using DELTation.LeoEcsExtensions.Systems.Run;
using DELTation.LeoEcsExtensions.Utilities;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Simulation.Ball;
using Simulation.Paddles;
using Simulation.Physics;

namespace Simulation
{
    public class ConstructSimulationStateSystem : EcsSystemBase, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<OnBallDestroyed>> _ballDestroyedFilter = default;
        private readonly EcsFilterInject<Inc<Ball.Ball, Position>> _ballFilter = default;
        private readonly EcsFilterInject<Inc<OnPaddleDestroyed>> _paddleDestroyedFilter = default;
        private readonly EcsFilterInject<Inc<Paddle, Position, OwnerId>> _paddlesFilter = default;

        public void Run(EcsSystems systems)
        {
            ref var simulationState = ref World.NewEntityWith<SimulationState>();

            foreach (var i in _paddlesFilter)
            {
                ref var position = ref _paddlesFilter.Pools.Inc2.Get(i);
                var ownerId = _paddlesFilter.Pools.Inc3.Get(i);
                var paddleInfo = new PaddleInfo
                {
                    Position = position.Value,
                    OwnerId = ownerId.Id,
                };
                simulationState.Paddles.Add(paddleInfo);
            }

            foreach (var i in _paddleDestroyedFilter)
            {
                var onPaddleDestroyed = _paddleDestroyedFilter.Pools.Inc1.Get(i);
                simulationState.DestroyedPaddles.Add(onPaddleDestroyed.OwnerId);
            }

            foreach (var _ in _ballDestroyedFilter)
            {
                simulationState.BallInfo.HasDestroyed = true;
                break;
            }

            foreach (var i in _ballFilter)
            {
                ref var position = ref _ballFilter.Pools.Inc2.Get(i);
                simulationState.BallInfo.Position = position.Value;
                break;
            }
        }
    }
}