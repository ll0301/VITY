using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateGameManager : MonoBehaviour
{
    public GameObject popup;
    public Toggle onetoone;
    public Toggle otoText;
    public Toggle groupMatch;
    public Toggle gmText;
    public Toggle privateRoom;
    public Toggle prText;
    public InputField roomPass;

   public bool privateOnOff;
    public bool groupCheck;

    private void Start()
    {
        privateOnOff = false;
    }

    // 1대1매치 토글
    public void OnOnToOne()
    {
        if (groupCheck)
        {
            onetoone.isOn = true;
            groupMatch.isOn = false;
            otoText.interactable = true;
            gmText.interactable = false;
            groupCheck = false;
        }
    }

    // 단체전 토글
    public void OnGroupMatch()
    {
        if (!groupCheck)
        {
            onetoone.isOn = false;
            groupMatch.isOn = true;
            otoText.interactable = false;
            gmText.interactable = true;
            groupCheck = true;
        }
    }

    //비공개룸 토글 
    public void OnPrivateRoom()
    {
        if (!privateOnOff)
        {
            privateRoom.isOn = true;
            prText.interactable = true;
            roomPass.interactable = true;
            privateOnOff = true;
            return;
        }
        if (privateOnOff)
        {
            privateRoom.isOn = false;
            prText.interactable = false;
            roomPass.interactable = false;
            privateOnOff = false;
            return;
        }

    }


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
            GameObject.Find("InputField_roomname").GetComponent<InputField>().text = "";
            GameObject.Find("InputField_roompasswd").GetComponent<InputField>().text = "";
            popup.SetActive(false);
        }
    }
}
