using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    private PlayerMovement _playerMovement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
