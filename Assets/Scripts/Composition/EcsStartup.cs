﻿using DELTation.DIFramework.Lifecycle;
using DELTation.LeoEcsExtensions.Services;
using JetBrains.Annotations;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.ExtendedSystems;
using Networking;
using Presentation;
using Presentation.Ball;
using Presentation.Interpolation;
using Presentation.Paddles;
using Simulation;
using Simulation.Ball;
using Simulation.Paddles;
using Simulation.Physics;
using UnityEngine;
#if UNITY_EDITOR
using Leopotam.EcsLite.UnityEditor;
#endif

namespace Composition
{
    public class EcsStartup : IUpdatable, IDestroyable, IMainEcsWorld
    {
        private readonly INetworkingSetUp _networkingSetUp;
        private readonly PresentationConfig _presentationConfig;
        private readonly SimulationConfig _simulationConfig;
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

            SimulationWorld = new EcsWorld();
            PresentationWorld = new EcsWorld();
        }

        private EcsWorld PresentationWorld { get; }

        public EcsWorld SimulationWorld { get; }

        public void OnDestroy()
        {
            _simulationSystems?.Destroy();
            _simulationSystems = null;
            _presentationSystems?.Destroy();
            _presentationSystems = null;
            SimulationWorld.Destroy();
            PresentationWorld.Destroy();
        }

        EcsWorld IMainEcsWorld.World => PresentationWorld;

        public void OnUpdate()
        {
            TryStart();
            TryRun();
        }

        private void TryStart()
        {
            if (_networkingSetUp.IsServer && _simulationSystems == null)
            {
                _simulationSystems = CreateSimulationSystems();
                _simulationSystems.Init();
            }

            if (_networkingSetUp.IsClient && _presentationSystems == null)
            {
                _presentationSystems = CreatePresentationSystems();
                _presentationSystems.Init();
            }
        }

        private void TryRun()
        {
            if (_networkingSetUp.IsServer)
                _simulationSystems?.Run(Time.unscaledDeltaTime);
            if (_networkingSetUp.IsClient)
                _presentationSystems?.Run();
        }

        [NotNull]
        private SimulationEcsSystems CreateSimulationSystems()
        {
            var systems = new EcsSystems(SimulationWorld, new SimulationSharedData(_simulationConfig.DeltaTime));
#if UNITY_EDITOR
            systems.Add(new EcsWorldDebugSystem());
#endif
            AddSimulationSystems(systems);
            systems.Inject();
            return new SimulationEcsSystems(systems);
        }

        [NotNull]
        private EcsSystems CreatePresentationSystems()
        {
            var systems = new EcsSystems(PresentationWorld,
                new PresentationSharedData(_presentationConfig.InterpolationSettings, _presentationConfig.Prefabs,
                    SimulationWorld
                )
            );
#if UNITY_EDITOR
            systems.Add(new EcsWorldDebugSystem());
#endif

            AddPresentationSystems(systems);
            systems.Inject();
            return systems;
        }

        private void AddSimulationSystems(EcsSystems systems)
        {
            systems.Add(new ReceiveClientInputSystem());

            systems
                .Add(new PaddleMovementSystem())
                .Add(new VelocitySystem())
                .Add(new BallWallBounceSystem())
                ;

            systems
                .DelHere<ClientInput>()
                .DelHere<InputClientId>()
                ;


            systems
                .Add(new ConstructSimulationStateSystem())
                .DelHere<OnPaddleDestroyed>()
                .DelHere<OnBallDestroyed>()
                ;

            systems
                .Add(new SendServerStateSystem())
                .DelHere<SimulationState>()
                ;
        }

        private void AddPresentationSystems(EcsSystems systems)
        {
            systems.Add(new ReceiveServerStateSystem());

            systems
                .Add(new PaddlePresentationSystem())
                .Add(new BallPresentationSystem())
                .Add(new PositionInterpolationSystem())
                .Add(new RotationInterpolationSystem())
                ;

            systems.DelHere<SimulationState>();

            systems
                .Add(new InputSystem())
                ;

            systems
                .Add(new SendClientInputSystem())
                .DelHere<ClientInput>()
                ;
        }
    }
}