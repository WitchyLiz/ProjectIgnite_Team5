using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{

    public Vector3 MoveDirection;

    // Private variables
    private float _movementSpeed = 5f;
    private CharacterController _controller;
    private Vector2 _moveInput;

    private float _lookSpeed = 2f;
    private float _lookLimit = 50f;

    private Vector2 _mouseRot;
    private float _mouseY;

    private float _jumpForce = 10f;

    [SerializeField]
    private Transform _playerBody;


    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        _controller.Move(MoveDirection * _movementSpeed * Time.deltaTime);
        _playerBody.Rotate(Vector2.up * _mouseRot.x * _lookSpeed);
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

    public void Look(InputAction.CallbackContext Context) 
    { 
        _mouseRot = Context.ReadValue<Vector2>();
    }
}
