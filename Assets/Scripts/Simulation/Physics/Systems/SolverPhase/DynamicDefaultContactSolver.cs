using System;
using Leopotam.EcsLite.Di;
using Simulation.Physics.Components.Physics;
using Simulation.Physics.Components.Physics.Events;
using Simulation.Physics.Components.Physics.Tags;
using Simulation.Physics.Services;

namespace Simulation.Physics.Systems.SolverPhase
{
    /// <summary>
    ///     Assumes both bodies are dynamic
    /// </summary>
    public class DynamicDefaultContactSolver<TagA, TagB> : ContactSolver<TagA, TagB>
        where TagA : struct, ITag
        where TagB : struct, ITag
    {
        private EcsPoolInject<Pose> poses;
        private EcsPoolInject<RigidBody> rigidBodies;
        private EcsPoolInject<Velocity> velocities;

        protected override void SolveContact(in Contact<TagA, TagB> contact)
        {
            ref var rigidBodyA = ref rigidBodies.Value.Get(contact.BodyA);
            ref var rigidBodyB = ref rigidBodies.Value.Get(contact.BodyB);
            ref var velocityA = ref velocities.Value.Get(contact.BodyA);
            ref var velocityB = ref velocities.Value.Get(contact.BodyB);
            ref var poseA = ref poses.Value.Get(contact.BodyA);
            ref var poseB = ref poses.Value.Get(contact.BodyB);

            var restitution = Math.Min(rigidBodyA.Restitution, rigidBodyB.Restitution);

            SolversCommon.ApplyImpulseBothBodies(contact.Normal, contact.Point, restitution, rigidBodyA, rigidBodyB,
                poseA, poseB, ref velocityA, ref velocityB
            );
            SolversCommon.CorrectInterpenetrationBothBodies(contact.Normal, contact.Point, contact.Penetration,
                rigidBodyA, rigidBodyB, ref poseA, ref poseB
            );
        }
    }
}