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
            physicsObjectsFactory.CreateSegment(new Vector2(-5, 5), new Vector2(5, 5), 1);
            physicsObjectsFactory.CreateSegment(new Vector2(-5, -5), new Vector2(5, -5), 1);
        }
    }
}