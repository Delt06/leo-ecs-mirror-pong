using DELTation.LeoEcsExtensions.Utilities;
using Leopotam.EcsLite;
using Simulation.Ids;
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
            _world.AddEntityId(entity);
            _world.GetPool<ViewInfo>().Add(entity).Type = ViewType.Ball;
        }

        public void TryDestroyBall()
        {
            foreach (var i in _world.Filter<Ball>().Inc<SyncedEntityId>().End())
            {
                var id = _world.GetPool<SyncedEntityId>().Get(i);
                _world.NewEntityWith<OnSyncedEntityDestroyed>().Id = id;
                _world.DelEntity(i);
            }
        }
    }
}