using System.Diagnostics;
using System.Numerics;
using JetBrains.Annotations;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Simulation.Physics.Components.Physics.Events;
using Simulation.Physics.Components.Physics.Shapes;
using Simulation.Physics.Services;

namespace Simulation.Physics.Systems.NarrowPhase
{
    public abstract class ContactTester<ShapeA, ShapeB> : IEcsInitSystem, IEcsRunSystem
        where ShapeA : struct, IShape
        where ShapeB : struct, IShape
    {
        [UsedImplicitly]
        protected EcsCustomInject<PhysicsData> _physicsData;
        private ContactSolverReference[][] contactSolversMatrix;
        private EcsFilter possibleContacts;
        private EcsPool<PossibleContact<ShapeA, ShapeB>> possibleContactsPool;

        public void Init(EcsSystems systems)
        {
            var worldEvents = systems.GetWorld(WorldNames.Events);
            possibleContacts = worldEvents.Filter<PossibleContact<ShapeA, ShapeB>>().End();
            possibleContactsPool = worldEvents.GetPool<PossibleContact<ShapeA, ShapeB>>();
            contactSolversMatrix = _physicsData.Value.ContactSolversMatrix;
            RegisterTester(worldEvents, _physicsData.Value.ContactTestersMatrix);
        }

        public void Run(EcsSystems systems)
        {
            foreach (var evt in possibleContacts)
            {
                TestPair(possibleContactsPool.Get(evt));
                possibleContactsPool.Del(evt);
            }
        }

        private static void RegisterTester(EcsWorld worldEvents, ContactTesterReference[][] testersMatrix)
        {
            var a = default(ShapeA).TypeId();
            var b = default(ShapeB).TypeId();
#if DEBUG
            var pair = $"{nameof(ShapeA)}, {nameof(ShapeB)}";
            Debug.Assert(a > -1 && a < testersMatrix.Length,
                $"Failed to register tester for pair <{pair}>, {nameof(ShapeA)}.Id = {a}, which is not in [0, {testersMatrix.Length - 1}] value interval"
            );
            Debug.Assert(b > -1 && b < testersMatrix.Length,
                $"Failed to register tester for pair <{pair}>, {nameof(ShapeB)}.Id = {b}, which is not in [0, {testersMatrix.Length - 1}] value interval"
            );
            Debug.Assert(testersMatrix[a][b] == null && testersMatrix[b][a] == null,
                $"Failed to register tester for pair <{pair}>, pair has already been registered"
            );
#endif
            var testerReference = new ContactTesterReference<ShapeA, ShapeB>(worldEvents);
            testersMatrix[a][b] = testerReference;
            testersMatrix[b][a] = testerReference;
        }

        protected abstract void TestPair(in PossibleContact<ShapeA, ShapeB> possibleContact);

        protected void AddSolverTask(in PossibleContact<ShapeA, ShapeB> possibleContact, float penetration,
            Vector2 normal, Vector2 point)
        {
            var tagA = possibleContact.TagA;
            var tagB = possibleContact.TagB;
            var solverReference = contactSolversMatrix[tagA][tagB];
            solverReference.AddTask(possibleContact.BodyA, possibleContact.BodyB, tagA, tagB, penetration, normal, point
            );
        }
    }
}