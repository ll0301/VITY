using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RegisterUIManager : MonoBehaviour
{
    [Header("**Login**")]
    public GameObject login;
    public InputField loginNickInput;
    public InputField loginPassInput;

    [Header("**Register**")]
    public GameObject registerPop;
    public InputField regEmailInput; // 이메일 필드 
    public InputField regNickInput; // 닉네임필드 
    public InputField regPassInput; // 패스워드 필드
    public InputField regPassConInput; 
    public GameObject nickObj;
    public Button nickCheck;
    public GameObject emailObj;
    public Button emailCheck;
    public GameObject nickAgain;
    public Button nickAgainButton;
    public GameObject emailAgain;
    public Button emailAgainButton;
    public Button agreeButton;
    public Button submitButton; // 회원가입 완료버튼 

    [Header("**Popup Email Auth**")]
    public GameObject popupAuth;
    public Text authText;
    public Text authText2;
    public InputField authInput;
    

    [Header("**Popup**")]
    public GameObject popup;
    public Text popupText;
    public GameObject agreePop;

    public bool nickBool;
    public bool emailBool;
    public bool agree;

    private void Awake()
    {
        nickBool = false;
        emailBool = false;
        agree = false;
    }

    private void Update()
    {
        //----------------------------------------
        //닉네임과 이메일 각각의 입력창에 문자가 입력되면 
        // 체크버튼이 활성화되고 비어있으면 비활성화된다.
        if (regEmailInput.text.Equals(""))
        {
            emailCheck.interactable = false;
        }
        else
        {
            emailCheck.interactable = true;
        }

        if (!emailBool&&!nickBool)
        {
            nickCheck.interactable = false;
            regNickInput.interactable = false;
            nickAgain.SetActive(false);
            nickBool = false;
            regNickInput.text = "";
            nickObj.SetActive(true);
        }
        else if(emailBool&&!nickBool)
        {
            regNickInput.interactable = true;
            if (regNickInput.text.Equals(""))
            {
                nickCheck.interactable = false;
            }
            else
            {
                nickCheck.interactable = true;
            }
        }


        //-------------------------------------------
        

        // 닉네임과 email 중복확인이 끝나면 비밀번호 인풋이 활성화된다.
        if (emailBool && nickBool &&!agree)
        {
            regPassInput.interactable = true;
            regPassConInput.interactable = true;
        }
        else if (!nickBool)
        {
            regPassInput.text = "";
            regPassConInput.text = "";
            regPassInput.interactable = false;
            regPassConInput.interactable = false;
        }

        // 비밀번호는 8자 이상 
        if (!regPassConInput.text.Equals(""))
        {
            if (regPassInput.text.Length <8)
            {
                popup.SetActive(true);
                popupText.text = "비밀번호는 8자 이상입니다.";
                regPassInput.text = "";
                regPassConInput.text = "";
            }
        }
    }

    // 레지스터 패널 열기 
    public void OnRegisterButton()
    {
        loginNickInput.text = "";
        loginPassInput.text = "";
        login.SetActive(false);
        registerPop.SetActive(true);
    }    

    //레지스터 패널 닫기 
    public void CloseRegisterPanel()
    {
        regEmailInput.text = "";
        regNickInput.text = "";
        regPassInput.text = "";
        regPassConInput.text = "";
        agree = false;
        regEmailInput.interactable = true;
        regNickInput.interactable = true;
        emailBool = false;
        nickBool = false;
        nickObj.SetActive(true);
        emailObj.SetActive(true);
        emailAgainButton.interactable = true;
        nickAgainButton.interactable = true;
        emailAgain.SetActive(false);
        nickAgain.SetActive(false);
        agreeButton.interactable = true;
        submitButton.interactable = false;
        registerPop.SetActive(false);
        login.SetActive(true);
    }

    //가입동의 패널 열기 
    public void OnAgreeButton()
    {
        if (regNickInput.text.Equals("") || regEmailInput.text.Equals("") ||
   regPassInput.text.Equals("") || regPassConInput.text.Equals(""))
        {
            popup.SetActive(true);
            popupText.text = "빈 칸 없이 입력해주세요.";
            return;
        }
        if (!regPassInput.text.Equals(regPassConInput.text)){
            popup.SetActive(true);
            popupText.text = "비밀번호가 일치하지 않습니다.";
            return;
        }

        // Debug.Log(regPassInput.text);
        // Debug.Log(regPassConInput.text);
        agreePop.SetActive(true);
     }

    //가입동의 패널 닫기 
    public void CloseAgreePanel()
    {
        agreePop.SetActive(false);
    }


    // 경고 팝업 닫기
    public void ClosePopup()
    {
        popup.SetActive(false);
    }


    //***************************************************************
    // 닉네임 중복확인 
    public void CheckNicknameButton()
    {
        //-- 서버에서 정보가져오기 구현해야함 
        StartCoroutine(CheckId());
        //--
    }
    IEnumerator CheckId()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", regNickInput.text);
        using (UnityWebRequest www = UnityWebRequest.Post("http://15.164.101.199/checkId.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
               // popupText.text = www.downloadHandler.text;
            }
            else
            {
                //Debug.Log(www.downloadHandler.text);
                popup.SetActive(true);
                popupText.text = www.downloadHandler.text;
                if (popupText.text.Equals("id available"))
                {
                    nickBool = true;
                    regNickInput.interactable = false;
                    nickObj.SetActive(false);
                    nickAgain.SetActive(true);
                }
            }
        }
    }
    // 닉네임 다시입력 
    public void AgainNickButton()
    {
        regNickInput.interactable = true;
        regNickInput.text = "";
        nickObj.SetActive(true);
        nickAgain.SetActive(false);
        nickBool = false;
    }

    // 이메일 중복확인 
    public void CheckEmailButton()
    {
        StartCoroutine(CheckEmail());
    }
    //이메일 중복확인 
    IEnumerator CheckEmail()
    {
        int r1 = Random.Range(10, 99);
        int r2 = Random.Range(10, 99);
        int r3 = Random.Range(10, 99);
        string randomNum = r1.ToString() + r2.ToString() + r3.ToString();
        WWWForm form = new WWWForm();
        form.AddField("email", regEmailInput.text);
        form.AddField("number", randomNum);
        using (UnityWebRequest www = UnityWebRequest.Post("http://15.164.101.199/checkEmail.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                // popupText.text = www.downloadHandler.text;
            }
            else
            {
                //Debug.Log(www.downloadHandler.text);
                if (www.downloadHandler.text.Equals("Email available"))
                {
                    popupAuth.SetActive(true);
                    authText.text = www.downloadHandler.text;
                    authText2.text = "이메일에서 인증번호를 확인하고 입력하세요.";
                    emailBool = true;
                    regEmailInput.interactable = false;
                    emailObj.SetActive(false);
                    emailAgain.SetActive(true);
                    StartCoroutine(SendEmail(randomNum));
                }
                else
                {
                    popup.SetActive(true);
                    popupText.text = www.downloadHandler.text;
                }
            }
        }
    }
    //이메일로 인증번호 보내기 
    IEnumerator SendEmail(string randomNum)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", regEmailInput.text);
        form.AddField("number", randomNum);
        using (UnityWebRequest www = UnityWebRequest.Post("http://15.164.101.199/sendEmail.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                // popupText.text = www.downloadHandler.text;
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        }
    }
    // 이메일 다시입력하기 
    public void AgainEmailButton()
    {
        regEmailInput.interactable = true;
        regEmailInput.text = "";
        emailObj.SetActive(true);
        emailAgain.SetActive(false);
        emailBool = false;
    }
    //***************************************************************

    // 인증번호확인 
    public void CheckAuthNumber()
    {
        StartCoroutine(CheckAuth());
    }

    IEnumerator CheckAuth()
    {
        WWWForm form = new WWWForm();
        form.AddField("email", regEmailInput.text);
        form.AddField("number", authInput.text);
        using (UnityWebRequest www = UnityWebRequest.Post("http://15.164.101.199/checkAuth.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                // popupText.text = www.downloadHandler.text;
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                if (www.downloadHandler.text.Equals("\r\nsuccessfully"))
                {
                    popupAuth.SetActive(false);
                    authText.text = "";
                    popup.SetActive(true);
                    popupText.text = "인증번호가 일치합니다.";
                }
                else
                {
                    popup.SetActive(true);
                    popupText.text = "인증번호가 일치하지 않습니다.";
                }
             }
        }
    }

    // 인증없이 닫으면 임시로 저장해둔 이메일 지움 
    public void CloseAuthPanel()
    {
        StartCoroutine(CloseAuth());
    }

    IEnumerator CloseAuth()
    {
        WWWForm form = new WWWForm();
        form.AddField("email", regEmailInput.text);
        using (UnityWebRequest www = UnityWebRequest.Post("http://15.164.101.199/removeAuth.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                // popupText.text = www.downloadHandler.text;
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                authText.text = "";
                popupAuth.SetActive(false);
                emailBool = false;
                emailAgain.SetActive(false);
                emailObj.SetActive(true);
                regEmailInput.interactable = true;
                regEmailInput.text = "";
            }
        }
    }

    //회원가입 완료버튼을 누르면 
    public void RegisterSubmit()
    {
        StartCoroutine(Register());
    }

    IEnumerator Register()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", regNickInput.text);
        form.AddField("email", regEmailInput.text);
        form.AddField("password", regPassInput.text);
       // Debug.Log("username"+regNickInput.text);
       // Debug.Log("email"+regEmailInput.text);
       // Debug.Log("password"+regPassInput.text);
        using (UnityWebRequest www = UnityWebRequest.Post("http://15.164.101.199/register.php", form)){
            yield return www.SendWebRequest();

            if(www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                CloseRegisterPanel();
                agreePop.SetActive(false);
                popup.SetActive(true);
                popupText.text = "회원가입이 완료되었습니다.";
            }
        }
    }


}
