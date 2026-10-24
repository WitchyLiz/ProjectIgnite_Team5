using Unity.Mathematics;
using UnityEngine;

public class ReturnPlayer : MonoBehaviour
{
    // Discussions used:
    // https://discussions.unity.com/t/teleporting-character-issue-with-transform-position-in-unity-2018-3/221631

    private Vector3 _initialPosition;
    private string _tagToCheck = "path";
    private CharacterController _controller;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _initialPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(_tagToCheck))
        {
            // Transform position won't work because of the character controller. Temportarily turns it off.
            _controller.enabled = false;
            transform.position = _initialPosition;
            _controller.enabled = true;
        }
    }
}
