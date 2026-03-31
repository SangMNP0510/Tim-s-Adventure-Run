using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusBlock : MonoBehaviour
{
    public Sprite usedSprite;

    private bool used = false;
    private Animator animator;
    private SpriteRenderer sr;
    [SerializeField] private GameObject skillPrefab;
    [SerializeField] private Transform spawnPoint;

    void Start()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (used) return;

        if (!collision.gameObject.CompareTag("Player")) return;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                HitBlock();
                break;
            }
        }
    }

    void HitBlock()
    {
        used = true;
        AchievementManager.Instance.AddProgress("block");

        animator.enabled = false;
        sr.sprite = usedSprite;

        if (skillPrefab != null)
        {
            GameObject skill = Instantiate(skillPrefab, spawnPoint.position, Quaternion.identity);

            Rigidbody2D rb = skill.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = new Vector2(0, 5f);
            }
        }
    }
}