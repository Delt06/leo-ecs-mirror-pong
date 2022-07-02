using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Simulation.Physics;
using UnityEngine;

namespace Simulation.Ball
{
    public class BallWallBounceSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<Ball, Position, Velocity>> _filter = default;

        public void Run(EcsSystems systems)
        {
            foreach (var i in _filter)
            {
                const float wallOffset = 3f;
                ref var position = ref _filter.Pools.Inc2.Get(i);
                if (Mathf.Abs(position.Value.y) < wallOffset) continue;

                position.Value.y = Mathf.Sign(position.Value.y) * (wallOffset - 0.001f);

                ref var velocity = ref _filter.Pools.Inc3.Get(i);
                velocity.Value = Vector2.Reflect(velocity.Value, Vector2.up);
            }
        }
    }
}