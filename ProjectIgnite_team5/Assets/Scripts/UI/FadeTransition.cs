using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class FadeTransition : MonoBehaviour
{
	[SerializeField] private CanvasGroup _group;
	[SerializeField] private float _fadeInDuration = 10f;
	[SerializeField] private float _fadeOutDuration = 5f;

	
	private bool _isTransitioning = false;

	private void Awake()
	{
		_group = GetComponent<CanvasGroup>();
		_group.alpha = 0.0f;
	}

	public void StartTeleport(Transform playerTransform, CharacterController controller, Vector3 targetPosition)
	{
		// prevents triggering a new fade while one is already running
		if (_isTransitioning) return;
		StartCoroutine(FadeAndTeleportRoutine(playerTransform, controller, targetPosition));
	}

	public void FadeIn()
	{
		if (_isTransitioning) return;
		StartCoroutine(FadeInRoutine());
	}

	private IEnumerator FadeAndTeleportRoutine(Transform playerTransform, CharacterController controller, Vector3 targetPosition)
	{
		_isTransitioning = true;

		//start fade in
		yield return StartCoroutine(FadeRoutine(0f, 1f, _fadeInDuration));

		// teleport the player after the fade in is complete

		// Discussions used:
		// https://discussions.unity.com/t/teleporting-character-issue-with-transform-position-in-unity-2018-3/221631
		// player controller does wierd shit. need to disable to set the position
		controller.enabled = false;
		playerTransform.position = targetPosition;
		controller.enabled = true;
		

		//short pause at full white
		yield return new WaitForSeconds(0.1f);

		// fade back to the game
		yield return StartCoroutine(FadeRoutine(1f, 0f, _fadeOutDuration));

		// unlock so it can be retriggered again
		_isTransitioning = false;
	}

	private IEnumerator FadeInRoutine()
	{
		_isTransitioning = true;

		// Fade in to solid Alpha 0 -> 1
		yield return StartCoroutine(FadeRoutine(0f, 1f, _fadeInDuration));
	}

	private IEnumerator FadeRoutine(float startAlpha, float endAlpha, float duration)
	{
		float elapsedTime = 0f;

		while (elapsedTime < duration)
		{
			elapsedTime += Time.deltaTime;
			float currentAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
			_group.alpha = currentAlpha;
			yield return null;
		}

		_group.alpha = endAlpha;
	}
}