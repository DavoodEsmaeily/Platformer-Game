using UnityEngine;

public class Player : MonoBehaviour
{

    public int facingDir { get; private set; } = 1;
    private bool facingRight = true;

    [Header("Move info")]
    [SerializeField] public float moveSpeed = 12f;
    [SerializeField] public float jumpForce = 1f;


    [Header("Collision Info")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask whatIsGround;

    [Header("Dash Info")]
    [SerializeField] private float dashCoolDown;
    private float dashUsageTimer;

    public float dashSpeed;
    public float dashDuration;

    public float dashDir { get; private set; }

    #region Components
    public Animator Anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    #endregion

    #region States
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerAirState AirState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    #endregion

    private void Awake()
    {
        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(StateMachine, this, "idle");
        MoveState = new PlayerMoveState(StateMachine, this, "move");
        JumpState = new PlayerJumpState(StateMachine, this, "jump");
        AirState = new PlayerAirState(StateMachine, this, "jump");
        DashState = new PlayerDashState(StateMachine, this, "dash");
    }


    private void Start()
    {
        Anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        StateMachine.Initialize(IdleState);


    }

    private void Update()
    {
        StateMachine.CurrentState.Update();
        CheckForDashInpt();
    }

    private void CheckForDashInpt()
    {
        dashUsageTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashUsageTimer < 0)
        {
            dashUsageTimer = dashCoolDown;

            dashDir = Input.GetAxis("Horizontal");

            if (dashDir == 0)
                dashDir = facingDir;

            StateMachine.ChangeState(DashState);
        }
    }

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        rb.velocity = new Vector2(xVelocity, yVelocity);
        FlipController(xVelocity);
    }

    public bool IsGroundedDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);

    public void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    public void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)
            Flip();
        else if (_x < 0 && facingRight)
            Flip();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
    }
}
