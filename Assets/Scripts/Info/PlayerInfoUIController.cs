using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInfoUIController : MonoBehaviour
{
    [Header("Main UI")]
    public TMP_Text nameText;
    public Image avatarImage;

    [Header("Avatar Sprites")]
    public Sprite[] avatarSprites;

    [Header("Name Popup")]
    public GameObject namePopup;
    public TMP_InputField nameInput;
    public Button nameOkButton;

    [Header("Avatar Popup")]
    public GameObject avatarPopup;
    public Image previewAvatar;
    public Button avatarOkButton;
    public AvatarItem[] avatars;

    private int selectedAvatarIndex = -1;
    private int currentSelected = -1;

    void Start()
    {
        LoadCurrentInfo();
        SetupAvatarButtons();

        namePopup.SetActive(false);
        avatarPopup.SetActive(false);

        nameOkButton.onClick.AddListener(OnConfirmName);
        avatarOkButton.onClick.AddListener(OnConfirmAvatar);
    }

    void LoadCurrentInfo()
    {
        var profile = PlayerProfileManager.Instance;

        nameText.text = profile.playerName;

        int avatarIndex = profile.avatarIndex;
        if (avatarIndex >= 0 && avatarIndex < avatarSprites.Length)
        {
            avatarImage.sprite = avatarSprites[avatarIndex];
        }
    }

    public void OpenNamePopup()
    {
        namePopup.SetActive(true);
        nameInput.text = PlayerProfileManager.Instance.playerName;
    }

    public void OpenAvatarPopup()
    {
        avatarPopup.SetActive(true);

        int currentIndex = PlayerProfileManager.Instance.avatarIndex;
        previewAvatar.sprite = avatarSprites[currentIndex];

        selectedAvatarIndex = currentIndex;
        currentSelected = currentIndex;
    }

    void OnConfirmName()
    {
        string newName = nameInput.text.Trim();

        if (string.IsNullOrEmpty(newName))
            return;

        int avatarIndex = PlayerProfileManager.Instance.avatarIndex;

        PlayerProfileManager.Instance.SaveProfile(newName, avatarIndex);

        if (PlayerInformationService.Instance != null)
        {
            _ = PlayerInformationService.Instance.SavePlayer();
        }

        nameText.text = newName;

        namePopup.SetActive(false);
    }

    void SetupAvatarButtons()
    {
        for (int i = 0; i < avatars.Length; i++)
        {
            int index = i;

            avatars[i].button.onClick.RemoveAllListeners();
            avatars[i].button.onClick.AddListener(() =>
            {
                SelectAvatar(index);
            });
        }
    }

    void SelectAvatar(int index)
    {
        selectedAvatarIndex = index;
        previewAvatar.sprite = avatarSprites[index];

        if (currentSelected >= 0)
        {
            avatars[currentSelected].button.transform.localScale = Vector3.one;
        }

        avatars[index].button.transform.localScale = Vector3.one * 1.2f;
        currentSelected = index;
    }

    void OnConfirmAvatar()
    {
        if (selectedAvatarIndex < 0) return;

        string name = PlayerProfileManager.Instance.playerName;

        PlayerProfileManager.Instance.SaveProfile(name, selectedAvatarIndex);

        if (PlayerInformationService.Instance != null)
        {
            _ = PlayerInformationService.Instance.SavePlayer();
        }

        avatarImage.sprite = avatarSprites[selectedAvatarIndex];

        avatarPopup.SetActive(false);
    }
}