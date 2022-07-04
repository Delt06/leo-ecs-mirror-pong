using Leopotam.EcsLite;

namespace Simulation.Ids
{
    public static class EntityIdExtensions
    {
        public static void AddEntityId(this EcsWorld world, int entity)
        {
            var pool = world.GetPool<SyncedEntityId>();
            ref var entityId = ref pool.Add(entity);
            entityId.Id = entity;
            entityId.Gen = world.GetEntityGen(entity);
        }
    }
}