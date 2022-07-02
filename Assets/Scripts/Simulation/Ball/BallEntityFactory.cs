using DELTation.LeoEcsExtensions.Utilities;
using Leopotam.EcsLite;
using Simulation.Physics;
using UnityEngine;

namespace Simulation.Ball
{
    public class BallEntityFactory
    {
        private readonly EcsWorld _world;

        public BallEntityFactory(EcsWorld world) => _world = world;

        public void CreateBall()
        {
            var entity = _world.NewEntity();
            _world.GetPool<Ball>().Add(entity);
            _world.GetPool<Position>().Add(entity);

            const float speed = 5f;
            var direction = Random.insideUnitCircle.normalized;
            _world.GetPool<Velocity>().Add(entity).Value = direction * speed;
        }

        public void TryDestroyBall()
        {
            foreach (var i in _world.Filter<Ball>().End())
            {
                _world.DelEntity(i);
                _world.NewEntityWith<OnBallDestroyed>();
            }
        }
    }
}