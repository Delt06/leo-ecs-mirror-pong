using UnityEngine;

namespace Simulation.Ids
{
    public readonly struct EntityPosition
    {
        public readonly SyncedEntityId Id;
        public readonly Vector2 Position;

        public EntityPosition(SyncedEntityId id, Vector2 position)
        {
            Id = id;
            Position = position;
        }
    }
}