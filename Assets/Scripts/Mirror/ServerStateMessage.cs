using Simulation;

namespace Mirror
{
    public struct ServerStateMessage : NetworkMessage
    {
        public SimulationState SimulationState;
    }
}