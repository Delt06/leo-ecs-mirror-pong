using System.Numerics;

namespace Simulation.Physics.Components.Physics.Constraints
{
    public struct SpringBodyBody
    {
        public int BodyA;
        public int BodyB;
        public Vector2 AttachmentPointBodyA;
        public Vector2 AttachmentPointBodyB;
        public float RestLength;
        public float ElasticityCoefficient;

        /// <summary>
        ///     We assume bodyA is attached to bodyB (the only difference is how to draw string compression/extension)
        /// </summary>
        /// <param name="bodyA"></param>
        /// <param name="bodyB"></param>
        /// <param name="attachmentPointBodyA">attachment point on body A, defined in body A's local space</param>
        /// <param name="attachmentPointBodyB">attachment point on body B, defined in body B's local space</param>
        /// <param name="restLength"></param>
        /// <param name="elasticityCoefficient"></param>
        public SpringBodyBody(int bodyA, int bodyB, Vector2 attachmentPointBodyA, Vector2 attachmentPointBodyB,
            float restLength, float elasticityCoefficient)
        {
            BodyA = bodyA;
            BodyB = bodyB;
            AttachmentPointBodyA = attachmentPointBodyA;
            AttachmentPointBodyB = attachmentPointBodyB;
            RestLength = restLength;
            ElasticityCoefficient = elasticityCoefficient;
        }
    }
}