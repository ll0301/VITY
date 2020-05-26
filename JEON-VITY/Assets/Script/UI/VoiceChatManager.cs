using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceChatManager : MonoBehaviour
{

    public GameObject joinButton;
    public GameObject leaveButton;

    public bool micOnOff;
    // Start is called before the first frame update
    void Start()
    {
        joinButton.SetActive(true);
        leaveButton.SetActive(false);
        micOnOff = false;
    }
    
    public void micOn()
    {
        if (micOnOff)
            return;
        joinButton.SetActive(false);
        leaveButton.SetActive(true);
        micOnOff = true;
    }

    public void micOff()
    {
        if (!micOnOff)
            return;

        joinButton.SetActive(true);
        leaveButton.SetActive(false);
        micOnOff = false;
    }
}
