using System.Collections.Generic;
using DELTation.LeoEcsExtensions.Systems.Run;
using Leopotam.EcsLite;
using Mirror;
using Simulation;
using static Networking.ClientInputUtils;

namespace Networking
{
    public class ReceiveClientInputSystem : EcsSystemBase, IEcsRunSystem, IEcsInitSystem, IEcsDestroySystem
    {
        private readonly Queue<(NetworkConnectionToClient connection, ClientInput input)> _inputs = new();

        public void Destroy(EcsSystems systems)
        {
            NetworkServer.UnregisterHandler<ClientInputMessage>();
        }

        public void Init(EcsSystems systems)
        {
            NetworkServer.RegisterHandler<ClientInputMessage>(HandleIncomingMessage);
        }

        public void Run(EcsSystems systems)
        {
            while (_inputs.Count > 0)
            {
                var (connection, input) = _inputs.Dequeue();
                CreateClientInputEntity(World, connection, input);
            }
        }

        private void HandleIncomingMessage(NetworkConnectionToClient connection, ClientInputMessage message)
        {
            _inputs.Enqueue((connection, message.ClientInput));
        }
    }
}