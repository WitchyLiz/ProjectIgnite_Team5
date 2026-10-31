using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class FadeTransition : MonoBehaviour
{
	[SerializeField] private CanvasGroup group;
	[SerializeField] private float fadeInDuration = 10f;
	[SerializeField] private float fadeOutDuration = 5f;

	
	private bool isTransitioning = false;

	private void Awake()
	{
		group = GetComponent<CanvasGroup>();
		group.alpha = 0.0f;
	}

	public void StartTeleport(Transform playerTransform, CharacterController controller, Vector3 targetPosition)
	{
		// prevents triggering a new fade while one is already running
		if (isTransitioning) return;
		StartCoroutine(FadeAndTeleportRoutine(playerTransform, controller, targetPosition));
	}

	public void FadeIn()
	{
		if (isTransitioning) return;
		isTransitioning = true;
		StartCoroutine(FadeRoutine(0f, 1f, fadeInDuration));
	}

	private IEnumerator FadeAndTeleportRoutine(Transform playerTransform, CharacterController controller, Vector3 targetPosition)
	{
		isTransitioning = true;

		//start fade in
		yield return StartCoroutine(FadeRoutine(0f, 1f, fadeInDuration));

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
		yield return StartCoroutine(FadeRoutine(1f, 0f, fadeOutDuration));

		// unlock so it can be retriggered again
		isTransitioning = false;
	}

	private IEnumerator FadeRoutine(float startAlpha, float endAlpha, float duration)
	{
		float elapsedTime = 0f;

		while (elapsedTime < duration)
		{
			elapsedTime += Time.deltaTime;
			float currentAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
			group.alpha = currentAlpha;
			yield return null;
		}

		group.alpha = endAlpha;
	}
}