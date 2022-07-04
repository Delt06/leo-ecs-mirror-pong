using System.Diagnostics;
using JetBrains.Annotations;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Simulation.Physics.Components.Physics.Events;
using Simulation.Physics.Components.Physics.Tags;
using Simulation.Physics.Services;

namespace Simulation.Physics.Systems.SolverPhase
{
    public abstract class ContactSolver<TagA, TagB> : IEcsInitSystem, IEcsRunSystem
        where TagA : struct, ITag
        where TagB : struct, ITag
    {
        [UsedImplicitly]
        protected EcsCustomInject<PhysicsData> _physicsData;
        private EcsFilter contacts;
        private EcsPool<Contact<TagA, TagB>> contactsPool;

        public void Init(EcsSystems systems)
        {
            var worldEvents = systems.GetWorld(WorldNames.Events);
            contacts = worldEvents.Filter<Contact<TagA, TagB>>().End();
            contactsPool = worldEvents.GetPool<Contact<TagA, TagB>>();
            RegisterSolver(worldEvents, _physicsData.Value.ContactSolversMatrix);
        }

        public void Run(EcsSystems systems)
        {
            foreach (var evt in contacts)
            {
                SolveContact(contactsPool.Get(evt));
                contactsPool.Del(evt);
            }
        }

        private static void RegisterSolver(EcsWorld worldEvents, ContactSolverReference[][] solversMatrix)
        {
            var a = default(TagA).TypeId();
            var b = default(TagB).TypeId();
#if DEBUG
            var pair = $"{nameof(TagA)}, {nameof(TagB)}";
            Debug.Assert(a > -1 && a < solversMatrix.Length,
                $"Failed to register solver for pair <{pair}>, {nameof(TagA)}.Id = {a}, which is not in [0, {solversMatrix.Length - 1}] value interval"
            );
            Debug.Assert(b > -1 && b < solversMatrix.Length,
                $"Failed to register solver for pair <{pair}>, {nameof(TagB)}.Id = {b}, which is not in [0, {solversMatrix.Length - 1}] value interval"
            );
            Debug.Assert(solversMatrix[a][b] == null && solversMatrix[b][a] == null,
                $"Failed to register solver for pair <{pair}>, pair has already been registered"
            );
#endif
            var solverReference = new ContactSolverReference<TagA, TagB>(worldEvents);
            solversMatrix[a][b] = solverReference;
            solversMatrix[b][a] = solverReference;
        }

        protected abstract void SolveContact(in Contact<TagA, TagB> contact);
    }
}