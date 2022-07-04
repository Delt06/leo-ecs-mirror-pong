using Leopotam.EcsLite;
using Simulation.Physics.Components.Physics;
using Simulation.Physics.Components.Physics.Shapes;

namespace Simulation.Physics.Systems.UpdateBV
{
    public abstract class UpdateBV<Shape> : IEcsInitSystem, IEcsRunSystem
        where Shape : struct, IShape
    {
        private EcsPool<BoundingBox> boundingBoxes;
        private EcsFilter shapes;
        private EcsPool<Shape> shapesPool;

        public void Init(EcsSystems systems)
        {
            var world = systems.GetWorld();
            shapes = world.Filter<Shape>().Inc<BoundingBox>().End();
            shapesPool = world.GetPool<Shape>();
            boundingBoxes = world.GetPool<BoundingBox>();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var entity in shapes)
            {
                ref var shape = ref shapesPool.Get(entity);
                ref var boundingBox = ref boundingBoxes.Get(entity);
                RunTask(entity, shape, ref boundingBox);
            }
        }

        protected abstract void RunTask(int bodyEntity, in Shape shape, ref BoundingBox boundingBox);
    }
}