using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinManager : MonoBehaviour
{
    public string head;
    public string body;

    public int level;

    public int heartCount;

    private void Awake()
    {
        head = "Head 10";
        body = "MA Body 3"; 
    }
}
