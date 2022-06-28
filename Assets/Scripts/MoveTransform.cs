using Mirror;
using UnityEngine;

public class MoveTransform : MonoBehaviour
{
    private GameObject _o;

    private void Start()
    {
        _o = GameObject.CreatePrimitive(PrimitiveType.Cube);
        NetworkClient.RegisterHandler<MoveTransformCommand>(OnReceivedMessage, false);
    }

    private void Update()
    {
        if (!NetworkServer.active && !NetworkClient.isHostClient) return;

        var position = new Vector3(0, Mathf.Sin(Time.time), 0f);
        var moveTransformCommand = new MoveTransformCommand
        {
            Position = position,
        };
        NetworkServer.SendToAll(moveTransformCommand);
    }

    private void OnReceivedMessage(MoveTransformCommand obj)
    {
        _o.transform.position = obj.Position;
    }
}