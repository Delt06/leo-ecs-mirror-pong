using System.Numerics;
using System.Runtime.CompilerServices;
using Leopotam.EcsLite;
using Simulation.Physics.Components.Physics.Events;
using Simulation.Physics.Components.Physics.Tags;

namespace Simulation.Physics.Services
{
    public abstract class ContactSolverReference
    {
        public abstract void AddTask(int bodyA, int bodyB, int tagA, int tagB, float penetration, Vector2 normal,
            Vector2 point);
    }

    public class ContactSolverReference<TagA, TagB> : ContactSolverReference
        where TagA : struct, ITag
        where TagB : struct, ITag
    {
        private readonly EcsPool<Contact<TagA, TagB>> contactsPool;
        private readonly int expectedFirstTagTypeId;
        private readonly EcsWorld worldEvents;

        public ContactSolverReference(EcsWorld worldEvents)
        {
            this.worldEvents = worldEvents;
            contactsPool = worldEvents.GetPool<Contact<TagA, TagB>>();
            expectedFirstTagTypeId = default(TagA).TypeId();
        }

        public override void AddTask(int bodyA, int bodyB, int tagA, int tagB, float penetration, Vector2 normal,
            Vector2 point)
        {
            if (tagA == expectedFirstTagTypeId)
                AddTask(bodyA, bodyB, penetration, normal, point);
            else
                AddTask(bodyB, bodyA, penetration, -normal, point);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AddTask(int bodyA, int bodyB, float penetration, Vector2 normal, Vector2 point)
        {
            contactsPool.Add(worldEvents.NewEntity()) =
                new Contact<TagA, TagB>(bodyA, bodyB, penetration, normal, point);
        }
    }
}