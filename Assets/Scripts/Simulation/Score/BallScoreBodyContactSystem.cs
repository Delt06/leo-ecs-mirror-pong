using Composition;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Simulation.Ball;
using Simulation.Paddles;
using Simulation.Physics;
using Simulation.Physics.Components.Physics.Events;
using Simulation.Physics.Components.Physics.Tags;
using UnityEngine;
using Pose = Simulation.Physics.Components.Physics.Pose;

namespace Simulation.Score
{
    public class BallScoreBodyContactSystem : IEcsRunSystem, IEcsInitSystem
    {
        private readonly EcsFilterInject<Inc<Ball.Ball>> _ballFilter = default;
        private readonly EcsFilterInject<Inc<Contact<Dynamic, Static>>> _contactFilter = WorldNames.Events;
        private readonly EcsFilterInject<Inc<Paddle, Pose, PlayerScore>> _paddleFilter = default;
        private readonly EcsFilterInject<Inc<ScoreBody>> _scoreBodyFilter = default;
        private BallEntityFactory _ballEntityFactory;

        public void Init(EcsSystems systems)
        {
            _ballEntityFactory = DiExt.Resolve<BallEntityFactory>();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var i in _contactFilter)
            {
                ref var contact = ref _contactFilter.Pools.Inc1.Get(i);
                if (!_ballFilter.Contains(contact.BodyA)) continue;
                if (!_scoreBodyFilter.Contains(contact.BodyB)) continue;

                _ballEntityFactory.TryDestroyBall();
                _ballEntityFactory.CreateBall();

                var paddleIdx = GetPaddleIdxOrDefault(contact.Point.X);
                if (paddleIdx.HasValue)
                    _paddleFilter.Pools.Inc3.Get(paddleIdx.Value).Score++;
            }
        }

        private int? GetPaddleIdxOrDefault(float contactX)
        {
            var contactSign = (int) Mathf.Sign(contactX);
            foreach (var i in _paddleFilter)
            {
                var pose = _paddleFilter.Pools.Inc2.Get(i);
                var paddleSign = (int) Mathf.Sign(pose.Position.X);
                if (contactSign != paddleSign)
                    return i;
            }

            return null;
        }
    }
}