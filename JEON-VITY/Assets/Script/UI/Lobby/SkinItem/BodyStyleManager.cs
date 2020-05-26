using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BodyStyleManager : MonoBehaviour
{
    public GameObject lobby;
    //public GameObject character;
    public GameObject payPop;
    public List<GameObject> bodyList;
    public List<GameObject> slotList;

    public void Body1()
    {
        payPop.SetActive(true);
        payPop.GetComponent<PaymentManager>().itemname = "Body1";
        payPop.GetComponent<PaymentManager>().PopupStart();
    }

    public void slot1btn()
    {
        // 캐릭터의 바디스타일 선택 
        foreach (GameObject obj in bodyList)
        {

            if (obj.name.Equals("MA Body 3"))
            {
                obj.SetActive(true);
                lobby.GetComponent<SkinManager>().body = "MA Body 3";
            }
            else
            {
                obj.SetActive(false);
            }
        }

        //버튼 막기
        foreach (GameObject obj in slotList)
        {
            // Debug.Log(slotList.Count);
            if (obj.name.Equals("Slot1"))
            {
                obj.GetComponent<Button>().interactable = false;
            }
            else
            {
                obj.GetComponent<Button>().interactable = true;
            }
        }
    }

    public void slot2btn()
    {
        // 캐릭터의 바디스타일 선택 
        foreach (GameObject obj in bodyList)
        {

            if (obj.name.Equals("MA Body 5"))
            {
                obj.SetActive(true);
                lobby.GetComponent<SkinManager>().body = "MA Body 5";
            }
            else
            {
                obj.SetActive(false);
            }
        }

        //버튼 막기
        foreach (GameObject obj in slotList)
        {
            // Debug.Log(slotList.Count);
            if (obj.name.Equals("Slot2"))
            {
                obj.GetComponent<Button>().interactable = false;
            }
            else
            {
                obj.GetComponent<Button>().interactable = true;
            }
        }
    }

    public void slot3btn()
    {
        // 캐릭터의 바디스타일 선택 
        foreach (GameObject obj in bodyList)
        {

            if (obj.name.Equals("MA Body 4"))
            {
                obj.SetActive(true);
                lobby.GetComponent<SkinManager>().body = "MA Body 4";
            }
            else
            {
                obj.SetActive(false);
            }
        }

        //버튼 막기
        foreach (GameObject obj in slotList)
        {
            // Debug.Log(slotList.Count);
            if (obj.name.Equals("Slot3"))
            {
                obj.GetComponent<Button>().interactable = false;
            }
            else
            {
                obj.GetComponent<Button>().interactable = true;
            }
        }
    }

    public void slot4btn()
    {
        // 캐릭터의 바디스타일 선택 
        foreach (GameObject obj in bodyList)
        {

            if (obj.name.Equals("MA Body 2"))
            {
                obj.SetActive(true);
                lobby.GetComponent<SkinManager>().body = "MA Body 2";
            }
            else
            {
                obj.SetActive(false);
            }
        }

        //버튼 막기
        foreach (GameObject obj in slotList)
        {
            // Debug.Log(slotList.Count);
            if (obj.name.Equals("Slot4"))
            {
                obj.GetComponent<Button>().interactable = false;
            }
            else
            {
                obj.GetComponent<Button>().interactable = true;
            }
        }
    }
}
