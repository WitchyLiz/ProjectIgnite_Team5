using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
	private Camera camera;

    private void Start()
    {
        camera ??= Camera.main;
        
    }

    private void LateUpdate()
	{
        if (camera == null) return;

        Vector3 direction = camera.transform.position - transform.position;
		direction.y = 0f;

		transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
	}
}
