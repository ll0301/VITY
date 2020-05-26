using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PaymentManager : MonoBehaviour
{
    public GameObject pop;
    public GameObject head1;
    public Text headText1;
    public GameObject head2;
    public Text headText2;
    public GameObject head3;
    public Text headText3;
    public GameObject body1;
    public Text bodyText1;

    public GameObject payHeart;
    public GameObject roomHeart;
    public GameObject paySlot;
    public GameObject character;

    public Text minusHeart;
    public Text Message;

    public string itemname;
    public int item;
    public int your;

    public Text yourHeart;

    public Button OK;

    public string username;
    public string itemNumber;

    private void Start()
    {
        // 유저의 이름 셋팅 
        var lm = GameObject.Find("Lobby/LobbyBox/LobbyManager").GetComponent<LobbyManager>();
        username = lm.clientName;
    }

    public void PopupStart()
    {
       // Debug.Log("시작");
        if (itemname.Equals("Head1"))
        {
            character.SetActive(false);
            head1.SetActive(true);
            head2.SetActive(false);
            head3.SetActive(false);
            body1.SetActive(false);

            minusHeart.text = yourHeart.text;
            itemNumber = "item1";
            item = Convert.ToInt32(headText1.text);
            your = Convert.ToInt32(yourHeart.text);
            if (item < your)
            {
                OK.interactable = true;
                Message.text = "Would you like to purchase?";
            }
            else if (item > your)
            {
                OK.interactable = false;
                Message.text = "There is not enough heart.";
            }

        }
        else if (itemname.Equals("Head2"))
        {
            character.SetActive(false);
            head1.SetActive(false);
            head2.SetActive(true);
            head3.SetActive(false);
            body1.SetActive(false);
            minusHeart.text = yourHeart.text;
            itemNumber = "item2";
            item = Convert.ToInt32(headText2.text);
            your = Convert.ToInt32(yourHeart.text);

            if (item < your)
            {
                OK.interactable = true;
                Message.text = "Would you like to purchase?";
            }
            else if (item > your)
            {
                OK.interactable = false;
                Message.text = "There is not enough heart.";
            }
        }
        else if (itemname.Equals("Head3"))
        {
            character.SetActive(false);
            head1.SetActive(false);
            head2.SetActive(false);
            head3.SetActive(true);
            body1.SetActive(false);
            minusHeart.text = yourHeart.text;
            itemNumber = "item3";
            item = Convert.ToInt32(headText3.text);
            your = Convert.ToInt32(yourHeart.text);

            if (item < your)
            {
                OK.interactable = true;
                Message.text = "Would you like to purchase?";
            }
            else if (item > your)
            {
                OK.interactable = false;
                Message.text = "There is not enough heart.";
            }
        }
        else if (itemname.Equals("Body1"))
        {
            character.SetActive(false);
            head1.SetActive(false);
            head2.SetActive(false);
            head3.SetActive(false);
            body1.SetActive(true);
            itemNumber = "item4";
            minusHeart.text = yourHeart.text;
            item = Convert.ToInt32(bodyText1.text);
            your = Convert.ToInt32(yourHeart.text);

            if (item < your)
            {
                OK.interactable = true;
                Message.text = "Would you like to purchase?";
            }
            else if (item > your)
            {
                OK.interactable = false;
                Message.text = "There is not enough heart.";
            }
        }
    }


    public void PopCancel()
    {
        item = 0;
        your =0;
        minusHeart.text = "0000";
        head1.SetActive(false);
        head2.SetActive(false);
        head3.SetActive(false);
        body1.SetActive(false);
        character.SetActive(true);
        pop.SetActive(false);
    }

    // 아이템 결제 ok 
    public void PaymentItem()
    {
        StartCoroutine(Payment());
    }

    IEnumerator Payment()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("itemnumber", itemNumber);
        form.AddField("itempay", item);

        using (UnityWebRequest www = UnityWebRequest.Post("http://15.164.101.199/itempayment.php", form))
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
                PopCancel();
                roomHeart.GetComponent<HeartCountManager>().HeartUpdate();
                payHeart.GetComponent<HeartCountManager>().HeartUpdate();
                paySlot.GetComponent<PaySlotManager>().PaySlotCheck();
            }
        }
    }

}
