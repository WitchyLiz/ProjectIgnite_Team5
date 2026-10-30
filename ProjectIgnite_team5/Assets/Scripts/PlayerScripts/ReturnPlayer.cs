using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class ReturnPlayer : MonoBehaviour
{
    

    private Vector3 _initialPosition;
    private string _nonPathTag = "nonpath";
	private string _finishTag = "Finish";
	private CharacterController _controller;
	[SerializeField] private FadeTransition _nonPathFade;
	[SerializeField] private FadeTransition _finishFade;

	void Start()
    {
        _controller = GetComponent<CharacterController>();
        _initialPosition = transform.position;

		if (_nonPathFade == null)
		{
			_nonPathFade = GameObject.Find("FadeInOut").GetComponent<FadeTransition>();
		}

		if (_finishFade == null)
		{
			_finishFade = GameObject.Find("EndScreen").GetComponent<FadeTransition>();
		}
	}

    private void OnTriggerEnter(Collider other)
    {
		if (other.CompareTag(_nonPathTag))
		{
			_nonPathFade.StartTeleport(transform, _controller, _initialPosition);
		}
		else if (other.CompareTag(_finishTag))
		{
			_finishFade.FadeIn();
		}
	}
}
