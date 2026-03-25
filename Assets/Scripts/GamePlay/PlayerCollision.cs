using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private GameManager gameManager;
    private Rigidbody2D rb;
    private PlayerController player;
    private PlayerSkillController skill;

    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<PlayerController>();
        skill = GetComponent<PlayerSkillController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            if (!collision.gameObject.activeInHierarchy) return;

            if (ScoreService.Instance != null)
            {
                ScoreService.Instance.AddScore(ScoreEventType.Coin);
            }
            else
            {
                gameManager.AddScore(50);
            }
            gameManager.AddCoin(1);

            CollectCoinEffect(collision.gameObject);
        }
        else if (collision.CompareTag("Trap"))
        {
            player.Die(false);
            StartCoroutine(DelayGameOver());
        }
    }

    void CollectCoinEffect(GameObject coin)
    {
        Collider2D col = coin.GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        SpriteRenderer sr = coin.GetComponent<SpriteRenderer>();
        if (sr != null) sr.enabled = false;

        Destroy(coin, 0.2f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Enemy")) return;

        float playerY = transform.position.y;
        float enemyY = collision.transform.position.y;

        if (skill != null && skill.IsShieldActive())
        {
            Destroy(collision.gameObject);
            return;
        }

        if (skill != null && skill.IsPowerActive())
        {
            StartCoroutine(skill.PowerInvincible());
            return;
        }

        Snail snail = collision.gameObject.GetComponent<Snail>();
        if (snail != null)
        {
            if (snail.IsRolling())
            {
                player.Die(true);
                StartCoroutine(DelayGameOver());
                return;
            }

            if (!snail.IsDead())
            {
                if (playerY > enemyY + 0.5f && rb.velocity.y <= 0)
                {
                    snail.Die();
                    player.Bounce(10f);
                    ScoreService.Instance.AddScore(ScoreEventType.Enemy);
                    AchievementManager.Instance.AddProgress("kill");
                    return;
                }
            }

            player.Die(true);
            StartCoroutine(DelayGameOver());
            return;
        }

        Beetle beetle = collision.gameObject.GetComponent<Beetle>();
        if (beetle != null)
        {
            if (!beetle.IsDead())
            {
                if (playerY > enemyY + 0.5f && rb.velocity.y <= 0)
                {
                    beetle.Die();
                    player.Bounce(8f);
                    ScoreService.Instance.AddScore(ScoreEventType.Enemy);
                    AchievementManager.Instance.AddProgress("kill");
                    return;
                }
            }

            player.Die(true);
            StartCoroutine(DelayGameOver());
        }
    }

    IEnumerator DelayGameOver()
    {
        yield return new WaitForSeconds(1f);
        gameManager.GameOver();
    }
}