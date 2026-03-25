using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ProfileUIController : MonoBehaviour
{
    [Header("UI")]
    public TMP_InputField nameInput;
    public Image previewAvatar;
    public Button okButton;

    [Header("Avatar List")]
    public AvatarItem[] avatars;

    [Header("Scene")]
    public string mainMenuSceneName = "MainMenu";

    private int selectedAvatarIndex = -1;
    private int currentSelected = -1;

    void Start()
    {
        SetupAvatars();

        okButton.onClick.AddListener(OnClickOK);
        okButton.interactable = false;
    }

    void SetupAvatars()
    {
        for (int i = 0; i < avatars.Length; i++)
        {
            int index = i;

            avatars[i].button.onClick.RemoveAllListeners();
            avatars[i].button.onClick.AddListener(() =>
            {
                SelectAvatar(index, avatars[index].image.sprite);
            });
        }
    }

    void SelectAvatar(int index, Sprite sprite)
    {
        selectedAvatarIndex = index;
        previewAvatar.sprite = sprite;

        if (currentSelected >= 0)
        {
            avatars[currentSelected].button.transform.localScale = Vector3.one;
        }

        avatars[index].button.transform.localScale = Vector3.one * 1.2f;
        currentSelected = index;

        Validate();
    }

    public void OnNameChanged()
    {
        Validate();
    }

    void Validate()
    {
        bool valid = !string.IsNullOrEmpty(nameInput.text) && selectedAvatarIndex >= 0;
        okButton.interactable = valid;
    }

    void OnClickOK()
    {
        string name = nameInput.text;

        PlayerProfileManager.Instance.SaveProfile(name, selectedAvatarIndex);

        if (PlayerInformationService.Instance != null)
        {
            _ = PlayerInformationService.Instance.SavePlayer();
        }

        SceneManager.LoadScene(mainMenuSceneName);
    }
}