using UnityEngine;

public class RiftMovement : MonoBehaviour
{
    private float _xMax = 50f;
    private float _zMax = 50f;
    private float _xMin = 0f;
    private float _zMin = 0f;

    public bool HasBeenSeen = false;
    public bool OutOfVision = false;

    private float GetTeleportationBounds(float valueMin, float valueMax) 
    {
        float pos = Random.Range(valueMin, valueMax);

        return pos;
    }

    public void TeleportRift() 
    {
        float xPos = GetTeleportationBounds(_xMin, _xMax);
        float zPos = GetTeleportationBounds(_zMin, _zMax);

        transform.position = new Vector3(xPos, transform.position.y, zPos);
    }

}
