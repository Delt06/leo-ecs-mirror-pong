using System;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Simulation.Physics.Components.Physics;
using Simulation.Physics.Services;

namespace Simulation.Physics.Systems.BroadPhase
{
    public class BroadPhase : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter bodies;
        private EcsPoolInject<BodyMask> bodyMasks;
        private EcsPoolInject<BoundingBox> boundingBoxes;

        private float[] filterSortPool = new float[128];
        private EcsCustomInject<PhysicsData> physicsData;

        public void Init(EcsSystems systems)
        {
            var world = systems.GetWorld();
            bodies = world.Filter<BodyMask>().Inc<BoundingBox>().End();
        }

        public void Run(EcsSystems systems)
        {
            var testersMatrix = physicsData.Value.ContactTestersMatrix;
            var solversMatrix = physicsData.Value.ContactSolversMatrix;

            SortBoundingBoxes();

            var bodiesEntities = bodies.GetRawEntities();
            var bodiesCount = bodies.GetEntitiesCount();

            for (var i = 0; i < bodiesCount; ++i)
            {
                var bodyA = bodiesEntities[i];
                ref var bboxA = ref boundingBoxes.Value.Get(bodyA);

                for (var k = i + 1; k < bodiesCount; ++k)
                {
                    var bodyB = bodiesEntities[k];
                    ref var bboxB = ref boundingBoxes.Value.Get(bodyB);

                    // this bboxB and every next one cannot intersect with bboxA
                    if (bboxB.Min.X > bboxA.Max.X) break;

                    // we already know that bboxes intersect on X axis, so test only Y axis
                    if (bboxA.Max.Y >= bboxB.Min.Y && bboxB.Max.Y >= bboxA.Min.Y)
                    {
                        var maskA = bodyMasks.Value.Get(bodyA);
                        var maskB = bodyMasks.Value.Get(bodyB);
                        var testerReference = testersMatrix[maskA.ShapeTypeId][maskB.ShapeTypeId];
                        var solverReference = solversMatrix[maskA.TagTypeId][maskB.TagTypeId];

                        // some shape pairs cannot be tested, some tag pairs cannot be solved
                        if (testerReference != null && solverReference != null)
                            testerReference.AddTask(bodyA, bodyB, maskA, maskB);
                    }
                }
            }
        }

        /// <summary>
        ///     sorts bounding boxes using bbox.Min.X as a rule
        /// </summary>
        private void SortBoundingBoxes()
        {
            var count = bodies.GetEntitiesCount();
            if (count > 1)
            {
                var entities = bodies.GetRawEntities();
                if (filterSortPool.Length < entities.Length) Array.Resize(ref filterSortPool, entities.Length);
                for (var i = 0; i < count; i++)
                {
                    filterSortPool[i] = boundingBoxes.Value.Get(entities[i]).Min.X;
                }

                Array.Sort(filterSortPool, entities, 0, count);
                var sparseIndex = bodies.GetSparseIndex();
                for (var i = 0; i < count; i++)
                {
                    sparseIndex[entities[i]] = i + 1;
                }
            }
        }
    }
}