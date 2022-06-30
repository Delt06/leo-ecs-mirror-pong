using DELTation.LeoEcsExtensions.Systems.Run;
using DELTation.LeoEcsExtensions.Utilities;
using Leopotam.EcsLite;
using Simulation;
using UnityEngine;

namespace Presentation
{
    public class InputSystem : EcsSystemBase, IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var movement = Input.GetAxisRaw("Vertical");
            if (!Mathf.Approximately(movement, 0))
                World.NewEntityWith<ClientInput>().Motion = movement * Time.unscaledDeltaTime;
        }
    }
}