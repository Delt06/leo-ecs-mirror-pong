using System.Numerics;

namespace Simulation.Physics.Components.Physics.Constraints
{
    public struct SpringBodySpace
    {
        public int Body;
        public Vector2 AttachmentPointBody;
        public Vector2 AttachmentPointSpace;
        public float RestLength;
        public float ElasticityCoefficient;

        /// <summary>
        ///     We assume body is attached to point (the only difference is how to draw string compression/extension)
        /// </summary>
        /// <param name="body"></param>
        /// <param name="attachmentPointBody">attachment point on body, defined in body's local space</param>
        /// <param name="attachmentPointSpace">attachment point on space, defined in world space</param>
        /// <param name="restLength"></param>
        /// <param name="elasticityCoefficient"></param>
        public SpringBodySpace(int body, Vector2 attachmentPointBody, Vector2 attachmentPointSpace, float restLength,
            float elasticityCoefficient)
        {
            Body = body;
            AttachmentPointBody = attachmentPointBody;
            AttachmentPointSpace = attachmentPointSpace;
            RestLength = restLength;
            ElasticityCoefficient = elasticityCoefficient;
        }
    }
}