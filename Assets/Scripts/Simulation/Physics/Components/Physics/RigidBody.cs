namespace Simulation.Physics.Components.Physics
{
    public struct RigidBody
    {
        public float Restitution;
        public float Mass;
        public float InverseMass;
        public float InverseMMOI;

        public RigidBody(float mass, float mmoi, float restitution)
        {
            Mass = mass;
            InverseMass = 1 / mass;
            InverseMMOI = 1 / mmoi;
            Restitution = restitution;
        }
    }
}