using Mirror;
using Simulation;

namespace Networking
{
    public struct ServerStateMessage : NetworkMessage
    {
        public SimulationState SimulationState;
    }
}