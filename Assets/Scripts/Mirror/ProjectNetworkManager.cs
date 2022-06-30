using UnityEngine;

namespace Mirror
{
    public class ProjectNetworkManager : NetworkManager
    {
        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            base.OnServerAddPlayer(conn);
            Debug.Log("Welcome, " + conn.identity);
        }
    }
}