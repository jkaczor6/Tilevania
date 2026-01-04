using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 15f;
    [SerializeField] float climbSpeed = 5f;
    Animator animator;
    Vector2 moveInput;
    Rigidbody2D rb;
    CapsuleCollider2D coll;
    float originalGravity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        coll = GetComponent<CapsuleCollider2D>();
        originalGravity = rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        FlipSprite();
        ClimbLadder();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if(value.isPressed && coll.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            rb.linearVelocity += new Vector2(0f, jumpSpeed);
        }
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, rb.linearVelocity.y);
        rb.linearVelocity = playerVelocity;
        animator.SetBool("isRunning", Mathf.Abs(rb.linearVelocity.x) > Mathf.Epsilon);
    }

    void FlipSprite()
    {
        bool hasHorizontalSpeed = Mathf.Abs(rb.linearVelocity.x) > Mathf.Epsilon;
        if(hasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb.linearVelocity.x), 1f);
        }
    }

    void ClimbLadder()
    {
        bool isTouchingLadder = coll.IsTouchingLayers(LayerMask.GetMask("Climbing"));
        if(isTouchingLadder)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, moveInput.y * climbSpeed);
            rb.gravityScale = 0f;
            animator.SetBool("isClimbing", Mathf.Abs(rb.linearVelocity.y) > Mathf.Epsilon);
        }
        else
        {
            rb.gravityScale = originalGravity;
            animator.SetBool("isClimbing", false);
        }
    }
}
