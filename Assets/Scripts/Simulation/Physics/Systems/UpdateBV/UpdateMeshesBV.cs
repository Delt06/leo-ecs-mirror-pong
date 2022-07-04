using Leopotam.EcsLite.Di;
using Simulation.Physics.Components.Physics;
using Simulation.Physics.Components.Physics.Shapes;

namespace Simulation.Physics.Systems.UpdateBV
{
    public class UpdateMeshesBV : UpdateBV<Mesh>
    {
        private EcsPoolInject<Pose> poses;

        protected override void RunTask(int bodyEntity, in Mesh shape, ref BoundingBox boundingBox)
        {
            ref var pose = ref poses.Value.Get(bodyEntity);
            boundingBox = Mesh.CalculateBoundingBox(shape.Triangles, pose);
        }
    }
}