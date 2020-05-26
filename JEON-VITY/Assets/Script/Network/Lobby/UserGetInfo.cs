using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UserGetInfo : MonoBehaviour
{
    public GameObject lobby;

    public Text nameText;
    public Text winText;
    public Text loseText;

    public GameObject level1;
    public GameObject level2;
    public GameObject level3;
    public GameObject level4;
    public GameObject level5;
    public bool update;

    private void Start()
    {
        // 유저의 이름 셋팅 
       var lm = GameObject.Find("Lobby/LobbyBox/LobbyManager").GetComponent<LobbyManager>();
        nameText.text = lm.clientName;
        StartCoroutine(GetUserWin());
        StartCoroutine(GetUserLose());
    }
    public void Update()
    {
        var lm = GameObject.Find("Lobby/LobbyBox/LobbyManager").GetComponent<LobbyManager>();
        if (!lm.playerData.startGame)
        {
            if (update)
            {
                update = false;
                StartCoroutine(GetUserWin());
                StartCoroutine(GetUserLose());
            }
        }
    }

    // 승수 가져오기 코루틴
    IEnumerator GetUserWin()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", nameText.text);
        using (UnityWebRequest www = UnityWebRequest.Post("http://15.164.101.199/getwin.php", form))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                // popupText.text = www.downloadHandler.text;
            }
            else
            {
               // Debug.Log(www.downloadHandler.text);
                winText.text = www.downloadHandler.text;
                LevelCheck();
            }
        }
    }
    //패수 가져오기 코루틴 
    IEnumerator GetUserLose()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", nameText.text);
        using (UnityWebRequest www = UnityWebRequest.Post("http://15.164.101.199/getlose.php", form))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                // popupText.text = www.downloadHandler.text;
            }
            else
            {
               // Debug.Log(www.downloadHandler.text);
                loseText.text = www.downloadHandler.text;
            }
        }
    }

    //레벨 계산 
    public void LevelCheck()
    {
        int w = Convert.ToInt32(winText.text);
        //int l = Convert.ToInt32(loseText.text);

        if (w < 20)
        {
            level1.SetActive(true);
            lobby.GetComponent<SkinManager>().level = 1;
        }
        else if (w  >= 20 && w < 40)
        {
            level2.SetActive(true);
            lobby.GetComponent<SkinManager>().level = 2;
        }
        else if (w >= 40 && w < 60)
        {
            level3.SetActive(true);
            lobby.GetComponent<SkinManager>().level = 3;
        }
        else if (w >= 60 && w < 80)
        {
            level4.SetActive(true);
            lobby.GetComponent<SkinManager>().level = 4;
        }
        else if (w >= 80 && w < 1000)
        {
            level5.SetActive(true);
            lobby.GetComponent<SkinManager>().level = 5;
        }
    }

    private void OnDisable()
    {
        update = true;
    }
}
