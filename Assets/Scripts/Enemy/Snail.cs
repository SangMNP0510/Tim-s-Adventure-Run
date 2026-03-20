using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snail : MonoBehaviour
{
    public float speed = 2f;
    public float rollSpeed = 6f;

    [Header("Move Range")]
    public Transform pointA;
    public Transform pointB;

    private Rigidbody2D rb;
    private Animator anim;

    private bool movingRight = true;
    private bool isDead = false;
    private bool isRolling = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDead)
        {
            if (isRolling)
            {
                float dir = movingRight ? 1 : -1;
                rb.velocity = new Vector2(dir * rollSpeed, rb.velocity.y);
            }
            return;
        }

        Move();
    }

    void Move()
    {
        float dir = movingRight ? 1 : -1;
        rb.velocity = new Vector2(dir * speed, rb.velocity.y);

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (movingRight ? -1 : 1);
        transform.localScale = scale;

        if (movingRight && transform.position.x >= pointB.position.x)
            Flip();
        else if (!movingRight && transform.position.x <= pointA.position.x)
            Flip();
    }

    void Flip()
    {
        movingRight = !movingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public bool IsDead()
    {
        return isDead;
    }

    public bool IsRolling()
    {
        return isRolling;
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        isRolling = true;

        anim.SetBool("IsDead", true);

        float dir = movingRight ? 1 : -1;

        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(dir * rollSpeed, 2f), ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isRolling) return;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (Mathf.Abs(contact.normal.x) > 0.5f)
            {
                Flip();
                break;
            }
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Beetle beetle = collision.gameObject.GetComponent<Beetle>();
            if (beetle != null && !beetle.IsDead())
            {
                beetle.Die();
            }
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager gm = FindAnyObjectByType<GameManager>();
            gm.GameOver();
        }
    }

    private void OnBecameInvisible()
    {
        if (isRolling)
        {
            Destroy(gameObject);
        }
    }
}