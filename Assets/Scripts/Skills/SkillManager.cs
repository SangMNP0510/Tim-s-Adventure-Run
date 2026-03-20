using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;

    public int powerCount;
    public int shieldCount;
    public int bombCount;

    private void Awake()
    {
        Instance = this;
    }

    public void AddSkill(string type, int amount)
    {
        switch (type)
        {
            case "Power": powerCount += amount; break;
            case "Shield": shieldCount += amount; break;
            case "Bomb": bombCount += amount; break;
        }
    }

    public bool UseSkill(string type)
    {
        switch (type)
        {
            case "Power":
                if (powerCount > 0)
                {
                    powerCount--;
                    return true;
                }
                break;

            case "Shield":
                if (shieldCount > 0)
                {
                    shieldCount--;
                    return true;
                }
                break;

            case "Bomb":
                if (bombCount > 0)
                {
                    bombCount--;
                    return true;
                }
                break;
        }

        return false;
    }
}