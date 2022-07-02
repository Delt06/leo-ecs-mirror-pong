using System.Collections.Generic;
using Leopotam.EcsLite;
using Simulation.Ball;
using Simulation.Paddles;

namespace Simulation
{
    public struct SimulationState : IEcsAutoReset<SimulationState>
    {
        public List<PaddleInfo> Paddles;
        public List<uint> DestroyedPaddles;
        public BallInfo BallInfo;

        public void AutoReset(ref SimulationState c)
        {
            c.Paddles ??= new List<PaddleInfo>();
            c.Paddles.Clear();

            c.DestroyedPaddles ??= new List<uint>();
            c.DestroyedPaddles.Clear();

            c.BallInfo = default;
        }
    }
}