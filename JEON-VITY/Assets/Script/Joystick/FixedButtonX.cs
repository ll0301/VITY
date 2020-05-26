using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class FixedButtonX : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
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
            Debug.Log("Pressed X" + Pressed);
            StartCoroutine(Timer());
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
       // Pressed = false;
    }


    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(0.5f);
        Pressed = false;
        Debug.Log("Pressed X timer" + Pressed);
    }

}
