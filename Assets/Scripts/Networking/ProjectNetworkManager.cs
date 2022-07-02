using Mirror;
using Simulation.Ball;
using Simulation.Paddles;
using UnityEngine;
using UnityEngine.Scripting;

namespace Networking
{
    public class ProjectNetworkManager : NetworkManager
    {
        private BallEntityFactory _ballEntityFactory;
        private PaddleEntityFactory _paddleEntityFactory;

        [Preserve]
        public void Construct(PaddleEntityFactory paddleEntityFactory, BallEntityFactory ballEntityFactory)
        {
            _paddleEntityFactory = paddleEntityFactory;
            _ballEntityFactory = ballEntityFactory;
        }

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            base.OnServerAddPlayer(conn);
            Debug.Log("Welcome, " + conn.identity);
            _paddleEntityFactory.CreatePaddle(conn.identity.netId);
            if (_paddleEntityFactory.PaddlesCount == 2)
                _ballEntityFactory.CreateBall();
        }

        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            Debug.Log("Bye, " + conn.identity);
            _paddleEntityFactory.TryDestroyPaddle(conn.identity.netId);

            if (_paddleEntityFactory.PaddlesCount < 2)
                _ballEntityFactory.TryDestroyBall();

            base.OnServerDisconnect(conn);
        }
    }
}