using Leopotam.EcsLite;
using Mirror;
using Simulation;

namespace Networking
{
    public static class ClientInputUtils
    {
        public static void TryCreateClientInputEntity(EcsWorld world, NetworkConnection connection,
            ClientInput input)
        {
            if (connection == null || connection.identity == null) return;

            var entity = world.NewEntity();
            world.GetPool<ClientInput>().Add(entity) = input;
            world.GetPool<InputClientId>().Add(entity).Id = connection.identity.netId;
        }
    }
}