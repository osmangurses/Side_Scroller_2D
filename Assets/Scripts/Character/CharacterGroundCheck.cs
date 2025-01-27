using UnityEngine;

public class CharacterGroundCheck : MonoBehaviour
{
    public float rayLength,wallRayLength = 0.1f;
    public Vector2 rayOrigin,wallRay1Origin,wallRay2Origin = Vector2.zero;
    public float rayAngle,wallRayAngle = 90f;
    public LayerMask groundLayer;

    public bool isGrounded;
    Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        PerformGroundCheck();
    }

    void PerformGroundCheck()
    {
        Vector2 rayDirection = new Vector2(Mathf.Cos(rayAngle * Mathf.Deg2Rad), Mathf.Sin(rayAngle * Mathf.Deg2Rad));
        Vector2 origin = (Vector2)transform.position + rayOrigin;
        RaycastHit2D hit = Physics2D.Raycast(origin, rayDirection, rayLength, groundLayer);
        if (SkillManager.instance.selected_skill==Skill.WallJump)
        {
            Vector2 rayDirection1 = new Vector2(Mathf.Cos(wallRayAngle * Mathf.Deg2Rad), Mathf.Sin(wallRayAngle * Mathf.Deg2Rad));
            Vector2 origin1 = (Vector2)transform.position + wallRay1Origin;

            Vector2 rayDirection2 = new Vector2(Mathf.Cos(wallRayAngle * Mathf.Deg2Rad), Mathf.Sin(wallRayAngle * Mathf.Deg2Rad));
            Vector2 origin2 = (Vector2)transform.position + wallRay2Origin;

            RaycastHit2D hit1 = Physics2D.Raycast(origin1, rayDirection1, wallRayLength, groundLayer);
            RaycastHit2D hit2 = Physics2D.Raycast(origin2, rayDirection2, wallRayLength, groundLayer);

            if (hit.collider!=null || hit1.collider!=null || hit2.collider!=null)
            {
                rb.gravityScale = 0;
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
                rb.gravityScale = 3;
            }
            Debug.DrawRay(origin1, rayDirection1 * wallRayLength, isGrounded ? Color.green : Color.red);
            Debug.DrawRay(origin2, rayDirection2 * wallRayLength, isGrounded ? Color.green : Color.red);
        }
        else
        {
            if (rb.gravityScale<3)
            {
                rb.gravityScale = 3;
            }
            isGrounded = hit.collider != null;
        }
        if (isGrounded && CharacterMovement.instance.jumpCount!=2)
        {
            CharacterMovement.instance.jumpCount = 1;
        }
        Debug.DrawRay(origin, rayDirection * rayLength, isGrounded ? Color.green : Color.red);
    }
}
