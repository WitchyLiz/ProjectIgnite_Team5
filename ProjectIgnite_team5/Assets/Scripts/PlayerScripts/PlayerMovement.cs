using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{

    public Vector3 MoveDirection;

    // Variables
    private float _movementSpeed = 5f;
    private CharacterController _controller;
    private Vector2 _moveInput;
   

    private float _jumpForce = 10f;


    void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        _controller.Move(MoveDirection * _movementSpeed * Time.deltaTime);

    }

    //This will be called in the player's properties. Look at: player input > events > player > move.
    public void Move(InputAction.CallbackContext Context)
    {
        _moveInput = Context.ReadValue<Vector2>();
        MoveDirection = new Vector3(_moveInput.x, 0.0f, _moveInput.y);
    }

    public void Jump(InputAction.CallbackContext Context) 
    {
        //_Rigidbody.AddForce(Vector3.up * _jumpForce * _Rigidbody.mass, ForceMode.Impulse);
    }
}
