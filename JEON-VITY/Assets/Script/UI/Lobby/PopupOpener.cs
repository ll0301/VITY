using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupOpener : MonoBehaviour
{
    public GameObject popup;
    public Text message;


    public void OpenPopup()
    {
        if (popup != null)
        {
            popup.SetActive(true);

        }
    }

    public void ClosePopup()
    {
        if (popup != null)
        {
            popup.SetActive(false);
        }
    }

}
