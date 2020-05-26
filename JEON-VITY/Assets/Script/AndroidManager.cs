using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidManager : MonoBehaviour
{
#if UNITY_ANDROID
    private AndroidJavaObject curActivity;
    public string strLog = "No Java Log";
    static AndroidManager _instance;

    public static AndroidManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = new GameObject("AndroidManager").AddComponent<AndroidManager>();
        }
        return _instance;
    }

    void Awake()
    {
        ///&lt; 현재 활성화된 액티비티 얻어와서 저장
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        curActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
    }


    public void CallJavaFunc(string strFuncName, string strTemp)
    {
        if (curActivity == null)
            return;
        ///&lt; 액티비티안의 자바 메소드 호출
        curActivity.Call(strFuncName, strTemp);
    }
    void SetJavaLog(string strJavaLog)
    {
        strLog = strJavaLog;
    }
#endif
}