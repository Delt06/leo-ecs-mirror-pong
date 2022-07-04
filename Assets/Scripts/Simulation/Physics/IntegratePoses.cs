using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Simulation.Physics.Components.Physics;
using Simulation.Physics.Services;

namespace Simulation.Physics
{
    public class IntegratePoses : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter bodies;
        private EcsCustomInject<PhysicsData> physicsData;
        private EcsPoolInject<Pose> poses;
        private EcsPoolInject<RigidBody> rigidBodies;
        private EcsPoolInject<Velocity> velocities;

        public void Init(EcsSystems systems)
        {
            var world = systems.GetWorld();
            bodies = world.Filter<RigidBody>().Inc<Pose>().Inc<Velocity>().End();
        }

        public void Run(EcsSystems systems)
        {
            var dt = physicsData.Value.GetDt();

            foreach (var entity in bodies)
            {
                ref var pose = ref poses.Value.Get(entity);
                ref var rigidBody = ref rigidBodies.Value.Get(entity);
                ref var velocity = ref velocities.Value.Get(entity);

                var linearAcceleration = velocity.Force * rigidBody.InverseMass;
                var angularAcceleration = velocity.Torque * rigidBody.InverseMMOI;

                pose.Position += velocity.Linear * dt + linearAcceleration * dt * dt * 0.5f;
                pose.Rotation += velocity.Angular * dt + angularAcceleration * dt * dt * 0.5f;

                velocity.Linear += linearAcceleration * dt;
                velocity.Angular += angularAcceleration * dt;

                pose.RotationVector = Pose.GetRotationVector(pose.Rotation);

                Velocity.Clear(ref velocity);
            }
        }
    }
}