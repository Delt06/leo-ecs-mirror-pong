using System.Numerics;
using System.Runtime.CompilerServices;
using Simulation.Physics.Components;
using Simulation.Physics.Components.Physics;

namespace Simulation.Physics.Services
{
    public static class SolversCommon
    {
        public static void ApplyImpulseBothBodies(
            Vector2 normal, Vector2 point, float restitution,
            in RigidBody rigidBodyA, in RigidBody rigidBodyB, in Pose poseA, in Pose poseB,
            ref Velocity velocityA, ref Velocity velocityB)
        {
            var pointVelocityA = Velocity.ComputePointVelocity(point, velocityA, poseA);
            var pointVelocityB = Velocity.ComputePointVelocity(point, velocityB, poseB);
            var separatingVelocity = Vector2.Dot(pointVelocityA - pointVelocityB, normal);
            if (separatingVelocity > 0) return;

            var desiredDeltaVelocity = -(1 + restitution) * separatingVelocity;

            float deltaVelocity = default;
            deltaVelocity += GetAngularComponent(point, normal, rigidBodyA, poseA);
            deltaVelocity += GetLinearComponent(rigidBodyA);
            deltaVelocity += GetAngularComponent(point, normal, rigidBodyB, poseB);
            deltaVelocity += GetLinearComponent(rigidBodyB);

            var impulseContactSpace = new Vector2(desiredDeltaVelocity / deltaVelocity, 0);
            var impulse = Vector2Ext.Rotate(impulseContactSpace, normal);

            velocityA.Linear += impulse * rigidBodyA.InverseMass;
            velocityA.Angular += Vector2Ext.Cross(point - poseA.Position, impulse) * rigidBodyA.InverseMMOI;

            velocityB.Linear -= impulse * rigidBodyB.InverseMass;
            velocityB.Angular -= Vector2Ext.Cross(point - poseB.Position, impulse) * rigidBodyB.InverseMMOI;
        }

