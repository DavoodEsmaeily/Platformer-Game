using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private float xInput;
    private Animator anim;
    //Clean Code with use of SerializeField Access to UI 
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;

    private int FacingDir = 1;
    private bool flipReight = true;

    [Header("Collision Info")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded;

    [Header("Dash Info")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashTime;

    [Header("Attack info")]
    [SerializeField] private bool isAttacking;
    [SerializeField] private int comboCounter;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        CheckInput();
        CollisionChecks();

        dashTime -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftShift))
            dashTime = dashDuration;

        if (Input.GetKeyDown(KeyCode.Mouse0))
            isAttacking = true;

        FlipController();
        AnimatorController();
    }

    private void CollisionChecks()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    private void CheckInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    private void Movement()
    {
        if (dashTime > 0)
            rb.velocity = new Vector2(xInput * dashSpeed, 0);
        else
            rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);
    }

    private void Jump()
    {
        if (isGrounded)
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    public void AttackOver()
    {
        isAttacking = false;
    }

    private void AnimatorController()
    {
        // Animation Moving Player
        bool isMoving = rb.velocity.x != 0;
        anim.SetFloat("yVelocity", rb.velocity.y);

        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isDashing" , dashTime > 0);
        anim.SetBool("isAttack", isAttacking);
        anim.SetInteger("comboCounter", comboCounter);
    }

    private void Flip()
    {
        FacingDir = FacingDir * -1;
        flipReight = !flipReight;
        transform.Rotate(0, 180, 0);
    }

    private void FlipController()
    {
        if (rb.velocity.x > 0 && !flipReight)
            Flip();
        else if (rb.velocity.x < 0 && flipReight)
            Flip();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundCheckDistance));
    }
}
