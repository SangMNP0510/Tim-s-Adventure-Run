using UnityEngine;

public class AchievementUI : MonoBehaviour
{
    public Transform content;
    public GameObject prefab;

    void Start()
    {
        Load();
    }

    void Load()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        foreach (var ach in AchievementManager.Instance.achievements)
        {
            GameObject go = Instantiate(prefab, content);
            go.GetComponent<AchievementItemUI>().Setup(ach);
        }
    }
}