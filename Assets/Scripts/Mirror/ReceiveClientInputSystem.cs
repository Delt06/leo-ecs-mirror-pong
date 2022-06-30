using System.Collections.Generic;
using DELTation.LeoEcsExtensions.Systems.Run;
using DELTation.LeoEcsExtensions.Utilities;
using Leopotam.EcsLite;
using Simulation;

namespace Mirror
{
    public class ReceiveClientInputSystem : EcsSystemBase, IEcsRunSystem, IEcsInitSystem, IEcsDestroySystem
    {
        private readonly Queue<ClientInput> _inputs = new();

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
                var input = _inputs.Dequeue();
                World.NewEntityWith<ClientInput>() = input;
            }
        }

        private void HandleIncomingMessage(NetworkConnectionToClient connection, ClientInputMessage message)
        {
            _inputs.Enqueue(message.ClientInput);
        }
    }
}