using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Networking;

namespace Simulation.Paddles
{
    public class MovementSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<ClientInput, InputClientId>> _inputFilter = default;
        private readonly EcsFilterInject<Inc<Paddle, OwnerId>> _paddleFilter = default;

        public void Run(EcsSystems systems)
        {
            foreach (var iInput in _inputFilter)
            {
                var clientInput = _inputFilter.Pools.Inc1.Get(iInput);
                var inputClientId = _inputFilter.Pools.Inc2.Get(iInput);

                foreach (var iPaddle in _paddleFilter)
                {
                    var ownerId = _paddleFilter.Pools.Inc2.Get(iPaddle);
                    if (ownerId.Id != inputClientId.Id) continue;

                    ref var paddle = ref _paddleFilter.Pools.Inc1.Get(iPaddle);
                    var motion = clientInput.Motion * paddle.Speed;
                    paddle.Position.y += motion;
                }
            }
        }
    }
}