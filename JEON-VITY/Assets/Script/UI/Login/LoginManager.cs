using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    public InputField id;
    public InputField pass;
    public GameObject idbox;
    public GameObject gamequitPop;
    public GameObject popup;
    public Text popText;

    private void Awake()
    {
        DontDestroyOnLoad(idbox);
    }

    public void ClosePopup()
    {
        popText.text = "";
        popup.SetActive(false);
    }

    public void LoginButton()
    {
        if (id.text.Equals("") || pass.text.Equals(""))
        {
            popup.SetActive(true);
            popText.text = "아이디와 비밀번호를 입력하세요.";
            return;
        }
        StartCoroutine(Login());
    }
    // 로그인 후 씬 넘어가기 
    IEnumerator Login()
    {

        WWWForm form = new WWWForm();
        form.AddField("username", id.text);
        form.AddField("password", pass.text);
        Debug.Log(id.text);
        Debug.Log(pass.text);

        using (UnityWebRequest www = UnityWebRequest.Post("http://15.164.101.199/login.php", form))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                //Debug.Log(www.downloadHandler.text);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                if (www.downloadHandler.text.Equals("successfully"))
                {
                    idbox.GetComponentInChildren<Text>().text = id.text;
                    SceneManager.LoadScene("NetworkGame", LoadSceneMode.Single);
                }
                else
                {
                    popup.SetActive(true);
                    popText.text = www.downloadHandler.text;
                }
            }
        }
    }

    public void QuitGame()
    {
        gamequitPop.SetActive(true);
    }

    public void QuitOK()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    public void QuitCancel()
    {
        gamequitPop.SetActive(false);
    }


}
