using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beetle : MonoBehaviour
{
    public float speed = 2f;

    [Header("Move Range")]
    public Transform pointA;
    public Transform pointB;

    private Rigidbody2D rb;
    private Animator anim;

    private bool movingRight = true;
    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDead) return;

        Move();
    }

    void Move()
    {
        if (movingRight)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);

            if (transform.position.x >= pointB.position.x)
                Flip();
        }
        else
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);

            if (transform.position.x <= pointA.position.x)
                Flip();
        }
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