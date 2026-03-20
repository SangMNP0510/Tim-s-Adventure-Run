using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillController : MonoBehaviour
{
    private PlayerController player;
    private SpriteRenderer sr;

    [Header("Shield Armor")]
    public GameObject armorObject;

    [Header("Bomb")]
    public GameObject bombPrefab;
    public Transform bombSpawnPoint;
    [SerializeField] private float throwDelay = 0.3f;

    private float lastThrowTime;

    private bool isPowerActive;
    private bool isShieldActive;

    private int readyBombs = 0;

    private void Awake()
    {
        player = GetComponent<PlayerController>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void UsePower()
    {
        if (!SkillManager.Instance.UseSkill("Power")) return;

        StartCoroutine(PowerSkill());
    }

    IEnumerator PowerSkill()
    {
        isPowerActive = true;

        float time = 5f;

        while (time > 0)
        {
            transform.localScale = Vector3.one * 1.2f;
            yield return new WaitForSeconds(0.2f);

            transform.localScale = Vector3.one;
            yield return new WaitForSeconds(0.2f);

            time -= 0.4f;
        }

        transform.localScale = Vector3.one;
        isPowerActive = false;
    }

    public IEnumerator PowerInvincible()
    {
        for (int i = 0; i < 5; i++)
        {
            sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(0.1f);
        }
        sr.enabled = true;
    }

    public bool IsPowerActive() => isPowerActive;

    public void UseShield()
    {
        if (!SkillManager.Instance.UseSkill("Shield")) return;

        StartCoroutine(ShieldSkill());
    }

    IEnumerator ShieldSkill()
    {
        isShieldActive = true;

        if (armorObject != null)
            armorObject.SetActive(true);

        yield return new WaitForSeconds(5f);

        if (armorObject != null)
            armorObject.SetActive(false);

        isShieldActive = false;
    }

    public bool IsShieldActive() => isShieldActive;

    public void UseBomb()
    {
        if (!SkillManager.Instance.UseSkill("Bomb")) return;

        readyBombs++;
    }

    public void ThrowBomb()
    {
        if (readyBombs <= 0) return;

        if (Time.time - lastThrowTime < throwDelay) return;

        lastThrowTime = Time.time;
        readyBombs--;

        GameObject bomb = Instantiate(bombPrefab, bombSpawnPoint.position, Quaternion.identity);

        Rigidbody2D rb = bomb.GetComponent<Rigidbody2D>();

        float dir = transform.localScale.x;
        rb.velocity = new Vector2(dir * 5f, 6f);
    }
}