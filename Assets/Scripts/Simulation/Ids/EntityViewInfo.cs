namespace Simulation.Ids
{
    public readonly struct EntityViewInfo
    {
        public readonly SyncedEntityId Id;
        public readonly ViewInfo ViewInfo;

        public EntityViewInfo(SyncedEntityId id, ViewInfo viewInfo)
        {
            Id = id;
            ViewInfo = viewInfo;
        }
    }
}