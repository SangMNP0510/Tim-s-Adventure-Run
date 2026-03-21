using UnityEngine;

public class Bomb : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private Collider2D col;

    private bool isExploding = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isExploding) return;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
            Explode();
            return;
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            Explode();
        }
    }

    void Explode()
    {
        if (isExploding) return;

        isExploding = true;

        rb.velocity = Vector2.zero;
        rb.simulated = false;
        col.enabled = false;

        animator.Play("ExBomb");

        Destroy(gameObject, 0.5f);
    }
}