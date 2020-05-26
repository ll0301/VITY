using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotsManager : MonoBehaviour
{
    public GameObject lobby;

    public GameObject pop;

    public GameObject level2;
    public List<GameObject> Level3;
    public GameObject level4;
    public List<GameObject> Level5;

    public GameObject bodyPayObj;
    public GameObject headPayObj1;
    public GameObject headPayObj2;
    public GameObject headPayObj3;

    public int level;
    private void Awake()
    {
        level = lobby.GetComponent<SkinManager>().level;

        if (level == 2)
        {
            level2.SetActive(false);
        }
        if (level == 3)
        {
            level2.SetActive(false);
            foreach (GameObject obj in Level3)
            {
                obj.SetActive(false);
            }
        }
        if (level == 4)
        {
            level2.SetActive(false);
            foreach (GameObject obj in Level3)
            {
                obj.SetActive(false);
            }
            level4.SetActive(false);
        }
        if(level == 5)
        {
            level2.SetActive(false);
            foreach (GameObject obj in Level3)
            {
                obj.SetActive(false);
            }
            level4.SetActive(false);
            foreach (GameObject obj in Level5)
            {
                obj.SetActive(false);
            }
        }


    }


    public void Level2btn()
    {
        pop.SetActive(true);
        pop.GetComponent<AlertController>().level = 2;
        pop.GetComponent<AlertController>().SetText();
    }

    public void Level3btn()
    {
        pop.SetActive(true);
        pop.GetComponent<AlertController>().level = 3;
        pop.GetComponent<AlertController>().SetText();
    }

    public void Level4btn()
    {
        pop.SetActive(true);
        pop.GetComponent<AlertController>().level = 4;
        pop.GetComponent<AlertController>().SetText();
    }

    public void Level5btn()
    {
        pop.SetActive(true);
        pop.GetComponent<AlertController>().level = 5;
        pop.GetComponent<AlertController>().SetText();
    }
}
