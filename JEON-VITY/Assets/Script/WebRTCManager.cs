using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebRTCManager : MonoBehaviour
{

    private AndroidJavaObject UnityActivity;
    private AndroidJavaObject UnityInstance;

    private void Start()
    {
        AndroidJavaClass ajc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        UnityActivity = ajc.GetStatic<AndroidJavaObject>("currentActivity");

        AndroidJavaClass ajc2 = new AndroidJavaClass("com.example.webrtcandroidsampleapp.WebRtcActivity");
    }

}
