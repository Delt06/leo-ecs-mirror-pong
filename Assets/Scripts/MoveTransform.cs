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
        if (!NetworkServer.active && !NetworkClient.isHostClient)
        {
            if (NetworkClient.isConnected)
                if (Input.GetButtonDown("Jump"))
                {
                    JumpLocally();
                    NetworkClient.Send(new JumpCommand());
                }

            return;
        }

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