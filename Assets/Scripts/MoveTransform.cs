using Mirror;
using UnityEngine;

public class MoveTransform : MonoBehaviour
{
    public float TickTime = 0.1f;
    public float InterpolateTime = 0.1f;
    public float SnapTime = 0.2f;
    public bool Extrapolate = true;
    
    private Vector3 _lastVelocity;

    private GameObject _o;
    private float _timeSinceLastFrame;
    private Vector3 _serverPosition;

    private void Start()
    {
        _o = GameObject.CreatePrimitive(PrimitiveType.Cube);
        NetworkClient.RegisterHandler<MoveTransformCommand>(OnMoved, false);
        NetworkServer.RegisterHandler<JumpCommand>(OnJumped);

        InvokeRepeating(nameof(SimulateOnServer), 0, TickTime);
    }

    private void Update()
    {
        if (NetworkClient.active && NetworkClient.isConnected)
            OnClient();
    }

    private void OnClient()
    {
        _timeSinceLastFrame += Time.deltaTime;
        _o.transform.position = Interpolate(_o.transform.position, _serverPosition);

        if (Input.GetButtonDown("Jump"))
        {
            JumpLocally();
            NetworkClient.Send(new JumpCommand());
        }
    }

    private Vector3 Interpolate(Vector3 current, Vector3 target)
    {
        if (_timeSinceLastFrame > SnapTime)
            return target;
        var t = _timeSinceLastFrame / InterpolateTime;
        return Extrapolate ? Vector3.LerpUnclamped(current, target, t) : Vector3.Lerp(current, target, t);
    }

    private void SimulateOnServer()
    {
        if (NetworkServer.active || NetworkClient.isHostClient)
            OnServer();
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
        _timeSinceLastFrame = 0f;
        _serverPosition = obj.Position;
    }
}