using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    [SerializeField] private PlayerInputAction inputActions; // Generated class from Input Action Asset
    [SerializeField] private Vector2 moveInput; // Store movement input
    [SerializeField] private bool jumpPressed;
    [SerializeField] private bool jumpHeld;
    [SerializeField] private bool leftMousePressed;
    [SerializeField] private bool rightMousePressed;
    [SerializeField] private bool dashPressed;
    private void Awake()
    {
        inputActions = new PlayerInputAction(); // Initialize input actions
    }

    private void OnEnable()
    {
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;

        inputActions.Player.Jump.started += OnJumpStarted;
        inputActions.Player.Jump.canceled += OnJumpCanceled;

        inputActions.Player.LeftMouse.performed += OnLeftMouse;
        inputActions.Player.LeftMouse.canceled += OnLeftMouse;

        inputActions.Player.RightMouse.performed += OnRightMouse;
        inputActions.Player.RightMouse.canceled += OnRightMouse;

        inputActions.Player.Slide.started += OnDash;
        inputActions.Enable(); // Enable input actions
    }

    private void OnLeftMouse(InputAction.CallbackContext context)
    {
    }

    private void OnDisable()
    {
        inputActions.Disable(); // Disable input actions
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>(); // Get movement input
    }

    private void OnJumpStarted(InputAction.CallbackContext context)
    {
        jumpPressed = true;
        jumpHeld = true;
        
    }

    private void OnJumpCanceled(InputAction.CallbackContext context)
    {
        jumpHeld = false;
        
    }

    private void OnRightMouse(InputAction.CallbackContext context)
    {
        if (context.performed)
            rightMousePressed = true; // Set right mouse button pressed
        else
            rightMousePressed = false;
    }

    public Vector2 GetMoveInput()
    {
        return moveInput; // Return current move input
    }

    public bool GetJumpInputDown()
    {
        if (jumpPressed)
        {
            jumpPressed = false; // Reset the press flag after it's read
            return true;
        }
        return false;
    }

    public bool GetLeftMouseInput()
    {
        bool leftClick = leftMousePressed;
        leftMousePressed = false; // Reset left mouse after use
        return leftClick;
    }

    public bool GetRightMouseInput()
    {
        bool rightClick = rightMousePressed;
        rightMousePressed = false; // Reset right mouse after use
        return rightClick;
    }
    private void OnDash(InputAction.CallbackContext context)
    {
        dashPressed = true;
    }

    public bool GetDashInputDown()
    {
        if (dashPressed)
        {
            Debug.Log("dash pressed");
            dashPressed = false; // Reset the press flag after it's read
            return true;
        }
        return false;
    }
}