using UnityEngine;

public class Player : Entity
{
    private float xInput;
    //Clean Code with use of SerializeField Access to UI 
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;


    [Header("Dash Info")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashTime;

    [Header("Attack info")]
    [SerializeField] private float comboTime = .3f;
    [SerializeField] private float comboTimeWindow;
    [SerializeField] private bool isAttacking;
    [SerializeField] private int comboCounter;
    [SerializeField] private float dashCooldown;
    [SerializeField] private float dashCooldownTimer;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

   
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        Movement();
        CheckInput();

        dashTime -= Time.deltaTime;
        comboTimeWindow -= Time.deltaTime;
        dashCooldownTimer -= Time.deltaTime;



        FlipController();
        AnimatorController();
    }

    private void CheckInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
            DashAbility();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartAttackEvent();
        }
    }

    private void StartAttackEvent()
    {
        if (!isGrounded)
            return;

        if (comboTimeWindow < 0)
            comboCounter = 0;

        isAttacking = true;
        comboTimeWindow = comboTime;
    }


    private void DashAbility()
    {
        if(dashCooldownTimer < 0 && !isAttacking)  
        {
            dashCooldownTimer = dashCooldown;
            dashTime = dashDuration;
        }
    }

    private void Movement()
    {
        if(isAttacking)
        {
            rb.velocity = new Vector2(0, 0);
        }
       else if (dashTime > 0)
            rb.velocity = new Vector2(facingDir * dashSpeed, 0);
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
        comboCounter++;

        if (comboCounter > 2)
            comboCounter = 0;
    }

    private void AnimatorController()
    {
        // Animation Moving Player
        bool isMoving = rb.velocity.x != 0;
        anim.SetFloat("yVelocity", rb.velocity.y);

        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isDashing", dashTime > 0);
        anim.SetBool("isAttack", isAttacking);
        anim.SetInteger("comboCounter", comboCounter);
    }

    private void FlipController()
    {
        if (rb.velocity.x > 0 && !flipReight)
            Flip();
        else if (rb.velocity.x < 0 && flipReight)
            Flip();
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }

}
