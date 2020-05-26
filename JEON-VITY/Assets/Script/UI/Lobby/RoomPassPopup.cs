using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomPassPopup : MonoBehaviour
{
    public GameObject popup;
    public GameObject alert;
    public InputField password;
    public Text alertText;
    public string owner;
    public string pass;


    // 방으로 들어가기   
    public void InRoom()
    {
        // 패스워드가 맞으면 
        if (pass.Equals(password.text.ToString()))
        {
            var lo = GameObject.Find("LobbyManager").GetComponent<LobbyManager>();
            lo.InGameRoom(owner);
            ClosePopup(); // 비밀번호가 맞으면 방으로 입장한다. 
        }
        else
        {
            OpenAlert(); // 비밀번호가 틀리면 경고창이뜬다. 
        }
            
    }

    //비밀번호창 열기 
    public void OpenPopup(string _owner, string _pass)
    {
        if (popup != null)
        {
            popup.SetActive(true);
        }
        owner = _owner;  
        pass = _pass;
    }

    //경고 알림창 
    public void OpenAlert()
    {
        alertText.text = "비밀번호가 틀렸습니다.";

        if (alert != null)
        {
            alert.SetActive(true); // 알림창을 띄운다. 
        }
        
    }

    // 비밀번호 틀렸을때 뜨는 경고알림창을 끄는 메소드 
    public void CloseAlert()
    {
        if (alert != null)
        {
            alert.SetActive(false);
        }
        ClosePopup(); //알림창을 끌 때 비밀번호 입력창까지 전부 꺼버린다. 
    }

    //비밀번호창 끄기 
    public void ClosePopup()
    {
        if (popup != null)
        {
            popup.SetActive(false);
        }
        owner = "";
        pass = "";
        password.text = "";
    }
}
