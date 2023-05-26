using System.Collections;
using System.Collections.Generic;
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
    }

    private void Movement()
    {
        rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void AnimatorController()
    {
        // Animation Moving Player
        bool isMoving = rb.velocity.x != 0;
        anim.SetBool("IsMoving", isMoving);
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
}
