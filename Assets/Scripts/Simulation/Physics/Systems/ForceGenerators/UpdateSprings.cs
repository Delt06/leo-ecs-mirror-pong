using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Simulation.Physics.Components;
using Simulation.Physics.Components.Physics;
using Simulation.Physics.Components.Physics.Constraints;

namespace Simulation.Physics.Systems.ForceGenerators
{
    public class UpdateSprings : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsPoolInject<SpringBodyBody> springsBodyBodyPool = WorldNames.Constraints;
        private readonly EcsPoolInject<SpringBodySpace> springsBodySpacePool = WorldNames.Constraints;
        private EcsPoolInject<Pose> poses;
        private EcsPoolInject<RigidBody> rigidBodies;
        private EcsFilter springsBodyBody;
        private EcsFilter springsBodySpace;
        private EcsPoolInject<Velocity> velocities;

        public void Init(EcsSystems systems)
        {
            var worldConstraints = systems.GetWorld(WorldNames.Constraints);
            springsBodyBody = worldConstraints.Filter<SpringBodyBody>().End();
            springsBodySpace = worldConstraints.Filter<SpringBodySpace>().End();
        }

        public void Run(EcsSystems systems)
        {
            // possible optimization:
            // if spring.RestLength == 0, k = spring.ElasticityCoefficient
            // no need to calc diff.Length
            foreach (var entity in springsBodyBody)
            {
                ref var spring = ref springsBodyBodyPool.Value.Get(entity);
                ref var poseA = ref poses.Value.Get(spring.BodyA);
                ref var poseB = ref poses.Value.Get(spring.BodyB);

                var attachmentPointBodyA = Pose.Transform(spring.AttachmentPointBodyA, poseA);
                var attachmentPointBodyB = Pose.Transform(spring.AttachmentPointBodyB, poseB);
                var diff = attachmentPointBodyB - attachmentPointBodyA;

                if (Vector2Ext.IsZero(diff)) continue;

                var k = spring.ElasticityCoefficient * (1 - spring.RestLength / diff.Length());
                var springForce = diff * k;

                Velocity.ApplyForce(ref velocities.Value.Get(spring.BodyA), poseA, springForce,
                    attachmentPointBodyA
                );
                Velocity.ApplyForce(ref velocities.Value.Get(spring.BodyB), poseB, -springForce,
                    attachmentPointBodyB
                );
            }

            foreach (var entity in springsBodySpace)
            {
                ref var spring = ref springsBodySpacePool.Value.Get(entity);
                ref var pose = ref poses.Value.Get(spring.Body);

                var attachmentPointBody = Pose.Transform(spring.AttachmentPointBody, pose);
                var diff = spring.AttachmentPointSpace - attachmentPointBody;

                if (Vector2Ext.IsZero(diff)) continue;

                var k = spring.ElasticityCoefficient * (1 - spring.RestLength / diff.Length());
                var springForce = diff * k;

                Velocity.ApplyForce(ref velocities.Value.Get(spring.Body), pose, springForce,
                    attachmentPointBody
                );
            }
        }
    }
}