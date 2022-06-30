using Leopotam.EcsLite;
using Simulation;

namespace Composition
{
    public class SimulationEcsSystems
    {
        private readonly SimulationSharedData _sharedData;
        private readonly EcsSystems _systems;
        private float _timeSinceLastRun;

        public SimulationEcsSystems(EcsSystems systems)
        {
            _systems = systems;
            _sharedData = systems.GetSimulationData();
        }

        public void Init() => _systems.Init();

        public void Destroy() => _systems.Destroy();

        public void Run(float dt)
        {
            _timeSinceLastRun += dt;
            while (_timeSinceLastRun >= dt)
            {
                _timeSinceLastRun -= _sharedData.DeltaTime;
                _systems.Run();
            }
        }
    }
}