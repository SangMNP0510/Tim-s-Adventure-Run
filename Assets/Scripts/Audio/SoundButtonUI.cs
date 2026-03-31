using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SoundButtonUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;

    [Header("Press Effect")]
    public float pressedScale;
    public float darkenAmount;

    private Image buttonImage;
    private Vector3 originalScale;
    private Color originalColor;

    void Start()
    {
        buttonImage = GetComponent<Image>();
        originalScale = transform.localScale;
        originalColor = buttonImage.color;

        UpdateIcon();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.localScale = originalScale * pressedScale;

        buttonImage.color = originalColor * darkenAmount;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localScale = originalScale;
        buttonImage.color = originalColor;

        bool currentState = AudioGlobalManager.Instance.GetSoundState();
        AudioGlobalManager.Instance.SetSound(!currentState);

        UpdateIcon();
    }

    void UpdateIcon()
    {
        if (AudioGlobalManager.Instance.GetSoundState())
            buttonImage.sprite = soundOnSprite;
        else
            buttonImage.sprite = soundOffSprite;
    }
}