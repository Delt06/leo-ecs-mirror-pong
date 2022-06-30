using DELTation.LeoEcsExtensions.Systems.Run;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Presentation.Interpolation;
using Simulation;
using UnityEngine;

namespace Presentation
{
    public class CubePresentationSystem : EcsSystemBase, IEcsRunSystem, IEcsInitSystem
    {
        private readonly EcsFilterInject<Inc<CubeView, InterpolatedPosition, InterpolatedRotation>> _cubeFilter =
            default;
        private readonly EcsFilterInject<Inc<SimulationState>> _stateFilter = default;

        public void Init(EcsSystems systems)
        {
            var idx = World.NewEntity();
            var gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Add<CubeView>(idx).GameObject = gameObject;
            Add<TransformRef>(idx).Transform = gameObject.transform;
            Add<InterpolatedPosition>(idx);
            Add<InterpolatedRotation>(idx).TargetRotation = Quaternion.identity;
        }

        public void Run(EcsSystems systems)
        {
            foreach (var iState in _stateFilter)
            {
                var simulationState = _stateFilter.Pools.Inc1.Get(iState);
                var cubePosition = simulationState.CubePosition;
                var cubeRotation = simulationState.CubeRotation;

                foreach (var iCube in _cubeFilter)
                {
                    ref var interpolatedPosition = ref _cubeFilter.Pools.Inc2.Get(iCube);
                    interpolatedPosition.TargetPosition = cubePosition;
                    interpolatedPosition.TimeSinceLastFrame = 0f;

                    ref var interpolatedRotation = ref _cubeFilter.Pools.Inc3.Get(iCube);
                    interpolatedRotation.TargetRotation = cubeRotation;
                    interpolatedRotation.TimeSinceLastFrame = 0f;
                }
            }
        }
    }
}