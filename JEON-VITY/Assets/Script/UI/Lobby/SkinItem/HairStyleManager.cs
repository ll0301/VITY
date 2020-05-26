using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HairStyleManager : MonoBehaviour
{

    public GameObject lobby;
    public GameObject payPop;
    public List<GameObject> headList;
    public List<GameObject> slotList;


    public void Head1()
    {
        payPop.SetActive(true);
        payPop.GetComponent<PaymentManager>().itemname ="Head1";
        payPop.GetComponent<PaymentManager>().PopupStart();
    }

    public void Head2()
    {
        payPop.SetActive(true);
        payPop.GetComponent<PaymentManager>().itemname = "Head2";
        payPop.GetComponent<PaymentManager>().PopupStart();
    }

    public void Head3()
    {
        payPop.SetActive(true);
        payPop.GetComponent<PaymentManager>().itemname = "Head3";
        payPop.GetComponent<PaymentManager>().PopupStart();
    }



    public void slot1btn()
    {
        // 캐릭터의 헤어스타일 선택 
        foreach (GameObject obj in headList)
        {

            if(obj.name.Equals("Head 10"))
            {
                obj.SetActive(true);
                lobby.GetComponent<SkinManager>().head = "Head 10";
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
            if(obj.name.Equals("Slot1"))
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
        // 캐릭터의 헤어스타일 선택 
        foreach (GameObject obj in headList)
        {

            if (obj.name.Equals("Head 9"))
            {
                obj.SetActive(true);
                lobby.GetComponent<SkinManager>().head = "Head 9";
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
        // 캐릭터의 헤어스타일 선택 
        foreach (GameObject obj in headList)
        {

            if (obj.name.Equals("Head 5"))
            {
                obj.SetActive(true);
                lobby.GetComponent<SkinManager>().head = "Head 5";
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
        // 캐릭터의 헤어스타일 선택 
        foreach (GameObject obj in headList)
        {

            if (obj.name.Equals("Head 1"))
            {
                obj.SetActive(true);
                lobby.GetComponent<SkinManager>().head = "Head 1";
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

    public void slot5btn()
    {
        // 캐릭터의 헤어스타일 선택 
        foreach (GameObject obj in headList)
        {

            if (obj.name.Equals("Head 4"))
            {
                obj.SetActive(true);
                lobby.GetComponent<SkinManager>().head = "Head 4";
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
            if (obj.name.Equals("Slot5"))
            {
                obj.GetComponent<Button>().interactable = false;
            }
            else
            {
                obj.GetComponent<Button>().interactable = true;
            }
        }
    }

    public void slot6btn()
    {
        // 캐릭터의 헤어스타일 선택 
        foreach (GameObject obj in headList)
        {

            if (obj.name.Equals("Head 14"))
            {
                obj.SetActive(true);
                lobby.GetComponent<SkinManager>().head = "Head 14";
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
            if (obj.name.Equals("Slot6"))
            {
                obj.GetComponent<Button>().interactable = false;
            }
            else
            {
                obj.GetComponent<Button>().interactable = true;
            }
        }
    }

    public void slot7btn()
    {
        // 캐릭터의 헤어스타일 선택 
        foreach (GameObject obj in headList)
        {

            if (obj.name.Equals("Head 8"))
            {
                obj.SetActive(true);
                lobby.GetComponent<SkinManager>().head = "Head 8";
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
            if (obj.name.Equals("Slot7"))
            {
                obj.GetComponent<Button>().interactable = false;
            }
            else
            {
                obj.GetComponent<Button>().interactable = true;
            }
        }
    }

    public void slot8btn()
    {
        // 캐릭터의 헤어스타일 선택 
        foreach (GameObject obj in headList)
        {

            if (obj.name.Equals("Head 6"))
            {
                obj.SetActive(true);
                lobby.GetComponent<SkinManager>().head = "Head 6";
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
            if (obj.name.Equals("Slot8"))
            {
                obj.GetComponent<Button>().interactable = false;
            }
            else
            {
                obj.GetComponent<Button>().interactable = true;
            }
        }
    }

    public void slot9btn()
    {
        // 캐릭터의 헤어스타일 선택 
        foreach (GameObject obj in headList)
        {

            if (obj.name.Equals("Head 7"))
            {
                obj.SetActive(true);
                lobby.GetComponent<SkinManager>().head = "Head 7";
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
            if (obj.name.Equals("Slot9"))
            {
                obj.GetComponent<Button>().interactable = false;
            }
            else
            {
                obj.GetComponent<Button>().interactable = true;
            }
        }
    }

    public void slot10btn()
    {
        // 캐릭터의 헤어스타일 선택 
        foreach (GameObject obj in headList)
        {

            if (obj.name.Equals("Head 11"))
            {
                obj.SetActive(true);
                lobby.GetComponent<SkinManager>().head = "Head 11";
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
            if (obj.name.Equals("Slot10"))
            {
                obj.GetComponent<Button>().interactable = false;
            }
            else
            {
                obj.GetComponent<Button>().interactable = true;
            }
        }
    }

    public void slot11btn()
    {
        // 캐릭터의 헤어스타일 선택 
        foreach (GameObject obj in headList)
        {

            if (obj.name.Equals("Head 12"))
            {
                obj.SetActive(true);
                lobby.GetComponent<SkinManager>().head = "Head 12";
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
            if (obj.name.Equals("Slot11"))
            {
                obj.GetComponent<Button>().interactable = false;
            }
            else
            {
                obj.GetComponent<Button>().interactable = true;
            }
        }
    }

    public void slot12btn()
    {
        // 캐릭터의 헤어스타일 선택 
        foreach (GameObject obj in headList)
        {

            if (obj.name.Equals("Head 13"))
            {
                obj.SetActive(true);
                lobby.GetComponent<SkinManager>().head = "Head 13";
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
            if (obj.name.Equals("Slot12"))
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
