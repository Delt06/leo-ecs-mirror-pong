using DELTation.LeoEcsExtensions.Systems.Run;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Simulation;
using UnityEngine;

namespace Presentation
{
    public class CubePresentationSystem : EcsSystemBase, IEcsRunSystem, IEcsInitSystem
    {
        private readonly EcsFilterInject<Inc<SimulationState>> _stateFilter = default;
        private readonly EcsFilterInject<Inc<CubeView, InterpolatedPosition>> _viewsFilter = default;

        public void Init(EcsSystems systems)
        {
            var idx = World.NewEntity();
            var gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Add<CubeView>(idx).GameObject = gameObject;
            Add<TransformRef>(idx).Transform = gameObject.transform;
            Add<InterpolatedPosition>(idx);
        }

        public void Run(EcsSystems systems)
        {
            foreach (var iState in _stateFilter)
            {
                var cubePosition = _stateFilter.Pools.Inc1.Get(iState).CubePosition;
                foreach (var iCube in _viewsFilter)
                {
                    ref var interpolatedPosition = ref _viewsFilter.Pools.Inc2.Get(iCube);
                    interpolatedPosition.TargetPosition = cubePosition;
                    interpolatedPosition.TimeSinceLastFrame = 0f;
                }
            }
        }
    }
}