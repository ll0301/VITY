using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomUiManager : MonoBehaviour
{
    [Header("PlayerBox Obj")]
    public GameObject playerList; // 게임방 화면 
    public GameObject player_box;
    public GameObject ready_on;
    public GameObject ready_off;
    public GameObject start_on;
    public GameObject readyIcon;
    public GameObject kickOut_icon;
    public GameObject ownerIcon;
    public Text playerName;
    public Button CharacterSetting;


    public GameObject popup;
    public Text message;

    public bool readyOn = false;

    private void Update()
    {
        var lo = GameObject.Find("LobbyManager").GetComponent<LobbyManager>();
        if (lo.playerData.roomOwner) // 방장일때 
        {
            if(lo.playerData.groupCheck)//단체전일때
            {
                if (lo.mReadyList.Count==4) //4명모두 레디를 했다면 
                {
                  //  스타트 버튼이 활성화된다.
                    start_on.GetComponent<Button>().interactable = true;
                }
                else
                {
                    //  스타트 버튼이 비활성화된다.
                    start_on.GetComponent<Button>().interactable = false;
                }
            }
            else if (!lo.playerData.groupCheck)//개인전일때 
            {
                if (lo.mReadyList.Count == 2)  //2명모두 레디를 했다면 
                {
                    //  스타트 버튼이 활성화된다.
                    start_on.GetComponent<Button>().interactable = true;
                }
                else
                {
                    //  스타트 버튼이 비활성화된다.
                    start_on.GetComponent<Button>().interactable = false;
                }
            }
        }
        else
        {
            start_on.SetActive(false);
        }

    }


    // 레디하기 
    public void OnReadyButton()
    { 
        if (!readyOn)
        {
            readyOn = true;
            ready_on.SetActive(false);
            ready_off.SetActive(true);
            CharacterSetting.interactable = false;
            var lo = GameObject.Find("LobbyManager").GetComponent<LobbyManager>();
            lo.playerData.readyCheck = true;
            lo.playerData.message = "readyonoffvity";
            lo.PlayerSaveData();
        }
    }

    //레디취소하기 
    public void OffReadyButton()
    {
        if (readyOn)
        {
            Debug.Log("레디취소");
            readyOn = false;
            ready_on.SetActive(true);
            ready_off.SetActive(false);
            CharacterSetting.interactable = true;
            var lo = GameObject.Find("LobbyManager").GetComponent<LobbyManager>();
            lo.playerData.readyCheck = false;
            lo.playerData.message = "readyonoffvity";
            lo.PlayerSaveData();
        }
    }
    
    public void OpenPopup()
    {
        if (popup != null)
        {
            message.text = playerName.GetComponentInChildren<Text>().text + "님을 퇴장시킵니다.";
            popup.SetActive(true);
        }
    }

    public void ClosePopup()
    {
        if (popup != null)
        {
            popup.SetActive(false);
        }
    }

    // 플레이어 강제퇴장시키기 
    public void KickOutPlayer()
    {
        popup.SetActive(false);
        var lo = GameObject.Find("LobbyManager").GetComponent<LobbyManager>();
        lo.playerData.message = "kickoutplayer";
        lo.playerData.roomPasswd = playerName.GetComponentInChildren<Text>().text.ToString();
        Debug.Log(lo.playerData.roomPasswd);
        lo.PlayerSaveData();
    }
}
