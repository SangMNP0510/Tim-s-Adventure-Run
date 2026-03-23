using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillItem : MonoBehaviour
{
    public string skillType;
    public int amount = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        SkillManager.Instance.AddSkill(skillType, amount);

        Destroy(gameObject);
    }
}