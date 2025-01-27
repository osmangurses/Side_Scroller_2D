using UnityEngine;

public class CharacterAnimationManager : MonoBehaviour
{
    private Animator animator;
    private CharacterMovement movement;
    private CharacterGroundCheck groundCheck;
    private SpriteRenderer spriteRenderer;

    private bool isFacingRight = true;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<CharacterMovement>();
        groundCheck = GetComponent<CharacterGroundCheck>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component is missing!");
        }
    }

    private void Update()
    {
        HandleAnimation();
        HandleCharacterDirection();
    }

    void HandleAnimation()
    {
        if (!groundCheck.isGrounded)
        {
            if (movement.rb.velocity.y > 0)
            {
                animator.Play("JumpingUp");
            }
            else
            {
                animator.Play("JumpingDown");
            }
        }
        else
        {
            if (Mathf.Abs(movement.rb.velocity.x) > 0.1f)
            {
                animator.Play("Run");
            }
            else
            {
                animator.Play("Idle");
            }
        }
    }

    void HandleCharacterDirection()
    {
        float horizontalInput = movement.rb.velocity.x;
        if (horizontalInput > 0.1f && !isFacingRight)
        {
            Flip();
        }
        else if (horizontalInput < -0.1f && isFacingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = !isFacingRight;
        }
    }
}
