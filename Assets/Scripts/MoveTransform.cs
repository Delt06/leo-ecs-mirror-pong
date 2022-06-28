using Mirror;
using UnityEngine;

public class MoveTransform : MonoBehaviour
{
    private GameObject _o;

    private void Start()
    {
        _o = GameObject.CreatePrimitive(PrimitiveType.Cube);
        NetworkClient.RegisterHandler<MoveTransformCommand>(OnMoved, false);
        NetworkServer.RegisterHandler<JumpCommand>(OnJumped);
    }

    private void Update()
    {
        if (NetworkClient.active && NetworkClient.isConnected)
            OnClient();

        if (NetworkServer.active || NetworkClient.isHostClient)
            OnServer();
    }

    private void OnClient()
    {
        if (Input.GetButtonDown("Jump"))
        {
            JumpLocally();
            NetworkClient.Send(new JumpCommand());
        }
    }

    private static void OnServer()
    {
        var position = new Vector3(0, Mathf.Sin(Time.time), 0f);
        var moveTransformCommand = new MoveTransformCommand
        {
            Position = position,
        };
        NetworkServer.SendToAll(moveTransformCommand);
    }

    private void OnJumped(NetworkConnectionToClient arg1, JumpCommand arg2)
    {
        JumpLocally();
    }

    private void JumpLocally()
    {
        _o.transform.Rotate(15, 0, 0);
    }

    private void OnMoved(MoveTransformCommand obj)
    {
        _o.transform.position = obj.Position;
    }
}