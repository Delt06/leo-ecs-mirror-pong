using System.Numerics;
using Simulation.Physics.Components.Physics.Tags;

namespace Simulation.Physics.Components.Physics.Events
{
    public struct Contact<TagA, TagB>
        where TagA : struct, ITag
        where TagB : struct, ITag
    {
        public int BodyA;
        public int BodyB;
        public float Penetration;
        public Vector2 Normal; // from bodyB to bodyA; defined in world space
        public Vector2 Point; // midpoint of penetration; defined in world space

        public Contact(int bodyA, int bodyB, float penetration, Vector2 normal, Vector2 point)
        {
            BodyA = bodyA;
            BodyB = bodyB;
            Penetration = penetration;
            Normal = normal;
            Point = point;
        }
    }
}