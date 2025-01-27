using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMovement : MonoBehaviour
{
    public static CharacterMovement instance;
    public float moveSpeed = 5f;

    public float jumpForce = 10f;

    private CharacterGroundCheck groundCheck;

    [HideInInspector] public Rigidbody2D rb;

    private float moveInput;

    public int jumpCount = 1;
    private void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody2D>();
        groundCheck = GetComponent<CharacterGroundCheck>();
    }

    private void Update()
    {
        if (SkillManager.instance.selected_skill==Skill.MiniPlayer && transform.localScale!=Vector3.one*0.3f)
        {
            transform.localScale = Vector3.one * 0.3f;
        }
        else if (SkillManager.instance.selected_skill != Skill.MiniPlayer && transform.localScale == Vector3.one * 0.3f)
        {
            transform.localScale = Vector3.one;
        }
        moveInput = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && (groundCheck.isGrounded ||(SkillManager.instance.selected_skill==Skill.DoubleJump && jumpCount!=0)))
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        if (moveInput <= -0.1 && transform.position.x < (CheckpointUtility.ConvertIDToVector3(GameManager.instance.last_check_point.checkpointID).x - 2))
        {
            moveInput = 0;
        }
        if (SkillManager.instance.selected_skill==Skill.SpeedUp)
        {
            rb.velocity = new Vector2(moveInput * moveSpeed*3, rb.velocity.y);
            
        }
        else
        {
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        }
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        jumpCount--;
    }
}
