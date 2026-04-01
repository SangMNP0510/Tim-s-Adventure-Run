using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementItemUI : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text progressText;
    public TMP_Text rewardText;
    public Button claimButton;

    private AchievementData data;

    public void Setup(AchievementData ach)
    {
        data = ach;

        nameText.text = ach.name;
        progressText.text = ach.current + "/" + ach.target;
        rewardText.text = ach.reward.ToString();

        if (ach.isClaimed)
        {
            claimButton.gameObject.SetActive(true);
            claimButton.interactable = false;
            claimButton.GetComponentInChildren<TMP_Text>().text = "Claimed";
            claimButton.image.color = Color.gray;
        }
        else if (ach.isCompleted)
        {
            claimButton.gameObject.SetActive(true);
            claimButton.interactable = true;
            claimButton.GetComponentInChildren<TMP_Text>().text = "Claim";
            claimButton.image.color = Color.white;
        }
        else
        {
            claimButton.gameObject.SetActive(false);
        }

        claimButton.onClick.RemoveAllListeners();
        claimButton.onClick.AddListener(OnClaim);
    }

    void OnClaim()
    {
        AchievementManager.Instance.Claim(data.id);

        claimButton.interactable = false;
        claimButton.GetComponentInChildren<TMP_Text>().text = "Claimed";
    }
}