using Simulation.Ids;

namespace Simulation.Score
{
    public readonly struct EntityScore
    {
        public readonly SyncedEntityId EntityId;
        public readonly int Score;

        public EntityScore(SyncedEntityId entityId, int score)
        {
            EntityId = entityId;
            Score = score;
        }
    }
}