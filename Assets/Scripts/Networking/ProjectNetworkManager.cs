using Mirror;
using UnityEngine;
using UnityEngine.Scripting;

namespace Networking
{
    public class ProjectNetworkManager : NetworkManager
    {
        private PaddleFactory _paddleFactory;

        [Preserve]
        public void Construct(PaddleFactory paddleFactory)
        {
            _paddleFactory = paddleFactory;
        }

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            base.OnServerAddPlayer(conn);
            Debug.Log("Welcome, " + conn.identity);
            _paddleFactory.CreatePaddle(conn.identity.netId);
        }

        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            Debug.Log("Bye, " + conn.identity);
            _paddleFactory.TryDestroyPaddle(conn.identity.netId);
            base.OnServerDisconnect(conn);
        }
    }
}