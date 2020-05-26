using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSkinManager : MonoBehaviour
{
    public string head;
    public string body;

    public List<GameObject> bodyList;
    public List<GameObject> headList;

    private void Awake()
    {
        var lo = GameObject.Find("Lobby").GetComponent<SkinManager>();
        head = lo.head;
        body = lo.body;

        // 캐릭터의 헤어스타일 선택 
        foreach (GameObject obj in headList)
        {
            if (obj.name.Equals(head))
            {
                obj.SetActive(true);
            }
            else
            {
                obj.SetActive(false);
            }
        }

        // 캐릭터의 바디스타일 선택 
        foreach (GameObject obj in bodyList)
        {

            if (obj.name.Equals(body))
            {
                obj.SetActive(true);
            }
            else
            {
                obj.SetActive(false);
            }
        }

    }



}
