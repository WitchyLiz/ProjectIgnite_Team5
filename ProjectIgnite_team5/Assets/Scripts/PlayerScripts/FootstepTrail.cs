using System;
using System.Collections.Generic;
using Unity.GraphToolkit.Editor;
using UnityEngine;

public class FootstepTrail : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject FootstepPrefab;

    [Header("Instellingen")]
    public float stepInterval = 0.6f;
    public float minMoveDistance = 0.01f;   // behouden voor bestaande inspector-instellingen
    public int maxFootsteps = 500;
    public float sideOffset = 0.15f;
    public float yOffset = 0.02f;
    public float minSpeed = 0.1f;           // onder deze snelheid: geen stap

    [Header("Audio")]
	public AudioClip[] stepSound = new AudioClip[2];
	public AudioSource audioSource;
	private bool isFirstStep = true;

	private float stepTimer;
    private Vector3 lastPosition;
    private PlayerMovement playerMovement;
    private bool nextIsLeft = true;
    private List<GameObject> footsteps = new List<GameObject>();

    void Start()
    {
        lastPosition = transform.position;
        playerMovement = GetComponentInParent<PlayerMovement>();
    }

    void Update()
    {
        stepTimer -= Time.deltaTime;

        if (playerMovement == null || !playerMovement.IsMoving)
        {
            stepTimer = 0f;
            lastPosition = transform.position;
            return;
        }

        // Alleen horizontale verplaatsing telt; verticale gravity-beweging niet.
        Vector3 movement = transform.position - lastPosition;
        movement.y = 0f;
        float distance = movement.magnitude;
        float speed = distance / Mathf.Max(Time.deltaTime, 0.0001f);

        if (speed <= minSpeed)
        {
            stepTimer = 0f;
            lastPosition = transform.position;
            return;
        }

        if (stepTimer <= 0f)
        {
            SpawnFootstep();
            stepTimer = stepInterval;
        }

        lastPosition = transform.position;
    }

    void SpawnFootstep()
    {
        GameObject prefab = FootstepPrefab;
		if (prefab == null) return;

		float side = nextIsLeft ? -sideOffset : sideOffset;
        nextIsLeft = !nextIsLeft;

        Vector3 pos = transform.position + transform.right * side;

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 2f))
        {
            pos.y = hit.point.y + yOffset;
        }
        else
        {
            pos.y = yOffset;
        }

		GameObject step = Instantiate(prefab, pos, Quaternion.identity);

		//We doen even de orientatie van de voetstappen naar de forward van de speler met een genullificeerde Y component.
		Vector3 facingDirection = -transform.forward;
		facingDirection.y = 0f;
		step.transform.forward= facingDirection;

		//Voet stap spiegelen gebasseerd op "side".
		//Zo hebben we maar voetstap 1 prefab nodig
		Vector3 scale = step.transform.localScale;
		//We kunnen de Sign van "side gebruiken om de voetstap op de X as te scalen voor links en rechtss
		scale.x *= Mathf.Sign(side);
		step.transform.localScale = scale;

		footsteps.Add(step);

        if (footsteps.Count > maxFootsteps)
        {
            Destroy(footsteps[0]);
            footsteps.RemoveAt(0);
        }

		int index = isFirstStep ? 0 : 1;
		audioSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
		audioSource.PlayOneShot(stepSound[index]);
		isFirstStep = !isFirstStep;
	}
}