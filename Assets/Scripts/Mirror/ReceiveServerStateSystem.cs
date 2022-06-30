using System.Collections.Generic;
using DELTation.LeoEcsExtensions.Systems.Run;
using DELTation.LeoEcsExtensions.Utilities;
using Leopotam.EcsLite;
using Simulation;

namespace Mirror
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
            NetworkClient.RegisterHandler<ServerStateMessage>(HandleIncomingState, false);
        }

        public void Run(EcsSystems systems)
        {
            if (!_states.TryPeek(out var state)) return;
            World.NewEntityWith<SimulationState>() = state;
            _states.Clear();
        }

        private void HandleIncomingState(ServerStateMessage obj)
        {
            _states.Push(obj.SimulationState);
            World.NewEntityWith<SimulationState>() = obj.SimulationState;
        }
    }
}