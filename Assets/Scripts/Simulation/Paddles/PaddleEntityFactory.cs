using DELTation.LeoEcsExtensions.Utilities;
using Leopotam.EcsLite;
using Simulation.Ids;
using Simulation.Physics;

namespace Simulation.Paddles
{
    public class PaddleEntityFactory
    {
        private readonly EcsFilter _paddlesFilter;
        private readonly EcsWorld _world;

        public PaddleEntityFactory(EcsWorld world)
        {
            _world = world;
            _paddlesFilter = _world.Filter<Paddle>().End();
        }

        public int PaddlesCount => _paddlesFilter.GetEntitiesCount();

        public void CreatePaddle(uint id)
        {
            var entity = _world.NewEntity();
            ref var paddle = ref _world.GetPool<Paddle>().Add(entity);
            paddle.Speed = 5f;
            _world.GetPool<Position>().Add(entity)
                .Value.x = 5f * GetNewPaddleSide();
            ref var ownerId = ref _world.GetPool<OwnerId>().Add(entity);
            ownerId.Id = id;
            _world.AddEntityId(entity);
            _world.GetPool<ViewInfo>().Add(entity).Type = ViewType.Paddle;
        }

        private float GetNewPaddleSide()
        {
            foreach (var i in _world.Filter<Paddle>().Inc<Position>().End())
            {
                var position = _world.GetPool<Position>().Get(i);
                if (position.Value.x > 0)
                    return -1f;
                return 1f;
            }

            return -1f;
        }

        public void TryDestroyPaddle(uint id)
        {
            foreach (var i in _world.Filter<Paddle>().Inc<OwnerId>().Inc<SyncedEntityId>().End())
            {
                ref var ownerId = ref _world.GetPool<OwnerId>().Get(i);
                if (ownerId.Id != id) continue;

                var entityId = _world.GetPool<SyncedEntityId>().Get(i);

                _world.NewEntityWith<OnSyncedEntityDestroyed>().Id = entityId;
                _world.DelEntity(i);
            }
        }
    }
}