using Mirror;
using Simulation;
using Simulation.Paddles;
using UnityEngine;
using UnityEngine.Scripting;

namespace Networking
{
    public class ProjectNetworkManager : NetworkManager
    {
        private PaddleEntityFactory _paddleEntityFactory;

        [Preserve]
        public void Construct(PaddleEntityFactory paddleEntityFactory)
        {
            _paddleEntityFactory = paddleEntityFactory;
        }

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            base.OnServerAddPlayer(conn);
            Debug.Log("Welcome, " + conn.identity);
            _paddleEntityFactory.CreatePaddle(conn.identity.netId);
        }

        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            Debug.Log("Bye, " + conn.identity);
            _paddleEntityFactory.TryDestroyPaddle(conn.identity.netId);
            base.OnServerDisconnect(conn);
        }
    }
}