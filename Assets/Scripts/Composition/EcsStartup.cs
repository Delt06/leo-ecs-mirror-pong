using DELTation.DIFramework.Lifecycle;
using DELTation.LeoEcsExtensions.Services;
using JetBrains.Annotations;
using Leopotam.EcsLite;
using Presentation;
using Simulation;
using UnityEngine;

namespace Composition
{
    public class EcsStartup : IUpdatable, IDestroyable, IMainEcsWorld, IStartable
    {
        private readonly INetworkingSetUp _networkingSetUp;
        private readonly SimulationConfig _simulationConfig;
        private readonly PresentationConfig _presentationConfig;
        [CanBeNull]
        private EcsSystems _presentationSystems;
        [CanBeNull]
        private SimulationEcsSystems _simulationSystems;

        public EcsStartup(SimulationConfig simulationConfig, PresentationConfig presentationConfig,
            INetworkingSetUp networkingSetUp)
        {
            _presentationConfig = presentationConfig;
            _simulationConfig = simulationConfig;
            _networkingSetUp = networkingSetUp;

            World = new EcsWorld();
        }

        public void OnDestroy()
        {
            _simulationSystems?.Destroy();
            _simulationSystems = null;
            _presentationSystems?.Destroy();
            _presentationSystems = null;
            World.Destroy();
        }

        public EcsWorld World { get; }

        public void OnStart()
        {
            if (_networkingSetUp.IsServer)
            {
                _simulationSystems ??= CreateSimulationSystems();
                _simulationSystems.Init();
            }

            if (_networkingSetUp.IsClient)
            {
                _presentationSystems ??= CreatePresentationSystems();
                _presentationSystems.Init();
            }
        }

        public void OnUpdate()
        {
            if (_networkingSetUp.IsServer)
                _simulationSystems?.Run(Time.unscaledDeltaTime);
            if (_networkingSetUp.IsClient)
                _presentationSystems?.Run();
        }

        [NotNull]
        private SimulationEcsSystems CreateSimulationSystems()
        {
            var simulationSystems = new EcsSystems(World, new SimulationSharedData(_simulationConfig.DeltaTime));
            AddSimulationSystems(simulationSystems);
            return new SimulationEcsSystems(simulationSystems);
        }

        [NotNull]
        private EcsSystems CreatePresentationSystems()
        {
            var systems = new EcsSystems(World, new PresentationSharedData(_presentationConfig.InterpolationSettings));
            AddPresentationSystems(_presentationSystems);
            return systems;
        }

        private void AddSimulationSystems(EcsSystems systems) { }

        private void AddPresentationSystems(EcsSystems systems) { }
    }
}