using System.Collections.Generic;
using DELTation.LeoEcsExtensions.Systems.Run;
using DELTation.LeoEcsExtensions.Utilities;
using Leopotam.EcsLite;
using Mirror;
using Simulation;

namespace Networking
{
    public class ReceiveServerStateSystem : EcsSystemBase, IEcsRunSystem, IEcsInitSystem, IEcsDestroySystem
    {
        private readonly Stack<SimulationState> _states = new();

        public void Destroy(EcsSystems systems)
        {
            NetworkClient.UnregisterHandler<ServerStateMessage>();
        }

        public void Init(EcsSystems systems)
        {
            NetworkClient.RegisterHandler<ServerStateMessage>(HandleIncomingMessage, false);
        }

        public void Run(EcsSystems systems)
        {
            if (!_states.TryPeek(out var state)) return;
            World.NewEntityWith<SimulationState>() = state;
            _states.Clear();
        }

        private void HandleIncomingMessage(ServerStateMessage message)
        {
            _states.Push(message.SimulationState);
        }
    }
}