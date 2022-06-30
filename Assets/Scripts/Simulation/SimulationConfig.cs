using UnityEngine;

namespace Simulation
{
    [CreateAssetMenu]
    public class SimulationConfig : ScriptableObject
    {
        [Min(0f)]
        public float DeltaTime = 0.05f;
    }
}