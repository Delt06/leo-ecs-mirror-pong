using System.Numerics;
using Leopotam.EcsLite;
using Simulation.Physics.Components.Physics;
using Simulation.Physics.Components.Physics.Constraints;
using Simulation.Physics.Components.Physics.Shapes;
using Simulation.Physics.Components.Physics.Tags;

namespace Simulation.Physics.Services
{
    public class PhysicsObjectsFactory
    {
        protected EcsPool<BoundingBox> bboxes;
        protected EcsPool<BodyMask> bodyMasks;
        protected EcsPool<Circle> circleShapes;
        protected EcsPool<Mesh> meshShapes;
        protected EcsPool<Pose> poses;
        // bodies
        protected EcsPool<RigidBody> rigidBodies;
        protected EcsPool<Segment> segmentShapes;
        // constraints
        protected EcsPool<SpringBodyBody> springsBodyBody;
        protected EcsPool<SpringBodySpace> springsBodySpace;
        protected EcsPool<StaticBody> staticBodies;
        protected EcsPool<Velocity> velocities;
        protected EcsWorld world;
        protected EcsWorld worldConstraints;

        public PhysicsObjectsFactory(EcsWorld world, EcsWorld worldConstraints)
        {
            this.world = world;
            this.worldConstraints = worldConstraints;
            rigidBodies = world.GetPool<RigidBody>();
            staticBodies = world.GetPool<StaticBody>();
            velocities = world.GetPool<Velocity>();
            poses = world.GetPool<Pose>();
            bodyMasks = world.GetPool<BodyMask>();
            circleShapes = world.GetPool<Circle>();
            segmentShapes = world.GetPool<Segment>();
            meshShapes = world.GetPool<Mesh>();
            bboxes = world.GetPool<BoundingBox>();
            springsBodyBody = worldConstraints.GetPool<SpringBodyBody>();
            springsBodySpace = worldConstraints.GetPool<SpringBodySpace>();
        }

        public int CreateCircle(Vector2 center, float radius, float mass, float restitution, int? entity = null)
        {
            var mmoi = Circle.CalculateMMOI(radius, mass);
            var e = entity ?? world.NewEntity();

            rigidBodies.Add(e) = new RigidBody(mass, mmoi, restitution);
            poses.Add(e) = new Pose(center);
            velocities.Add(e);
            bodyMasks.Add(e) = new BodyMask(Circle.Id, Dynamic.Id);
            circleShapes.Add(e).Radius = radius;
            bboxes.Add(e);

            return e;
        }

        public int CreateSegment(Vector2 a, Vector2 b, float restitution, int? entity = null)
        {
            var segment = new Segment(a, b);
            var e = entity ?? world.NewEntity();

            staticBodies.Add(e).Restitution = restitution;
            bodyMasks.Add(e) = new BodyMask(Segment.Id, Static.Id);
            segmentShapes.Add(e) = segment;
            bboxes.Add(e) = new BoundingBox { Min = Vector2.Min(a, b), Max = Vector2.Max(a, b) };

            return e;
        }

        public int CreateMesh(Mesh mesh, float mass, float restitution, Vector2 position, float rotation)
        {
            var mmoi = Mesh.CalculateMMOI(mesh, mass);

            var e = world.NewEntity();

            rigidBodies.Add(e) = new RigidBody(mass, mmoi, restitution);
            poses.Add(e) = new Pose(position + mesh.MassCenter, rotation);
            velocities.Add(e);
            bodyMasks.Add(e) = new BodyMask(Mesh.Id, Dynamic.Id);
            meshShapes.Add(e) = mesh;
            bboxes.Add(e);

            return e;
        }

        public int AttachSpring(int bodyA, int bodyB, Vector2 attachmentPointBodyA, Vector2 attachmentPointBodyB,
            float restLength, float elasticityCoefficient)
        {
            var e = worldConstraints.NewEntity();
            springsBodyBody.Add(e) = new SpringBodyBody(bodyA, bodyB, attachmentPointBodyA, attachmentPointBodyB,
                restLength, elasticityCoefficient
            );
            return e;
        }

        public int AttachSpring(int body, Vector2 attachmentPointBody, Vector2 attachmentPointSpace, float restLength,
            float elasticityCoefficient)
        {
            var e = worldConstraints.NewEntity();
            springsBodySpace.Add(e) = new SpringBodySpace(body, attachmentPointBody, attachmentPointSpace, restLength,
                elasticityCoefficient
            );
            return e;
        }
    }
}