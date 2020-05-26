using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HeartCountManager : MonoBehaviour
{
    public GameObject lobby;
    public Text heartCount;

    public string username;

    // Start is called before the first frame update
    void Start()
    {
        // 유저의 이름 셋팅 
        var lm = GameObject.Find("Lobby/LobbyBox/LobbyManager").GetComponent<LobbyManager>();
        username = lm.clientName;
        StartCoroutine(GetUserHeart());
    }



     // 승수 가져오기 코루틴
    IEnumerator GetUserHeart()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        using (UnityWebRequest www = UnityWebRequest.Post("http://15.164.101.199/getheart.php", form))
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
               heartCount.text = www.downloadHandler.text;
                lobby.GetComponent<SkinManager>().heartCount = Convert.ToInt32(heartCount.text);
            }
        }
    }


    public void HeartUpdate()
    {
        StartCoroutine(GetUserHeart());
    }
}
