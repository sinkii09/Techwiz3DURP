using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float movementMagnitude;
    [SerializeField] private bool isGround;
    
    PlayerController playerController;
    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    public Animator Animator
    {
        get
        {
            return animator;
        }

        set
        {
            animator = value;
        }
    }
    void Start()
    {
    }

    // Update the movement magnitude (for blend tree transitions)
    public void UpdateMovementMagnitude(float magnitude)
    {
        animator.SetFloat("movementMagnitude", magnitude);
        movementMagnitude = magnitude;
    }

    // Trigger the jump animation
    public void TriggerJump()
    {
        animator.SetTrigger("Jump");
    }

    // Trigger DashGround
    public void TriggerDashGround()
    {
        animator.SetTrigger("DashGround");
    }

    // Trigger DashAir
    public void TriggerDashAir()
    {
        animator.SetTrigger("DashAir");
    }

    // Set the CheckOnAir state
    public void SetBoolCheckOnAir()
    {
        animator.SetBool("CheckOnAir", true);
    }

    // Update the isGrounded parameter
    public void SetIsGrounded(bool isGrounded)
    {
        if (this.isGround != isGrounded)
        {
            animator.SetBool("isGrounded", isGrounded);
            isGround = isGrounded;
            animator.SetBool("CheckOnAir", false);
        }
    }
    public void SetTriggerIsKnockback()
    {
        animator.SetTrigger("isKnockback");
    }
    public void SettriggerDead()
    {
        animator.SetTrigger("isDead");
    }
    public void DeadEvent()
    {
        Debug.Log("event call");
        playerController.DeadAnimationEvent();
    }
}
