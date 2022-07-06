using System.Numerics;
using DELTation.LeoEcsExtensions.Utilities;
using Leopotam.EcsLite;
using Simulation.Ids;
using Simulation.Physics.Components;
using Simulation.Physics.Components.Physics;
using Simulation.Physics.Components.Physics.Shapes;
using Simulation.Physics.Components.Physics.Tags;
using Simulation.Physics.Services;

namespace Simulation.Paddles
{
    public class PaddleEntityFactory
    {
        private readonly EcsFilter _paddlesFilter;
        private readonly PhysicsObjectsFactory _physicsObjectsFactory;
        private readonly EcsWorld _world;
        private Triangle[] _paddleTriangles;

        public PaddleEntityFactory(EcsWorld world, PhysicsObjectsFactory physicsObjectsFactory)
        {
            _world = world;
            _physicsObjectsFactory = physicsObjectsFactory;
            _paddlesFilter = _world.Filter<Paddle>().End();
        }

        public int PaddlesCount => _paddlesFilter.GetEntitiesCount();

        public void CreatePaddle(uint id)
        {
            var entity = _world.NewEntity();
            ref var paddle = ref _world.GetPool<Paddle>().Add(entity);
            paddle.Speed = 5f;
            var x = 5f * GetNewPaddleSide();
            _paddleTriangles = new[]
            {
                new Triangle(new Vector2(-0.5f, -0.5f), new Vector2(0.5f, -0.5f), new Vector2(0.5f, 0.5f)),
                new Triangle(new Vector2(-0.5f, -0.5f), new Vector2(0.5f, 0.5f), new Vector2(-0.5f, 0.5f)),
            };
            var mesh = Mesh.CreateMesh(_paddleTriangles);
            _physicsObjectsFactory.CreateStaticMesh(mesh, 1f, new Vector2(x, 0f), 0f, entity);

            _world.GetPool<BodyMask>().Get(entity).TagTypeId = Static.Id;

            ref var ownerId = ref _world.GetPool<OwnerId>().Add(entity);
            ownerId.Id = id;
            _world.AddEntityId(entity);
            _world.GetPool<ViewInfo>().Add(entity).Type = ViewType.Paddle;
        }

        private float GetNewPaddleSide()
        {
            foreach (var i in _world.Filter<Paddle>().Inc<Pose>().End())
            {
                var position = _world.GetPool<Pose>().Get(i);
                if (position.Position.X > 0)
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