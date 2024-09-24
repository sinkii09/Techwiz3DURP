using UnityEngine;
using System.Collections.Generic;

public class MovementController : MonoBehaviour
{
    public enum MovementState
    {
        Move,
        Jump,
        Slide
    }

    [SerializeField] private float dampingFactor = 5f;

    private Vector3 currentVelocity;

    [SerializeField] private MovementState currentState;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float slideSpeed = 8f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpTime = 0.5f;

    [Header("Waypoint System")]
    [SerializeField] private List<Transform> waypoints;
    [SerializeField] private float waypointThreshold = 0.1f;
    [SerializeField] private int currentWaypointIndex = 0;
    [SerializeField] private int targetWaypointIndex = 0;

    private CharacterController characterController;
    private Vector3 velocity;
    private bool canDoubleJump;
    private float slideTime;
    private float jumpTimeCounter;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        currentState = MovementState.Move;
    }

    private void Update()
    {
        HandleInput();
        UpdateState();
        ApplyGravity();

        characterController.Move(velocity * Time.deltaTime);
    }

    private void HandleInput()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        switch (currentState)
        {
            case MovementState.Move:
                HandleWaypointMovement(horizontalInput);

                if (Input.GetKeyDown(KeyCode.Space) && characterController.isGrounded)
                {
                    SwitchMovementState(MovementState.Jump);
                }
                else if (Input.GetKeyDown(KeyCode.C) && characterController.isGrounded)
                {
                    SwitchMovementState(MovementState.Slide);
                }
                break;

            case MovementState.Jump:
                HandleWaypointMovement(horizontalInput);

                if (Input.GetKeyDown(KeyCode.Space) && canDoubleJump)
                {
                    velocity.y = jumpForce;
                    canDoubleJump = false;
                    jumpTimeCounter = jumpTime;
                }
                break;

            case MovementState.Slide:
                HandleWaypointMovement(horizontalInput, slideSpeed);
                break;
        }
    }

    private void HandleWaypointMovement(float input, float speed = -1)
    {
        if (speed < 0) speed = moveSpeed;

        Vector3 targetVelocity;

        if (input != 0)
        {
            targetWaypointIndex = input > 0 ?
                (currentWaypointIndex + 1) % waypoints.Count :
                (currentWaypointIndex - 1 + waypoints.Count) % waypoints.Count;

            Vector3 targetPosition = waypoints[targetWaypointIndex].position;
            Vector3 directionToTarget = (targetPosition - transform.position).normalized;

            targetVelocity = new Vector3(directionToTarget.x * speed, 0, directionToTarget.z * speed);

            if (HasReachedWaypoint(waypoints[targetWaypointIndex]))
            {
                currentWaypointIndex = targetWaypointIndex;
            }
        }
        else
        {
            targetVelocity = Vector3.zero;
            targetWaypointIndex = currentWaypointIndex;
        }

        currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, dampingFactor * Time.deltaTime);

        velocity.x = currentVelocity.x;
        velocity.z = currentVelocity.z;

        UpdateRotation();
    }

    private void UpdateState()
    {
        switch (currentState)
        {
            case MovementState.Move:
                // Already handled in HandleInput
                break;

            case MovementState.Jump:
                jumpTimeCounter -= Time.deltaTime;
                if (jumpTimeCounter <= 0 && characterController.isGrounded)
                {
                    SwitchMovementState(MovementState.Move);
                }
                break;

            case MovementState.Slide:
                slideTime += Time.deltaTime;
                if (slideTime >= 1f || !Input.GetKey(KeyCode.C))
                {
                    SwitchMovementState(MovementState.Move);
                }
                break;
        }
    }

    private void ApplyGravity()
    {
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        velocity.y += gravity * Time.deltaTime;
    }

    public void SwitchMovementState(MovementState newState)
    {
        ExitCurrentState();
        EnterNewState(newState);
        currentState = newState;
    }

    private void ExitCurrentState()
    {
        switch (currentState)
        {
            case MovementState.Move:
                break;
            case MovementState.Jump:
                break;
            case MovementState.Slide:
                slideTime = 0f;
                break;
        }
    }

    private void EnterNewState(MovementState newState)
    {
        switch (newState)
        {
            case MovementState.Move:
                break;
            case MovementState.Jump:
                velocity.y = jumpForce;
                canDoubleJump = true;
                jumpTimeCounter = jumpTime;
                break;
            case MovementState.Slide:
                // Implement slide logic (e.g., change collider size)
                break;
        }
    }

    private bool HasReachedWaypoint(Transform waypoint) =>
        Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z),
                         new Vector3(waypoint.position.x, 0, waypoint.position.z)) < waypointThreshold;

    private void UpdateRotation()
    {
        if (currentVelocity.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.LookRotation(currentVelocity);
        }
    }
}