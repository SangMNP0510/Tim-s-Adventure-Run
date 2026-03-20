using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(AudioSource))]
public class ButtonSFX : MonoBehaviour, IPointerDownHandler
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (AudioGlobalManager.Instance == null)
            return;

        if (AudioGlobalManager.Instance.GetSoundState())
        {
            audioSource.PlayOneShot(audioSource.clip);
        }
    }
}