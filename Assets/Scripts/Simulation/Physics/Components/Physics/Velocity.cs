using System.Numerics;

namespace Simulation.Physics.Components.Physics
{
    public struct Velocity
    {
        public Vector2 Linear;
        public Vector2 Force;

        public float Angular;
        public float Torque;

        public static void Clear(ref Velocity velocity)
        {
            velocity.Force = Vector2.Zero;
            velocity.Torque = 0;
        }

        /// <summary>
        ///     point is defined in world space
        /// </summary>
        public static Vector2 ComputePointVelocity(Vector2 point, in Velocity velocity, in Pose pose)
        {
            var pointVelocityAngular = Vector2Ext.LeftPerp(point - pose.Position) * velocity.Angular;
            return pointVelocityAngular + velocity.Linear;
        }

        /// <summary>
        ///     Applies force to the application point, force direction and application point are defined in world space
        /// </summary>
        public static void ApplyForce(ref Velocity velocity, in Pose pose, Vector2 force, Vector2 applicationPoint)
        {
            velocity.Torque += Vector2Ext.Cross(applicationPoint - pose.Position, force);
            velocity.Force += force;
        }

        /// <summary>
        ///     Applies force to the center of mass, force direction is defined in world space
        /// </summary>
        public static void ApplyForceLinear(ref Velocity velocity, Vector2 force)
        {
            velocity.Force += force;
        }

        /// <summary>
        ///     Applies force to the application point, force direction and application point are defined in world space
        /// </summary>
        public static void ApplyForceRotational(ref Velocity velocity, in Pose pose, Vector2 force,
            Vector2 applicationPoint)
        {
            velocity.Torque += Vector2Ext.Cross(applicationPoint - pose.Position, force);
        }
    }
}