using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 15f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] float deathKick = 10f;
    Animator animator;
    Vector2 moveInput;
    Rigidbody2D rb;
    BoxCollider2D Bcoll;
    CapsuleCollider2D Ccoll;
    float originalGravity;

    bool isAlive = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        Bcoll = GetComponent<BoxCollider2D>();
        Ccoll = GetComponent<CapsuleCollider2D>();
        originalGravity = rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isAlive) { return; }
        Run();
        FlipSprite();
        ClimbLadder();
        Die();
    }

    void OnMove(InputValue value)
    {
        if(!isAlive) { return; }
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if(value.isPressed && Bcoll.IsTouchingLayers(LayerMask.GetMask("Ground")))
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
        bool isTouchingLadder = Ccoll.IsTouchingLayers(LayerMask.GetMask("Climbing"));
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

    void Die()
    {
        if(Bcoll.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")))
        {
            isAlive = false;
            animator.SetTrigger("Dying");
            rb.linearVelocity = new Vector2(-(transform.localScale.x * deathKick), jumpSpeed);
        }
    }
}
