using DELTation.LeoEcsExtensions.Utilities;
using Leopotam.EcsLite;
using Simulation.Ids;
using Simulation.Physics.Components.Physics;
using Simulation.Physics.Services;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace Simulation.Ball
{
    public class BallEntityFactory
    {
        private readonly EcsWorld _world;
        private readonly PhysicsObjectsFactory _physicsObjectsFactory;

        public BallEntityFactory(EcsWorld world, PhysicsObjectsFactory physicsObjectsFactory)
        {
            _world = world;
            _physicsObjectsFactory = physicsObjectsFactory;
        }

        public void CreateBall()
        {
            var entity = _world.NewEntity();
            _world.GetPool<Ball>().Add(entity);
            _physicsObjectsFactory.CreateCircle(Vector2.Zero, 0.5f, 1f, 1f, entity);

            const float speed = 5f;
            var direction = GetRandomDirection();
            _world.GetPool<Velocity>().Get(entity).Linear = direction * speed;
            _world.AddEntityId(entity);
            _world.GetPool<ViewInfo>().Add(entity).Type = ViewType.Ball;
        }

        private Vector2 GetRandomDirection() => Vector2.Normalize(new Vector2(Random.value, Random.value));

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