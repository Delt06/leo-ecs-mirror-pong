using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Networking;
using UnityEngine;
using Pose = Simulation.Physics.Components.Physics.Pose;

namespace Simulation.Paddles
{
    public class PaddleMovementSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<ClientInput, InputClientId>> _inputFilter = default;
        private readonly EcsFilterInject<Inc<Paddle, Pose, OwnerId>> _paddleFilter = default;

        public void Run(EcsSystems systems)
        {
            foreach (var iInput in _inputFilter)
            {
                var clientInput = _inputFilter.Pools.Inc1.Get(iInput);
                var inputClientId = _inputFilter.Pools.Inc2.Get(iInput);

                foreach (var iPaddle in _paddleFilter)
                {
                    var ownerId = _paddleFilter.Pools.Inc3.Get(iPaddle);
                    if (ownerId.Id != inputClientId.Id) continue;

                    ref var paddle = ref _paddleFilter.Pools.Inc1.Get(iPaddle);
                    ref var position = ref _paddleFilter.Pools.Inc2.Get(iPaddle);
                    var motion = clientInput.Motion * paddle.Speed;
                    position.Position.Y += motion;
                    position.Position.Y = Mathf.Clamp(position.Position.Y, -paddle.YLimit, paddle.YLimit);
                }
            }
        }
    }
}