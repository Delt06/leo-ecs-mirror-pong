using Leopotam.EcsLite.Di;
using Simulation.Physics.Components.Physics;
using Simulation.Physics.Components.Physics.Shapes;

namespace Simulation.Physics.Systems.UpdateBV
{
    public class UpdateCirclesBV : UpdateBV<Circle>
    {
        private EcsPoolInject<Pose> poses;

        protected override void RunTask(int bodyEntity, in Circle shape, ref BoundingBox boundingBox)
        {
            ref var pose = ref poses.Value.Get(bodyEntity);
            boundingBox = BoundingBox.ConstructBoundingBox(pose.Position, shape.Radius);
        }
    }
}