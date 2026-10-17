using UnityEngine;
using System.Collections.Generic;

public class FootstepTrail : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject leftFootstepPrefab;
    public GameObject rightFootstepPrefab;

    [Header("Instellingen")]
    public float stepInterval = 0.6f;
    public float minMoveDistance = 0.01f;   // behouden voor bestaande inspector-instellingen
    public int maxFootsteps = 500;
    public float sideOffset = 0.15f;
    public float yOffset = 0.02f;
    public float minSpeed = 0.1f;           // onder deze snelheid: geen stap

    [Header("Audio")]
    public AudioClip stepSound;
    public AudioSource audioSource;

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
        GameObject prefab = nextIsLeft ? leftFootstepPrefab : rightFootstepPrefab;
        float side = nextIsLeft ? -sideOffset : sideOffset;
        nextIsLeft = !nextIsLeft;

        if (prefab == null) return;

        Vector3 pos = transform.position + transform.right * side;

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 2f))
        {
            pos.y = hit.point.y + yOffset;
        }
        else
        {
            pos.y = yOffset;
        }

        Quaternion flatten = Quaternion.Euler(90, 0, 0);
        Quaternion faceForward = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        Quaternion rot = faceForward * flatten;

        GameObject step = Instantiate(prefab, pos, rot);
        footsteps.Add(step);

        if (footsteps.Count > maxFootsteps)
        {
            Destroy(footsteps[0]);
            footsteps.RemoveAt(0);
        }

        if (audioSource != null && stepSound != null)
            audioSource.PlayOneShot(stepSound);
    }
}