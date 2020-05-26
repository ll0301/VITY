using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertController : MonoBehaviour
{
    public GameObject character;
    public GameObject pop;
    public GameObject lobby;
    public GameObject textLevel1;
    public GameObject textLevel2;
    public GameObject textLevel3;
    public GameObject textLevel4;
    public GameObject textLevel5;

    public int level;

    public void SetText()
    {
        if (level == 2)
        {
            character.SetActive(false);
            textLevel2.SetActive(true);
            textLevel1.SetActive(false);
            textLevel3.SetActive(false);
            textLevel4.SetActive(false);
            textLevel5.SetActive(false);
        }
        if (level == 3)
        {
            character.SetActive(false);
            textLevel2.SetActive(false);
            textLevel1.SetActive(false);
            textLevel3.SetActive(true);
            textLevel4.SetActive(false);
            textLevel5.SetActive(false);
        }
        if (level == 4)
        {
            character.SetActive(false);
            textLevel2.SetActive(false);
            textLevel1.SetActive(false);
            textLevel3.SetActive(false);
            textLevel4.SetActive(true);
            textLevel5.SetActive(false);
        }
        if (level == 5)
        {
            character.SetActive(false);
            textLevel2.SetActive(false);
            textLevel1.SetActive(false);
            textLevel3.SetActive(false);
            textLevel4.SetActive(false);
            textLevel5.SetActive(true);
        }
    }

    public void OK()
    {
        level = 0;
        textLevel2.SetActive(false);
        textLevel1.SetActive(false);
        textLevel3.SetActive(false);
        textLevel4.SetActive(false);
        textLevel5.SetActive(false);
        character.SetActive(true);
        pop.SetActive(false);
    }
}
