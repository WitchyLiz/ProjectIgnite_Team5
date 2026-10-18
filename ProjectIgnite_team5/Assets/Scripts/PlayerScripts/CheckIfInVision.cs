// Tutorial used: https://www.youtube.com/watch?v=38BtpQWEct4

// Documentations checked out in the meantime:
// https://docs.unity3d.com/6000.6/Documentation/ScriptReference/GeometryUtility.html
// https://docs.unity3d.com/6000.6/Documentation/ScriptReference/GeometryUtility.CalculateFrustumPlanes.html

using System.Linq;
using UnityEngine;

public class CheckIfInVision : MonoBehaviour
{

    //private variables

    [SerializeField]
    private GameObject _targetObject;
    [SerializeField]
    private Camera _camera;


    private void Update()
    {
        if (IsInFieldOfVision())
        {
            Debug.Log("in vision");

        }
        else 
        {
            Debug.Log("is not in vision");
            
        }
    }

    /// <summary>
    /// Returns a boolean value based on if the target object is in the FOV
    /// </summary>
    /// <returns>boolean</returns>
    public bool IsInFieldOfVision() 
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(_camera);
        return planes.All(plane => plane.GetDistanceToPoint(_targetObject.transform.position) >= 0);
    }

    private void InitiateTeleportation()
    {
        _targetObject.GetComponent<RiftMovement>().TeleportRift();
        return;
    }
}
