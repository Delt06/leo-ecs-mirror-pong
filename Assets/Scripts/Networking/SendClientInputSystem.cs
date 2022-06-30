using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Mirror;
using Presentation;
using Simulation;
using static Networking.ClientInputUtils;

namespace Networking
{
    public class SendClientInputSystem : IEcsRunSystem, IEcsInitSystem
    {
        private readonly EcsFilterInject<Inc<ClientInput>> _filter = default;
        private PresentationSharedData _presentationData;

        public void Init(EcsSystems systems)
        {
            _presentationData = systems.GetPresentationData();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var i in _filter)
            {
                ref readonly var clientInput = ref _filter.Pools.Inc1.Get(i);

                if (NetworkClient.isHostClient)
                    CreateClientInputEntity(_presentationData.SimulationWorld,
                        NetworkClient.connection, clientInput
                    );
                else
                    NetworkClient.Send(new ClientInputMessage
                        {
                            ClientInput = clientInput,
                        }
                    );
            }
        }
    }
}