using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;

    private bool isDead = false;

    private Animator animator;
    private bool isGrounded;
    private Rigidbody2D rb;

    private float moveInput;
    private bool jumpPressed;
    private bool isSitting;

    private BoxCollider2D boxCollider;
    private Vector2 standingSize;
    private Vector2 sittingSize;
    private Vector2 standingOffset;
    private Vector2 sittingOffset;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        boxCollider = GetComponent<BoxCollider2D>();
        standingSize = boxCollider.size;
        standingOffset = boxCollider.offset;

        sittingSize = new Vector2(standingSize.x, standingSize.y * 0.5f);
        sittingOffset = new Vector2(standingOffset.x, standingOffset.y - standingSize.y * 0.25f);
    }

    void Update()
    {
        if (isDead) return;

        HandleMovement();
        HandleJump();
        UpdateAnimation();
    }

    private void HandleMovement()
    {
        if (isDead) return;

        if (isSitting)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            return;
        }

        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if (moveInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void HandleJump()
    {
        if (isDead) return;

        if (jumpPressed && isGrounded && !isSitting)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpPressed = false;
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
    }

    private void UpdateAnimation()
    {
        if (isDead) return;

        bool isRunning = Mathf.Abs(rb.velocity.x) > 0.1f;
        bool isJumping = !isGrounded;

        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isSitting", isSitting);
    }

    public void MoveLeftDown()
    {
        if (isDead) return;
        moveInput = -1;
    }

    public void MoveRightDown()
    {
        if (isDead) return;
        moveInput = 1;
    }

    public void StopMove()
    {
        moveInput = 0;
    }

    public void JumpButton()
    {
        if (isDead) return;
        jumpPressed = true;
    }

    public void SitDown()
    {
        if (isDead) return;

        isSitting = true;
        boxCollider.size = sittingSize;
        boxCollider.offset = sittingOffset;
    }

    public void StopSit()
    {
        isSitting = false;
        boxCollider.size = standingSize;
        boxCollider.offset = standingOffset;
    }

    public void Die(bool playAnimation)
    {
        if (isDead) return;

        isDead = true;

        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;

        animator.SetBool("isRunning", false);
        animator.SetBool("isJumping", false);
        animator.SetBool("isSitting", false);

        if (playAnimation)
        {
            animator.SetBool("isDead", true);

            float dir = transform.localScale.x > 0 ? -1 : 1;
            rb.AddForce(new Vector2(dir * 5f, 8f), ForceMode2D.Impulse);
        }
        else
        {
            rb.velocity = new Vector2(0, -5f);
        }

        boxCollider.enabled = false;
    }

    public void ResetPlayer()
    {
        isDead = false;

        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;

        boxCollider.enabled = true;

        animator.SetBool("isDead", false);
        animator.SetBool("isRunning", false);
        animator.SetBool("isJumping", false);
        animator.SetBool("isSitting", false);

        animator.Play("Idle");

        moveInput = 0;
        jumpPressed = false;
        isSitting = false;

        boxCollider.size = standingSize;
        boxCollider.offset = standingOffset;

        transform.rotation = Quaternion.identity;

        StartCoroutine(Invincible());
    }

    IEnumerator Invincible()
    {
        int playerLayer = LayerMask.NameToLayer("Player");
        int enemyLayer = LayerMask.NameToLayer("Enemy");

        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true);

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        for (int i = 0; i < 6; i++)
        {
            sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(0.2f);
        }
        sr.enabled = true;

        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);
    }
}