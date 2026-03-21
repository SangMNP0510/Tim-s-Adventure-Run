using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beetle : MonoBehaviour
{
    public float speed = 2f;

    [Header("Move Path")]
    public List<Transform> movePoints;

    [Header("Step Climb")]
    public float stepHeight = 0.2f;
    public float checkDistance = 0.15f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator anim;
    private CapsuleCollider2D col;

    private int currentIndex = 0;
    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        col = GetComponent<CapsuleCollider2D>();
    }

    void FixedUpdate()
    {
        if (isDead) return;

        Move();
    }

    float GetDirection()
    {
        if (movePoints == null || movePoints.Count == 0) return 0;

        Transform target = movePoints[currentIndex];
        return target.position.x > transform.position.x ? 1 : -1;
    }

    void Move()
    {
        if (movePoints == null || movePoints.Count == 0) return;

        float direction = GetDirection();

        HandleStepClimb(direction);

        rb.velocity = new Vector2(direction * speed, rb.velocity.y - 0.1f);

        Transform target = movePoints[currentIndex];

        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            currentIndex++;
            if (currentIndex >= movePoints.Count)
                currentIndex = 0;
        }
    }

    void HandleStepClimb(float direction)
    {
        Bounds bounds = col.bounds;

        Vector2 originLow = new Vector2(bounds.center.x, bounds.min.y + 0.05f);
        Vector2 originHigh = originLow + Vector2.up * stepHeight;

        RaycastHit2D hitLow = Physics2D.Raycast(originLow, Vector2.right * direction, checkDistance, groundLayer);
        RaycastHit2D hitHigh = Physics2D.Raycast(originHigh, Vector2.right * direction, checkDistance, groundLayer);

        if (hitLow && !hitHigh)
        {
            rb.position += Vector2.up * stepHeight;
        }

        Debug.DrawRay(originLow, Vector2.right * direction * checkDistance, Color.red);
        Debug.DrawRay(originHigh, Vector2.right * direction * checkDistance, Color.green);
    }

    public bool IsDead()
    {
        return isDead;
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;

        anim.SetBool("IsDead", true);

        GetComponent<Collider2D>().enabled = false;

        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Dynamic;

        rb.AddForce(Vector2.up * 5f, ForceMode2D.Impulse);

        Destroy(gameObject, 0.7f);
    }
}