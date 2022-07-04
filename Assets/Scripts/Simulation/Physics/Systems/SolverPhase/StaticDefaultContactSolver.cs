using System;
using Leopotam.EcsLite.Di;
using Simulation.Physics.Components.Physics;
using Simulation.Physics.Components.Physics.Events;
using Simulation.Physics.Components.Physics.Tags;
using Simulation.Physics.Services;

namespace Simulation.Physics.Systems.SolverPhase
{
    /// <summary>
    ///     Assumes dynamic body has TagA, static body has TagB
    /// </summary>
    public class StaticDefaultContactSolver<TagA, TagB> : ContactSolver<TagA, TagB>
        where TagA : struct, ITag
        where TagB : struct, ITag
    {
        private EcsPoolInject<Pose> poses;
        private EcsPoolInject<RigidBody> rigidBodies;
        private EcsPoolInject<StaticBody> staticBodies;
        private EcsPoolInject<Velocity> velocities;

        protected override void SolveContact(in Contact<TagA, TagB> contact)
        {
            ref var dynamicBody = ref rigidBodies.Value.Get(contact.BodyA);
            ref var dynamicBodyVelocity = ref velocities.Value.Get(contact.BodyA);
            ref var dynamicBodyPose = ref poses.Value.Get(contact.BodyA);
            ref var staticBody = ref staticBodies.Value.Get(contact.BodyB);

            var restitution = Math.Min(dynamicBody.Restitution, staticBody.Restitution);

            SolversCommon.ApplyImpulseOneBody(contact.Normal, contact.Point, restitution, dynamicBody, dynamicBodyPose,
                ref dynamicBodyVelocity
            );
            SolversCommon.CorrectInterpenetrationOneBody(contact.Normal, contact.Point, contact.Penetration,
                dynamicBody, ref dynamicBodyPose
            );
        }
    }
}