        private static float GetAngularComponent(Vector2 contactPoint, Vector2 contactNormal, in RigidBody rigidBody,
            in Pose pose)
        {
            var relativeContactPosition = contactPoint - pose.Position;
            var torquePerUnitImpulse = Vector2Ext.Cross(relativeContactPosition, contactNormal);
            var rotationPerUnitImpulse = rigidBody.InverseMMOI * torquePerUnitImpulse;
            var velocityPerUnitImpulse = Vector2Ext.Cross(rotationPerUnitImpulse, relativeContactPosition);
            var velocityPerUnitImpulseContact = Vector2.Dot(velocityPerUnitImpulse, contactNormal);
            return velocityPerUnitImpulseContact;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float GetLinearComponent(in RigidBody rigidBody) => rigidBody.InverseMass;

        /// <summary>
        ///     Equivalent to version with both bodies, but body B's inverse mass and inverse mmoi equal 0
        /// </summary>
        public static void ApplyImpulseOneBody(
            Vector2 normal, Vector2 point, float restitution, in RigidBody rigidBody, in Pose pose,
            ref Velocity velocity)
        {
            var pointVelocity = Velocity.ComputePointVelocity(point, velocity, pose);
            var separatingVelocity = Vector2.Dot(pointVelocity, normal);
            if (separatingVelocity > 0) return;

            var desiredDeltaVelocity = -(1 + restitution) * separatingVelocity;

            float deltaVelocity = default;
            deltaVelocity += GetAngularComponent(point, normal, rigidBody, pose);
            deltaVelocity += GetLinearComponent(rigidBody);

            var impulseContactSpace = new Vector2(desiredDeltaVelocity / deltaVelocity, 0);
            var impulse = Vector2Ext.Rotate(impulseContactSpace, normal);

            velocity.Linear += impulse * rigidBody.InverseMass;
            velocity.Angular += Vector2Ext.Cross(point - pose.Position, impulse) * rigidBody.InverseMMOI;
        }

        /// <summary>
        ///     Doesn't change angular velocity
        /// </summary>
        public static void ApplyImpulseBothBodiesLinear(
            Vector2 normal, float restitution, in RigidBody rigidBodyA, in RigidBody rigidBodyB, ref Vector2 velocityA,
            ref Vector2 velocityB)
        {
            var separatingVelocity = Vector2.Dot(velocityA - velocityB, normal);
            if (separatingVelocity > 0) return;

            var impulse = -(1 + restitution) * separatingVelocity / (rigidBodyA.InverseMass + rigidBodyB.InverseMass);
            var impulsePerInverseMass = normal * impulse;

            velocityA += impulsePerInverseMass * rigidBodyA.InverseMass;
            velocityB -= impulsePerInverseMass * rigidBodyB.InverseMass;
        }

        /// <summary>
        ///     Equivalent to version with both bodies, but body B's inverse mass equals 0
        /// </summary>
        public static void ApplyImpulseOneBodyLinear(
            Vector2 normal, float restitution, bool bodyLocked, ref Vector2 velocity)
        {
            var separatingVelocity = Vector2.Dot(velocity, normal);
            if (separatingVelocity > 0) return;

            var impulseScalar = -(1 + restitution) * separatingVelocity;

            if (!bodyLocked) velocity += normal * impulseScalar;
        }

        public static void CorrectInterpenetrationBothBodies(
            Vector2 normal, Vector2 point, float penetration,
            in RigidBody rigidBodyA, in RigidBody rigidBodyB, ref Pose poseA, ref Pose poseB)
        {
            var angularComponentA = GetAngularComponent(point, normal, rigidBodyA, poseA);
            var linearComponentA = GetLinearComponent(rigidBodyA);
            var angularComponentB = GetAngularComponent(point, normal, rigidBodyB, poseB);
            var linearComponentB = GetLinearComponent(rigidBodyB);

            var totalInertia = angularComponentA + linearComponentA + angularComponentB + linearComponentB;
            var inverseTotalInertia = 1 / totalInertia;

            var rotationA = GetRotationPerUnitImpulse(point, normal, rigidBodyA, poseA);
            var rotationB = GetRotationPerUnitImpulse(point, normal, rigidBodyB, poseB);

            poseA.Position += normal * (penetration * linearComponentA * inverseTotalInertia);
            poseA.Rotation += penetration * rotationA * inverseTotalInertia;

            poseB.Position -= normal * (penetration * linearComponentB * inverseTotalInertia);
            poseB.Rotation -= penetration * rotationB * inverseTotalInertia;
        }

        private static float GetRotationPerUnitImpulse(Vector2 contactPoint, Vector2 contactNormal,
            in RigidBody rigidBody, in Pose pose)
        {
            var relativeContactPosition = contactPoint - pose.Position;
            var torquePerUnitImpulse = Vector2Ext.Cross(relativeContactPosition, contactNormal);
            var rotationPerUnitImpulse = rigidBody.InverseMMOI * torquePerUnitImpulse;
            return rotationPerUnitImpulse;
        }

        /// <summary>
        ///     Equivalent to version with both bodies, but body B's inverse mass equals 0
        /// </summary>
        public static void CorrectInterpenetrationOneBody(
            Vector2 normal, Vector2 point, float penetration, in RigidBody rigidBody, ref Pose pose)
        {
            var angularComponent = GetAngularComponent(point, normal, rigidBody, pose);
            var linearComponent = GetLinearComponent(rigidBody);

            var totalInertia = angularComponent + linearComponent;
            var inverseTotalInertia = 1 / totalInertia;

            var rotation = GetRotationPerUnitImpulse(point, normal, rigidBody, pose);

            pose.Position += normal * (penetration * linearComponent * inverseTotalInertia);
            pose.Rotation += penetration * rotation * inverseTotalInertia;
        }

        /// <summary>
        ///     Doesn't change angular velocity
        /// </summary>
        public static void CorrectInterpenetrationBothBodiesLinear(
            Vector2 normal, float penetration, in RigidBody rigidBodyA, in RigidBody rigidBodyB, ref Vector2 positionA,
            ref Vector2 positionB)
        {
            var correction = penetration / (rigidBodyA.InverseMass + rigidBodyB.InverseMass);
            var correctionPerInverseMass = normal * correction;

            positionA += correctionPerInverseMass * rigidBodyA.InverseMass;
            positionB -= correctionPerInverseMass * rigidBodyB.InverseMass;
        }

        /// <summary>
        ///     Equivalent to version with both bodies, but body B's inverse mass equals 0
        /// </summary>
        public static void CorrectInterpenetrationOneBodyLinear(
            Vector2 normal, float penetration, in RigidBody rigidBody, ref Vector2 position)
        {
            position += normal * penetration;
        }
    }
}