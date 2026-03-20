using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    public TextMeshProUGUI powerText;
    public TextMeshProUGUI shieldText;
    public TextMeshProUGUI bombText;

    public Button powerBtn;
    public Button shieldBtn;
    public Button bombBtn;

    void Update()
    {
        int power = SkillManager.Instance.powerCount;
        int shield = SkillManager.Instance.shieldCount;
        int bomb = SkillManager.Instance.bombCount;

        powerText.text = power.ToString();
        shieldText.text = shield.ToString();
        bombText.text = bomb.ToString();

        SetUIState(powerBtn, powerText, power > 0);
        SetUIState(shieldBtn, shieldText, shield > 0);
        SetUIState(bombBtn, bombText, bomb > 0);
    }

    void SetUIState(Button btn, TextMeshProUGUI text, bool isActive)
    {
        if (btn == null || text == null) return;

        float alpha = isActive ? 1f : 0.8f;

        Image icon = btn.image;
        Color iconColor = icon.color;
        iconColor.a = alpha;
        icon.color = iconColor;

        Color textColor = text.color;
        textColor.a = alpha;
        text.color = textColor;

        btn.interactable = isActive;
    }
}