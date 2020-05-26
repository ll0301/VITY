using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

// 버튼에 대한 스크립트
public class FixedButtonB : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
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
