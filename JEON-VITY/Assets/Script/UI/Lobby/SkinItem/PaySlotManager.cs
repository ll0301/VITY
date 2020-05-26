using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PaySlotManager : MonoBehaviour
{

    public GameObject payslot1;
    public GameObject payslot2;
    public GameObject payslot3;
    public GameObject payslot4;

    public string username;
    public string item1;
    public int count;
    private void Awake()
    {
        // 유저의 이름 셋팅 
        var lm = GameObject.Find("Lobby/LobbyBox/LobbyManager").GetComponent<LobbyManager>();
        username = lm.clientName;
        item1 = "item1";
        count = 0;
    }

    private void Start()
    {
        PaySlotCheck();
    }

    public void PaySlotCheck()
    {
        StartCoroutine(PaymentCheck());
    }

    // 승수 가져오기 코루틴
    IEnumerator PaymentCheck()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        if(count==0)
        form.AddField("item", "item1" );
        if (count == 1)
            form.AddField("item", "item2");
        if (count == 2)
            form.AddField("item", "item3");
        if (count == 3)
            form.AddField("item", "item4");
        //Debug.Log("실행함");
        using (UnityWebRequest www = UnityWebRequest.Post("http://15.164.101.199/paymentcheck.php", form))
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
                int check = Convert.ToInt32(www.downloadHandler.text);

                if (count == 0)
                {
                    if (check == 0)
                        payslot1.SetActive(true);
                    else if (check == 1)
                        payslot1.SetActive(false);
                    count++;
                    StartCoroutine(PaymentCheck());
                }
                else if(count == 1)
                {
                    if (check == 0)
                        payslot2.SetActive(true);
                    else if (check == 1)
                        payslot2.SetActive(false);
                    count++;
                    StartCoroutine(PaymentCheck());
                }
                else if (count == 2)
                {
                    if (check == 0)
                        payslot3.SetActive(true);
                    else if (check == 1)
                        payslot3.SetActive(false);
                    count++;
                    StartCoroutine(PaymentCheck());
                }
                else if (count == 3)
                {
                    if (check == 0)
                        payslot4.SetActive(true);
                    else if (check == 1)
                        payslot4.SetActive(false);
                    count = 0;
                }
            }
        }
    }

}
