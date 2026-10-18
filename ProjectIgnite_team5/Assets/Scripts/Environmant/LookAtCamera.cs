using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
	private Camera _camera;

	private void LateUpdate()
	{
		if (_camera == null)
			_camera = Camera.main;

		if (_camera == null)
			return;

		Vector3 direction = _camera.transform.position - transform.position;
		direction.y = 0f;

		transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
	}
}
