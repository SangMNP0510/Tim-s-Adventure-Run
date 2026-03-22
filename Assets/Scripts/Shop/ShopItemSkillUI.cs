using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemSkillUI : MonoBehaviour
{
    public Image icon;
    public TMP_Text titleText;

    public Button buyButton;
    public TMP_Text priceText;

    private string skillType;
    private int price;
    private ShopSkillManager manager;

    public void Setup(string type, Sprite itemIcon, string title, int price, ShopSkillManager manager)
    {
        this.skillType = type;
        this.price = price;
        this.manager = manager;

        icon.sprite = itemIcon;
        titleText.text = title;
        priceText.text = price + " Coins";

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(OnBuyClick);
    }

    void OnBuyClick()
    {
        manager.BuySkill(skillType, price);
    }
}