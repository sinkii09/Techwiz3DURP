using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private List<Transform> waypoints;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float smoothTime = 0.05f;
    [SerializeField] private float waypointThreshold = 0.1f;

    [Header("Jumping")]
    [SerializeField] private float jumpPower = 15f;
    [SerializeField] private int maxNumberOfJumps = 2;
    [SerializeField] private float gravityMultiplier = 2.0f;
    [SerializeField] private float airControlFactor = 0.5f;
    [SerializeField] private float jumpBufferTime = 0.2f;
    [SerializeField] private float coyoteTime = 0.15f;

    private const float GRAVITY = -9.81f;
    private float lastGroundedTime;
    private float jumpBufferCounter;
    private bool isJumpRequested ;

    [Header("Dashing")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.2f;

    private Vector3 dashDirection;
    private bool isDashing = false;

    [SerializeField] private Vector3 moveDirection;
    [SerializeField] private int prev = 0;
    [SerializeField] private int next = 1;
    [SerializeField] private int numberOfJumps;
    private Vector3 horizontalVelocity;
    private float currentVelocity;
    public bool IsGrounded => playerController.characterController.isGrounded;
    private PlayerController playerController;
    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }
    private void Update()
    {
        if (IsGrounded)
        {
            lastGroundedTime = Time.time;
            numberOfJumps = 0;
        }

        if (jumpBufferCounter > 0)
        {
            jumpBufferCounter -= Time.deltaTime;
        }
        UpdateMovement(playerController.inputController.GetMoveInput());

    }
    private void FixedUpdate()
    {
        ApplyMovement();
    }
    #region Handle Move
    public void ApplyMovement()
    {
        Vector3 movement = (horizontalVelocity + Vector3.up * moveDirection.y)*Time.deltaTime;
        playerController.characterController.Move(movement);

    }

    public void UpdateMovement(Vector2 input)
    {
        Vector3 targetDirection = Vector3.zero;
        if (input.x != 0)
        {
            int targetIndex = input.x > 0 ? next : prev;
            if(targetIndex>= 0 && targetIndex < waypoints.Count)
            {
                targetDirection = (waypoints[targetIndex].position - transform.position).normalized;
                moveDirection = new Vector3(targetDirection.x, moveDirection.y, targetDirection.z);

                if (HasReachedWaypoint(waypoints[targetIndex]))
                {
                    if (input.x > 0)
                    {
                        prev = targetIndex;
                        next = targetIndex + 1;
                    }
                    else
                    {
                        prev = targetIndex - 1;
                        next = targetIndex;
                    }
                }
            }
        }

        Vector3 targetVelocity = targetDirection * moveSpeed;

        // Smoothly interpolate current horizontal velocity towards target velocity
        float smoothFactor = IsGrounded ? 1f : airControlFactor;
        horizontalVelocity = Vector3.Lerp(horizontalVelocity, targetVelocity, smoothFactor * Time.deltaTime * 10f);

        moveDirection = new Vector3(horizontalVelocity.x, moveDirection.y, horizontalVelocity.z);
        UpdateRotation();
    }

    #endregion
    public void StartDash()
    {
        isDashing = true;
        dashDirection = new Vector3(moveDirection.x, 0, moveDirection.z).normalized;
        if (dashDirection == Vector3.zero)
        {
            dashDirection = (waypoints[next].position - transform.position).normalized;
        }
    }

    #region HandleJump
    public void RequestJump()
    {
        jumpBufferCounter = jumpBufferTime;
        isJumpRequested = true;
    }

    public void Jump()
    {
        if (CanJump())
        {
            moveDirection.y = jumpPower;
            numberOfJumps++;
            jumpBufferCounter = 0;
            isJumpRequested = false;
        }
    }

    public bool CanJump()
    {
        bool hasCoyoteTime = Time.time - lastGroundedTime <= coyoteTime;
        return (IsGrounded || hasCoyoteTime || numberOfJumps < maxNumberOfJumps) && isJumpRequested;
    }

    public void ProcessJump()
    {
        if (jumpBufferCounter > 0 && CanJump())
        {
            Jump();
        }
    }

    public void ResetJumpRequest()
    {
        isJumpRequested = false;
    }

    #endregion
    public void ApplyGravity()
    {
        if (IsGrounded && moveDirection.y < 0)
        {
            moveDirection.y = -1f;
        }
        else
        {
            moveDirection.y += GRAVITY * gravityMultiplier * Time.deltaTime;
        }
    }

    private void UpdateRotation()
    {
        if (horizontalVelocity.sqrMagnitude == 0) return;

        float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, smoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    private bool HasReachedWaypoint(Transform waypoint) =>
        Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z),
                         new Vector3(waypoint.position.x, 0, waypoint.position.z)) < waypointThreshold;

    //private int GetNextWaypointIndex() => currentWaypointIndex % waypoints.Count;

    //private int GetPreviousWaypointIndex() => (currentWaypointIndex - 1 + waypoints.Count) % waypoints.Count;

}