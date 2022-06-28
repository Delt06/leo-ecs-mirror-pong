using Mirror;
using UnityEngine;

public struct MoveTransformCommand : NetworkMessage
{
    public Vector3 Position;
}