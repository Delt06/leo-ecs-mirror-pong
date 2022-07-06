using DELTation.DIFramework.Lifecycle;
using DELTation.LeoEcsExtensions.Services;
using JetBrains.Annotations;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.ExtendedSystems;
using Networking;
using Presentation;
using Presentation.Interpolation;
using Simulation;
using Simulation.Debugging;
using Simulation.Ids;
using Simulation.Paddles;
using Simulation.Physics;
using Simulation.Physics.Components.Physics.Tags;
using Simulation.Physics.Services;
using Simulation.Physics.Systems.BroadPhase;
using Simulation.Physics.Systems.ForceGenerators;
using Simulation.Physics.Systems.NarrowPhase;
using Simulation.Physics.Systems.SolverPhase;
using Simulation.Physics.Systems.UpdateBV;
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
            PhysicsEventsWorld = new EcsWorld();
            PhysicsConstraintsWorld = new EcsWorld();
            PhysicsObjectsFactory = new PhysicsObjectsFactory(SimulationWorld, PhysicsConstraintsWorld);
        }

        public PhysicsObjectsFactory PhysicsObjectsFactory { get; }

        public EcsWorld PhysicsConstraintsWorld { get; }
        public EcsWorld PhysicsEventsWorld { get; }

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

            PhysicsEventsWorld.Destroy();
            PhysicsConstraintsWorld.Destroy();
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
            systems
                .Add(new InitEnvironmentSystem())
                ;
            systems.Add(new ReceiveClientInputSystem());

            systems
                .Add(new PaddleMovementSystem())
                ;

            systems
                .AddWorld(PhysicsEventsWorld, WorldNames.Events)
                .AddWorld(PhysicsConstraintsWorld, WorldNames.Constraints)
                // force generators
                .Add(new UpdateSprings())
                // apply forces
                .Add(new IntegratePoses())
                // update bounding volumes
                .Add(new UpdateCirclesBV())
                .Add(new UpdateMeshesBV())
                // broad phase
                .Add(new BroadPhase())
                // narrow phase
                .Add(new ContactTesterCircleCircle())
                .Add(new ContactTesterCircleMesh())
                .Add(new ContactTesterCircleSegment())
                .Add(new ContactTesterMeshSegment())
                // solver phase
                .Add(new DynamicDefaultContactSolver<Dynamic, Dynamic>())
                .Add(new StaticDefaultContactSolver<Dynamic, Static>())
                .Inject(new PhysicsData(_simulationConfig.DeltaTime, 1, 3, 2))
                ;

            systems
                .DelHere<ClientInput>()
                .DelHere<InputClientId>()
                ;


            systems
                .Add(new ConstructSimulationStateSystem())
                .DelHere<OnSyncedEntityDestroyed>()
                ;

            systems
                .Add(new SendServerStateSystem())
                .DelHere<SimulationState>()
                ;

            systems
                .Add(new ShapesGizmosSystem())
                ;

            systems.Inject(PhysicsObjectsFactory);
        }

        private void AddPresentationSystems(EcsSystems systems)
        {
            systems.Add(new ReceiveServerStateSystem());

            systems
                .Add(new EntityViewPresentationSystem())
                .Add(new PositionPresentationSystem())
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