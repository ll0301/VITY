using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyAlert : MonoBehaviour
{
    //alert 
    public GameObject alert;
    public Text alertText;

    //경고 알림창 
    public void OpenAlert()
    {
        alertText.text = "인원수 초과입니다.";

        if (alert != null)
        {
            alert.SetActive(true); // 알림창을 띄운다. 
        }
        
    }

    public void GameAlert()
    {
        alertText.text = "게임진행중인 방입니다.";

        if (alert != null)
        {
            alert.SetActive(true); // 알림창을 띄운다. 
        }
    }


    // 비밀번호 틀렸을때 뜨는 경고알림창을 끄는 메소드 
    public void CloseAlert()
    {
        var lo = GameObject.Find("LobbyManager").GetComponent<LobbyManager>();
        if (alert != null)
        {
            alert.SetActive(false);
        }
        lo.CloseRoomPanel();
    }
}
