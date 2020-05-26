using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FixedButtonAim : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{

    [HideInInspector]
    public bool Pressed;

    public void OnPointerDown(PointerEventData eventData)
    {
        Pressed = true;
        //  Debug.Log("눌림");
      // StartCoroutine(Timer());
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        Pressed = false;
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(0.5f);
        Pressed = false;
    }
}
