using DELTation.LeoEcsExtensions.Utilities;
using Leopotam.EcsLite;

namespace Simulation
{
    public class PaddleFactory
    {
        private readonly EcsWorld _world;

        public PaddleFactory(EcsWorld world) => _world = world;

        public void CreatePaddle(uint id)
        {
            var entity = _world.NewEntity();
            ref var paddle = ref _world.GetPool<Paddle>().Add(entity);
            paddle.Speed = 1f;
            ref var ownerId = ref _world.GetPool<OwnerId>().Add(entity);
            ownerId.Id = id;
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