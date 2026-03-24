using System.Collections;
using UnityEngine;

public class SpringPad2D : MonoBehaviour
{
    [Header("Force")]
    public float bounceForce = 15f;

    [Header("Collider")]
    public BoxCollider2D boxCollider;
    public float compressedHeight = 0.2f;
    private float originalHeight;

    [Header("Animation")]
    public Animator animator;

    [Header("Timing")]
    public float compressTime = 0.1f;
    public float releaseTime = 0.1f;

    private bool isTriggering = false;

    void Start()
    {
        if (boxCollider == null)
            boxCollider = GetComponent<BoxCollider2D>();

        originalHeight = boxCollider.size.y;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isTriggering) return;

        if (!collision.gameObject.CompareTag("Player")) return;

        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

        if (rb.velocity.y > 0) return;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y < -0.5f)
            {
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();

                if (player != null)
                {
                    StartCoroutine(DoSpring(player));
                }
                break;
            }
        }
    }

    IEnumerator DoSpring(PlayerController player)
    {
        isTriggering = true;

        animator.SetTrigger("Compress");
        SetColliderHeight(compressedHeight);

        yield return new WaitForSeconds(compressTime);

        player.Bounce(bounceForce);

        animator.SetTrigger("Release");
        SetColliderHeight(originalHeight);

        yield return new WaitForSeconds(releaseTime);

        isTriggering = false;
    }

    void SetColliderHeight(float height)
    {
        Vector2 size = boxCollider.size;
        size.y = height;
        boxCollider.size = size;

        Vector2 offset = boxCollider.offset;
        offset.y = height / 2f;
        boxCollider.offset = offset;
    }
}