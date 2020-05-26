using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FixedButtonR : MonoBehaviour, IPointerDownHandler
{
    public GameObject button;
    [HideInInspector]
    public bool Pressed;
    int count;
   

    public void OnPointerDown(PointerEventData eventData)
    {
        count++;
        if (count == 1)
        {
            Pressed = true;
          //  Debug.Log("클릭");
           // Debug.Log(count);
            button.GetComponent<Button>().interactable = false;
        }
        if(count == 2)
        {
            Pressed = false;
           // Debug.Log("취소");
           // Debug.Log(count);
            count = 0;
            button.GetComponent<Button>().interactable = true;
        }
    }

}
