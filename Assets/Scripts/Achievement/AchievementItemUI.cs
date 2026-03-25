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

        claimButton.gameObject.SetActive(ach.isCompleted && !ach.isClaimed);
        claimButton.interactable = !ach.isClaimed;

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