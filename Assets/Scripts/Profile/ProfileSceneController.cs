using UnityEngine;

public class ProfileSceneController : MonoBehaviour
{
    public GameObject profileUIPanel;

    void Start()
    {
        ShowUI();
    }

    void ShowUI()
    {
        if (profileUIPanel != null)
            profileUIPanel.SetActive(true);
    }
}