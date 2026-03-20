using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosePopupButton : MonoBehaviour
{
    public GameObject popup;

    public void ClosePopup()
    {
        popup.SetActive(false);
    }
}