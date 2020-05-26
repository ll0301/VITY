using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyFade : MonoBehaviour
{
    public void Fade()
    {
        StartCoroutine(DoFade());
    }

    IEnumerator DoFade()
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();

        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime * 2;
            yield return null;
        }
        canvasGroup.interactable = false;
        yield return null;

        var lo = GameObject.Find("LobbyManager").GetComponent<LobbyManager>();
        lo.Room_Active();
        //LobbyManager.instance.Room_Active();
    }
}
