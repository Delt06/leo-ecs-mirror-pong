using System.Runtime.CompilerServices;
using Leopotam.EcsLite;
using Simulation.Physics.Components.Physics;
using Simulation.Physics.Components.Physics.Events;
using Simulation.Physics.Components.Physics.Shapes;

namespace Simulation.Physics.Services
{
    public abstract class ContactTesterReference
    {
        public abstract void AddTask(int bodyA, int bodyB, BodyMask maskA, BodyMask maskB);
    }

    public class ContactTesterReference<ShapeA, ShapeB> : ContactTesterReference
        where ShapeA : struct, IShape
        where ShapeB : struct, IShape
    {
        private readonly int expectedFirstShapeTypeId;
        private readonly EcsPool<PossibleContact<ShapeA, ShapeB>> possibleContactsPool;
        private readonly EcsWorld worldEvents;

        public ContactTesterReference(EcsWorld worldEvents)
        {
            this.worldEvents = worldEvents;
            possibleContactsPool = worldEvents.GetPool<PossibleContact<ShapeA, ShapeB>>();
            expectedFirstShapeTypeId = default(ShapeA).TypeId();
        }

        public override void AddTask(int bodyA, int bodyB, BodyMask maskA, BodyMask maskB)
        {
            if (maskA.ShapeTypeId == expectedFirstShapeTypeId)
                AddTask(bodyA, bodyB, maskA.TagTypeId, maskB.TagTypeId);
            else
                AddTask(bodyB, bodyA, maskB.TagTypeId, maskA.TagTypeId);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AddTask(int bodyA, int bodyB, int tagA, int tagB)
        {
            possibleContactsPool.Add(worldEvents.NewEntity()) = new PossibleContact<ShapeA, ShapeB>
                { BodyA = bodyA, BodyB = bodyB, TagA = tagA, TagB = tagB };
        }
    }
}