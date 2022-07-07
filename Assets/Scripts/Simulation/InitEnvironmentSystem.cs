using System.Numerics;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Simulation.Ids;
using Simulation.Physics.Services;
using Simulation.Score;

namespace Simulation
{
    public class InitEnvironmentSystem : IEcsInitSystem
    {
        private readonly EcsCustomInject<PhysicsObjectsFactory> _physicsObjectsFactory = default;

        public void Init(EcsSystems systems)
        {
            var physicsObjectsFactory = _physicsObjectsFactory.Value;
            const int x = 6;
            const int y = 3;
            var world = systems.GetWorld();
            var viewPool = world.GetPool<ViewInfo>();
            const float restitution = 1;
            var segment1 = physicsObjectsFactory.CreateSegment(new Vector2(-x, y), new Vector2(x, y), restitution);
            var segment2 = physicsObjectsFactory.CreateSegment(new Vector2(-x, -y), new Vector2(x, -y), restitution);
            viewPool.Add(segment1).Type = ViewType.HorizontalWall;
            viewPool.Add(segment2).Type = ViewType.HorizontalWall;

            world.AddEntityId(segment1);
            world.AddEntityId(segment2);

            var scoreBodyPool = world.GetPool<ScoreBody>();
            var rightSegment = physicsObjectsFactory.CreateSegment(new Vector2(x, -y), new Vector2(x, y), restitution);
            scoreBodyPool.Add(rightSegment);
            var leftSegment = physicsObjectsFactory.CreateSegment(new Vector2(-x, -y), new Vector2(-x, y), restitution);
            scoreBodyPool.Add(leftSegment);
        }
    }
}