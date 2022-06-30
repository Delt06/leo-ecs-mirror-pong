using DELTation.DIFramework;
using DELTation.DIFramework.Containers;
using Presentation;
using Simulation;
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
                .Register<EcsStartup>()
                ;
        }
    }
}