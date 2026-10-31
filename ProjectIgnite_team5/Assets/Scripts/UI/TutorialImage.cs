using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialImage : MonoBehaviour
{
	private RectTransform rectTransform;

	[SerializeField] private float timeBeforeDrop = 10f;
	[SerializeField] private float dropDistance = 150f;
	[SerializeField]private float lerpDuration = 4.0f;
	private bool hasTriggered = false;
	void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
	}

	public void OnMove(InputAction.CallbackContext context)
	{
		if (hasTriggered || !context.performed) return;

		hasTriggered = true;
		StartCoroutine(WaitAndDropRoutine());
	}

	private IEnumerator WaitAndDropRoutine()
	{
		// Wait so ui does no drop right away
		yield return new WaitForSeconds(timeBeforeDrop);

	
		Vector2 startPosition = rectTransform.anchoredPosition;
		Vector2 targetPosition = startPosition - new Vector2(0f, dropDistance);

		float elapsedTime = 0f;

		while (elapsedTime < lerpDuration)
		{
			elapsedTime += Time.deltaTime;
			float t = Mathf.Clamp01(elapsedTime / lerpDuration);
			//make it an S curve so it moves nicer
			float smoothT = Mathf.SmoothStep(0f, 1f, t);
			rectTransform.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, smoothT);
			yield return null;
		}

		rectTransform.anchoredPosition = targetPosition;
	}
}