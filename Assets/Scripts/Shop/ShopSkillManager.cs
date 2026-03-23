using UnityEngine;

public class ShopSkillManager : MonoBehaviour
{
    public ShopItemSkillUI itemPrefab;
    public Transform content;

    public Sprite powerIcon;
    public Sprite bombIcon;
    public Sprite shieldIcon;

    public GameObject notEnoughPopup;
    public GameObject successPopup;

    void Start()
    {
        CreateSkill("Power", powerIcon, "Power", 200);
        CreateSkill("Bomb", bombIcon, "Bomb", 300);
        CreateSkill("Shield", shieldIcon, "Shield", 250);
    }

    void CreateSkill(string type, Sprite icon, string title, int price)
    {
        ShopItemSkillUI item = Instantiate(itemPrefab, content);
        item.Setup(type, icon, title, price, this);
    }

    public void BuySkill(string type, int price)
    {
        if (CoinManager.Instance.CurrentCoins < price)
        {
            Debug.Log("Không đủ coin");
            if (notEnoughPopup != null)
                notEnoughPopup.SetActive(true);
            return;
        }

        CoinManager.Instance.SpendCoins(price);

        SkillManager.Instance.AddSkill(type, 1);

        Debug.Log("Mua thành công: " + type);

        if (successPopup != null)
            successPopup.SetActive(true);
    }
}