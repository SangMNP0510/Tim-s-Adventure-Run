using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ProfileUIController : MonoBehaviour
{
    [Header("UI")]
    public TMP_InputField nameInput;
    public Image previewAvatar; // FrameAv
    public Button okButton;

    [Header("Avatar Root")]
    public Transform avatarContainer; // Content

    [Header("Scene")]
    public string mainMenuSceneName = "MainMenu";

    private int selectedAvatarIndex = -1;

    void Start()
    {
        SetupAvatars();

        okButton.onClick.AddListener(OnClickOK);
        okButton.interactable = false;
    }

    void SetupAvatars()
    {
        int index = 0;

        foreach (Transform child in avatarContainer.GetComponentsInChildren<Transform>())
        {
            Button btn = child.GetComponent<Button>();

            if (btn != null && child.name.Contains("Frame_Btn"))
            {
                Image img = child.Find("Av")?.GetComponent<Image>();

                if (img == null) continue;

                int currentIndex = index;

                btn.onClick.AddListener(() =>
                {
                    SelectAvatar(currentIndex, img.sprite);
                });

                index++;
            }
        }
    }

    void SelectAvatar(int index, Sprite sprite)
    {
        selectedAvatarIndex = index;
        previewAvatar.sprite = sprite;

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

        SceneManager.LoadScene(mainMenuSceneName);
    }
}