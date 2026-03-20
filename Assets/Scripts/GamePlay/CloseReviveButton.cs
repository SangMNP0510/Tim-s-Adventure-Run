using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseReviveButton : MonoBehaviour
{
    public void OnClickClose()
    {
        GameManager.Instance.ShowLose();
    }
}