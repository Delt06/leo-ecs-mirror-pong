﻿using Composition;

namespace Mirror
{
    public class MirrorNetworkingSetUp : INetworkingSetUp
    {
        public bool IsServer => NetworkServer.active || NetworkClient.isHostClient;

        public bool IsClient => NetworkClient.active && NetworkClient.isConnected;
    }
}