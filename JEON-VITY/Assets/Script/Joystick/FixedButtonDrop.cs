using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FixedButtonDrop : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [HideInInspector]
    public bool Pressed;
    
    public void OnPointerDown(PointerEventData eventData)
    {

            Pressed = true;
            StartCoroutine(Timer());

    }

    public void OnPointerUp(PointerEventData eventData)
    {
         Pressed = false;
    }


    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(0.1f);
        Pressed = false;
    }
}
