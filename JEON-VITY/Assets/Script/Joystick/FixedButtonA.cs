using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class FixedButtonA : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    [HideInInspector]
    public bool Pressed;

    private void Awake()
    {
        Pressed = false;
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if (!Pressed)
        {
            Pressed = true;
            StartCoroutine(Timer());
        }       
    }


    public void OnPointerUp(PointerEventData eventData)
    {
       //Pressed = false;
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(1f);
        Pressed = false;
    }
}
