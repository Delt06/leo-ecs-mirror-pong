using DELTation.DIFramework;
using DELTation.DIFramework.Containers;
using Networking;
using Presentation;
using Simulation;
using Simulation.Ball;
using Simulation.Paddles;
using UnityEngine;

namespace Composition
{
    public class CompositionRoot : DependencyContainerBase
    {
        [SerializeField] private SimulationConfig _simulationConfig;
        [SerializeField] private PresentationConfig _presentationConfig;

        protected override void ComposeDependencies(ICanRegisterContainerBuilder builder)
        {
            builder
                .Register(_simulationConfig)
                .Register(_presentationConfig)
                .Register<MirrorNetworkingSetUp>()
                .Register<EcsStartup>()
                .RegisterFromMethod((EcsStartup startup) => startup.SimulationWorld)
                ;

            builder
                .Register<PaddleEntityFactory>()
                .Register<BallEntityFactory>()
                .RegisterFromMethod((EcsStartup startup) => startup.PhysicsObjectsFactory)
                ;
        }
    }
}