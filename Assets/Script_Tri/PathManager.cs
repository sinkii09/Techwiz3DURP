using System.Collections;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    public GameObject visualBody; // Reference to the visual body of the player
    private Quaternion targetRotation; // Store the target rotation for smooth lerping
    [SerializeField] AnimationController animationController;
    public float moveSpeed = 5f;

    AudioTesting audioTesting;

    [Header("Jump Settings")]
    public float jumpForce = 11f;
    public float fallMultiplier = 4f;
    public float peakGravityMultiplier = 6f; // New parameter
    public float peakThreshold = 7f; // New parameter
    public float ascendingControlMultiplier = 0.6f;
    public float descendingControlMultiplier = 0.4f;
    public float gravityChange = 1;
    private bool isJumping = false;
    public float dashSpeed = 20f;
    private CharacterController controller;

    [Header("Waypoint Tracking")]
    [SerializeField] public Waypoint currentWaypoint; // Set the initial waypoint in the Inspector
    [SerializeField] public Waypoint nextWaypoint;    // Visible in the Inspector for tracking the next waypoint
    [SerializeField] public Waypoint previousWaypoint; // Only used to update currentWaypoint when moving backward

    [SerializeField] private bool isPathTracking = true; // Start in freeform mode
    [SerializeField] float distanceThreshold;
    private Vector3 velocity;

    [SerializeField] private bool isDashing = false;
    [SerializeField] private Vector3 dashDirection;
    [SerializeField] private float dashDuration = 0.2f;  // Duration of the dash
    private float dashTimer = 0f;

    [SerializeField] private float dashCooldown = 5f;    // Cooldown time between dashes
    [SerializeField] private float dashCooldownTimer = 0f; // Timer to track cooldown

    private Vector3 freeformDirection; // Use the direction between current and next waypoint for freeform movement

    // Smooth movement transition
    [SerializeField] private float accelerationTime = 2f; // Time to reach full speed (adjust as needed)
    private float currentSpeed = 0f; // Speed starts at 0 and accelerates
    [SerializeField] private float decelerationFactor = 2f; // Speed reduction when no input (adjust as needed)


    PlayerController playerController;
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        audioTesting = visualBody.GetComponent<AudioTesting>();
        controller = GetComponent<CharacterController>();
        animationController = GetComponent<AnimationController>();
        UpdateWaypoints(); // Initialize waypoints at the start
        SetFreeformDirection(); // Set initial direction for freeform
        isPathTracking = true;
    }
    public void ApplyKnockback(Vector3 trapPosition, float knockbackForce)
    {
        // Calculate the direction from the trap to the player
        Vector3 trapToPlayerDirection = (transform.position - trapPosition).normalized;

        // Option 1: Constrain knockback along the path direction (current to next waypoint)
        Vector3 pathDirection = (nextWaypoint.transform.position - currentWaypoint.transform.position).normalized;

        // Project the trap-to-player knockback onto the path direction
        Vector3 knockbackDirection = Vector3.Project(trapToPlayerDirection, pathDirection);

        // Optionally, prevent movement on the Y-axis if you want a horizontal knockback
        knockbackDirection.y = 0;
        audioTesting.PlayHurtClip();
        // Apply knockback, ensuring it's a small force
        StartCoroutine(ApplyKnockbackCoroutine(knockbackDirection, knockbackForce));
    }
    [SerializeField] private bool isKnockedBack = false;   // To track if the player is currently in knockback
    [SerializeField] private float knockbackDuration = 0.3f; // Duration of the knockback effect
    private float knockbackTimer = 0f;                     // Timer to track the knockback duration
    private IEnumerator ApplyKnockbackCoroutine(Vector3 knockbackDirection, float knockbackForce)
    {
        // Disable normal movement
        isKnockedBack = true;
        knockbackTimer = 0f;
        controller.Move(Vector3.zero);
        animationController.SetTriggerIsKnockback();

        // Apply knockback for a short duration (e.g., 0.2 seconds)
        while (knockbackTimer < knockbackDuration)
        {
            knockbackTimer += Time.deltaTime;
            
            // Move the player based on the knockback direction and force
            Vector3 knockbackMovement = 5 * knockbackDirection.normalized * knockbackForce * Time.deltaTime;
            Debug.Log(knockbackMovement);
            // Apply the knockback force to the CharacterController
            controller.Move(knockbackMovement);

            yield return null; // Wait until next frame
        }
        Debug.Log("Knock");
        // Re-enable normal movement after the knockback duration
        isKnockedBack = false;
    }


    void Update()
    {
        if(playerController.isDead) return;
        if (isKnockedBack)
        {
            return;
        }

        if (isPathTracking)
        {
            HandlePathMovement();
        }
        else
        {
            HandleFreeMovement();
        }
        HandleJump();
    }
    void HandleJump()
    {
        bool isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Apply a small downward force when grounded
            isJumping = false;
        }

        // Apply gravity based on vertical velocity
        if (velocity.y < 0)
        {
            // Falling
            velocity.y += Physics.gravity.y * fallMultiplier * Time.deltaTime * gravityChange;
        }
        else if (velocity.y > 0 && velocity.y < peakThreshold)
        {
            // Near the peak of the jump
            velocity.y += Physics.gravity.y * peakGravityMultiplier * Time.deltaTime * gravityChange;
        }
        else
        {
            // Rising
            velocity.y += Physics.gravity.y * Time.deltaTime * gravityChange;
        }
    }

    public void SetPathTracking(bool value)
    {
        isPathTracking = value; // Toggle between freeform movement and path-following
    }

    public void SetCurrentWP(Waypoint wp)
    {
        currentWaypoint = wp;
        nextWaypoint = currentWaypoint.nextWaypoint;
        previousWaypoint = currentWaypoint.previousWaypoint;
        Debug.Log(currentWaypoint);
        Debug.Log(previousWaypoint);
    }

    private Vector3 moveDirection; // Make moveDirection a class-level variable to persist between frames
    void HandlePathMovement()
    {
        // Accelerate based on key hold duration (faster when moving)
        if (Input.GetKey(KeyCode.D)) // Move towards the next waypoint
        {
            currentSpeed += Time.deltaTime / (accelerationTime / 2); // Faster acceleration
            currentSpeed = Mathf.Clamp(currentSpeed, 0f, 1f); // Clamp speed between 0 and 1
            moveDirection = MoveAlongPath(1); // Update moveDirection towards the next waypoint
        }
        else if (Input.GetKey(KeyCode.A)) // Move towards the previous waypoint
        {
            currentSpeed += Time.deltaTime / (accelerationTime / 2); // Faster acceleration
            currentSpeed = Mathf.Clamp(currentSpeed, 0f, 1f); // Clamp speed between 0 and 1
            moveDirection = MoveAlongPath(-1); // Update moveDirection towards the previous waypoint
        }
        else
        {
            // Decelerate gradually when no input is pressed, but retain the last valid moveDirection
            currentSpeed -= Time.deltaTime / (accelerationTime / decelerationFactor);
            currentSpeed = Mathf.Clamp(currentSpeed, 0f, 1f); // Clamp speed between 0 and 1
        }

        // Apply the currentSpeed to the moveDirection, ensuring smooth deceleration
        Vector3 adjustedMoveDirection = moveDirection * currentSpeed;

        // Update the movement magnitude in the animator based on the currentSpeed
        animationController.UpdateMovementMagnitude(currentSpeed);

        ApplyMovement(adjustedMoveDirection); // Apply the adjusted moveDirection
    }

    void HandleFreeMovement()
    {
        // Accelerate based on key hold duration (faster when moving)
        if (Input.GetKey(KeyCode.D)) // Move in the freeform forward direction
        {
            currentSpeed += Time.deltaTime / (accelerationTime / 2); // Faster acceleration
            currentSpeed = Mathf.Clamp(currentSpeed, 0f, 1f); // Clamp speed between 0 and 1
            moveDirection = freeformDirection; // Update moveDirection in the freeform forward direction
        }
        else if (Input.GetKey(KeyCode.A)) // Move in the freeform backward direction
        {
            currentSpeed += Time.deltaTime / (accelerationTime / 2); // Faster acceleration
            currentSpeed = Mathf.Clamp(currentSpeed, 0f, 1f); // Clamp speed between 0 and 1
            moveDirection = -freeformDirection; // Update moveDirection in the freeform backward direction
        }
        else
        {
            // Decelerate gradually when no input is pressed, but retain the last valid moveDirection
            currentSpeed -= Time.deltaTime / (accelerationTime / decelerationFactor);
            currentSpeed = Mathf.Clamp(currentSpeed, 0f, 1f); // Clamp speed between 0 and 1
        }

        // Apply the currentSpeed to the moveDirection, ensuring smooth deceleration
        Vector3 adjustedMoveDirection = moveDirection * currentSpeed;

        // Update the movement magnitude in the animator based on the currentSpeed
        animationController.UpdateMovementMagnitude(currentSpeed);

        ApplyMovement(adjustedMoveDirection); // Apply the adjusted moveDirection
    }
    private Vector3 previousMoveDirection;
    private bool canDoubleJump = false; // Track if the player can double jump
    public bool isDoubleJumpAllowed = false; // Flag to allow or disallow double jump
    void ApplyMovement(Vector3 moveDirection)
    {
        // Smooth out direction changes
        Vector3 smoothedDirection = Vector3.Lerp(previousMoveDirection, moveDirection, Time.deltaTime * 5f);
        previousMoveDirection = smoothedDirection;

        // Update movement magnitude in the animator
        float movementMagnitude = smoothedDirection.magnitude;
        animationController.UpdateMovementMagnitude(movementMagnitude);

        // Update grounded status in the animator
        bool isGrounded = controller.isGrounded;
        animationController.SetIsGrounded(isGrounded); // Update isGrounded in the Animator

        // Update the dash cooldown timer
        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }

        // Jump logic (works in both modes)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                velocity.y = jumpForce; // Apply jump force for the first jump
                animationController.TriggerJump(); // Trigger Jump animation
                isJumping = true;
                canDoubleJump = true; // Enable double jump (but we'll check if it's allowed later)
            }
            else if (canDoubleJump && isDoubleJumpAllowed)
            {
                velocity.y = jumpForce; // Apply second jump force
                animationController.TriggerJump(); // Trigger double jump animation (if available)
                canDoubleJump = false; // Disable further double jumps
            }
        }

        // Apply gravity based on vertical velocity
        if (velocity.y < 0)
        {
            // Falling
            velocity.y += Physics.gravity.y * fallMultiplier * Time.deltaTime;
        }
        else if (velocity.y > 0 && velocity.y < peakThreshold)
        {
            // Near the peak of the jump
            velocity.y += Physics.gravity.y * peakGravityMultiplier * Time.deltaTime;
        }
        else
        {
            // Rising
            velocity.y += Physics.gravity.y * Time.deltaTime;
        }

        // Apply in-air control
        if (!isGrounded)
        {
            if (velocity.y > 0) // Ascending
            {
                smoothedDirection *= ascendingControlMultiplier;
            }
            else // Descending
            {
                smoothedDirection *= descendingControlMultiplier;
            }
        }

        // Final movement calculation
        Vector3 finalMovement = moveDirection * moveSpeed * Time.deltaTime;
        finalMovement.y = velocity.y * Time.deltaTime;

        // Move the player
        if (!isKnockedBack)
            controller.Move(finalMovement);

        // Update the target rotation if there is movement
        if (moveDirection != Vector3.zero)
        {
            SetTargetRotation(moveDirection);
        }

        // Continue rotating towards the target rotation even if movement stops
        RotateVisualBodyTowardsTarget();

        // Dash logic
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && dashCooldownTimer <= 0f)
        {
            isDashing = true;
            dashTimer = 0f;
            dashDirection = moveDirection.normalized; // Save the current movement direction for the dash
            dashCooldownTimer = dashCooldown; // Start the cooldown timer

            // Use triggers for DashGround and DashAir
            if (isGrounded)
            {
                animationController.TriggerDashGround();
            }
            else
            {
                animationController.TriggerDashAir();
            }
        }

        // Dash state logic
        if (isDashing)
        {
            Dash(); // Call dash movement logic
        }

        // Reset jump when grounded
        if (isGrounded)
        {
            isJumping = false;
            canDoubleJump = true; // Reset double jump ability
        }

        // Update waypoints only when grounded
        UpdateCurrentWaypoint();
    }



    // Smooth dash movement logic
    void Dash()
    {
        fallMultiplier = 0.1f;
        dashTimer += Time.deltaTime;

        // Keep moving in the dash direction
        Vector3 dashMovement = dashDirection * dashSpeed * Time.deltaTime;
        dashMovement.y = velocity.y * Time.deltaTime;  // Apply gravity during dash

        // Move the player
        controller.Move(dashMovement);

        // Stop dashing after dashDuration has passed
        if (dashTimer >= dashDuration)
        {
            isDashing = false;
            animationController.SetBoolCheckOnAir();
            fallMultiplier = 4f;
        }
    }

    // Set the target rotation based on movement direction
    void SetTargetRotation(Vector3 moveDirection)
    {
        Quaternion fullRotation = Quaternion.LookRotation(moveDirection);
        float newYRotation = fullRotation.eulerAngles.y;
        targetRotation = Quaternion.Euler(visualBody.transform.eulerAngles.x, newYRotation, visualBody.transform.eulerAngles.z);
    }

    // Rotate the visual body smoothly towards the target rotation
    void RotateVisualBodyTowardsTarget()
    {
        visualBody.transform.rotation = Quaternion.Slerp(visualBody.transform.rotation, targetRotation, Time.deltaTime * 8f); // Smooth rotation
    }

    void SetFreeformDirection()
    {
        // Set freeform direction based on the direction from current to next waypoint
        if (nextWaypoint != null)
        {
            freeformDirection = (nextWaypoint.transform.position - currentWaypoint.transform.position).normalized;
        }
    }

    Vector3 MoveAlongPath(int direction)
    {
        Vector3 moveDirection;
        if (direction > 0 && nextWaypoint != null) // Moving forward
        {
            moveDirection = (nextWaypoint.transform.position - transform.position).normalized;
        }
        else if (direction < 0 && previousWaypoint != null) // Moving backward
        {
            moveDirection = (currentWaypoint.transform.position - transform.position).normalized;
        }
        else
        {
            moveDirection = Vector3.zero;
        }

  
        return moveDirection;
    }

    void UpdateCurrentWaypoint()
    {
        // Calculate the distance only in the X and Z plane (ignore Y axis)
        Vector3 horizontalPlayerPosition = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 horizontalNextWaypointPosition = new Vector3(nextWaypoint.transform.position.x, 0, nextWaypoint.transform.position.z);
        Vector3 horizontalCurrentWaypointPosition = new Vector3(currentWaypoint.transform.position.x, 0, currentWaypoint.transform.position.z);

        // Set the threshold based on whether the player is jumping
        distanceThreshold = controller.isGrounded ? 1f : 2f; // If grounded, threshold is 1, else it's 3 for jumping

        // Moving forward (D key) from current to next waypoint
        if (Input.GetKey(KeyCode.D) && nextWaypoint != null)
        {
            // Use the X and Z plane distance (ignoring Y)
            if (Vector3.Distance(horizontalPlayerPosition, horizontalNextWaypointPosition) < distanceThreshold)
            {
                previousWaypoint = currentWaypoint; // Save current as previous
                currentWaypoint = nextWaypoint;     // Move to next
                nextWaypoint = currentWaypoint.nextWaypoint; // Update next waypoint

                SetFreeformDirection(); // Update freeform direction based on new waypoints
            }
        }
        // Moving backward (A key) from current to previous waypoint
        else if (Input.GetKey(KeyCode.A) && previousWaypoint != null)
        {
            // Use the X and Z plane distance (ignoring Y)
            if (Vector3.Distance(horizontalPlayerPosition, horizontalCurrentWaypointPosition) < distanceThreshold)
            {
                nextWaypoint = currentWaypoint;     // Set current as next
                currentWaypoint = previousWaypoint; // Move to previous
                previousWaypoint = currentWaypoint.previousWaypoint; // Update previous waypoint

                SetFreeformDirection(); // Update freeform direction based on new waypoints
            }
        }
    }
    void UpdateWaypoints()
    {
        nextWaypoint = currentWaypoint.nextWaypoint;
        previousWaypoint = currentWaypoint.previousWaypoint;
    }
}