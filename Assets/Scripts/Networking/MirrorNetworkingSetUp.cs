using Composition;
using Mirror;

namespace Networking
{
    public class MirrorNetworkingSetUp : INetworkingSetUp
    {
        public bool IsServer => NetworkServer.active || NetworkClient.isHostClient;

        public bool IsClient => NetworkClient.active && NetworkClient.isConnected;
    }
}