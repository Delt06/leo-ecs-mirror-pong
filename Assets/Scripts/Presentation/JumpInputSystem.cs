using DELTation.LeoEcsExtensions.Systems.Run;
using DELTation.LeoEcsExtensions.Utilities;
using Leopotam.EcsLite;
using Simulation;
using UnityEngine;

namespace Presentation
{
    public class JumpInputSystem : EcsSystemBase, IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            if (Input.GetButtonDown("Jump"))
                World.NewEntityWith<ClientInput>().Jump = true;
        }
    }
}