using System;
using System.Collections.Generic;

namespace Simulation.Ids
{
    public struct SyncedEntityId : IEqualityComparer<SyncedEntityId>
    {
        public int Id;
        public short Gen;

        public bool Equals(SyncedEntityId x, SyncedEntityId y) => x.Id == y.Id && x.Gen == y.Gen;

        public int GetHashCode(SyncedEntityId obj) => HashCode.Combine(obj.Id, obj.Gen);

        public override string ToString() => $"SyncedEntityId[{Id}:{Gen}]";
    }
}