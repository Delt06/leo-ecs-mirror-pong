using System.Numerics;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Simulation.Physics.Services;

namespace Simulation
{
    public class InitEnvironmentSystem : IEcsInitSystem
    {
        private readonly EcsCustomInject<PhysicsObjectsFactory> _physicsObjectsFactory = default;

        public void Init(EcsSystems systems)
        {
            var physicsObjectsFactory = _physicsObjectsFactory.Value;
            const int x = 5;
            const int y = 3;
            physicsObjectsFactory.CreateSegment(new Vector2(-x, y), new Vector2(x, y), 1);
            physicsObjectsFactory.CreateSegment(new Vector2(-x, -y), new Vector2(x, -y), 1);
        }
    }
}