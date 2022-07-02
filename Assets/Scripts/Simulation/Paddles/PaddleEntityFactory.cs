using DELTation.LeoEcsExtensions.Utilities;
using Leopotam.EcsLite;
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
            foreach (var i in _world.Filter<Paddle>().Inc<OwnerId>().End())
            {
                ref var ownerId = ref _world.GetPool<OwnerId>().Get(i);
                if (ownerId.Id != id) continue;

                _world.NewEntityWith<OnPaddleDestroyed>().OwnerId = ownerId.Id;
                _world.DelEntity(i);
            }
        }
    }
}