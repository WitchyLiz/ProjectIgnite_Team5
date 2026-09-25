using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;


[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{

    //public variables
    public Transform Orientation;

    // Private variables

    //player
    private CharacterController _controller;

    // Movement
    private float _movementSpeed = 2.5f;
    private float _gravity = -20f;
    private float _jumpHeight = 1.5f;
    private float _verticalVelocity;
    private Vector2 _moveInput;
    // Rotation
    private Vector2 _mouseRot;
    private float _xRot = 0f;
    private float _yRot = 0f;
    private float _lookSpeed = 1.5f;

    //variables for smoother rotation
    private float _drag = 0.04f;
    private float _currentXRot;
    private float _currentYRot;
    private float _xRotVelocity;
    private float _yRotVelocity;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        ApplyGravity();
        GetRotation();
        Move();
    }

    private void GetRotation()
    {
        _xRot -= _mouseRot.y;
        _xRot = Mathf.Clamp(_xRot, -35f, 35f);
        _yRot += _mouseRot.x;

        _currentXRot = Mathf.SmoothDamp(_currentXRot, _xRot, ref _xRotVelocity, _drag);
        _currentYRot = Mathf.SmoothDamp(_currentYRot, _yRot, ref _yRotVelocity, _drag);

       transform.rotation = Quaternion.Euler(_currentXRot * _lookSpeed, _currentYRot * _lookSpeed, 0f);
    }

    private void Move() 
    {
        Vector3 direction = GetMoveDirection();

        Vector3 move = direction.normalized * _movementSpeed;
        move.y = _verticalVelocity;

        _controller.Move(move * Time.deltaTime);

    }

    private void ApplyGravity()
    {
        if (_controller.isGrounded && _verticalVelocity < 0f)
            _verticalVelocity = -2f;
        else
            _verticalVelocity += _gravity * Time.deltaTime;
    }

    private Vector3 GetMoveDirection()
    {
        Vector3 direction = Orientation.forward * _moveInput.y + Orientation.right * _moveInput.x;
        direction.y = 0f;
        return direction;

    }

    //This will be called in the player's properties. Look at: player input > events > player > move. Basically just collects the inputs and gives them to this script.
    public void OnMove(InputAction.CallbackContext Context)
    {
        _moveInput = Context.ReadValue<Vector2>();
    }


    public void Jump(InputAction.CallbackContext Context) 
    {
        if (Context.performed && _controller.isGrounded)
        {
            _verticalVelocity = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
        }
    }

    public void Look(InputAction.CallbackContext Context)
    {
        _mouseRot = Context.ReadValue<Vector2>();
    }
}
