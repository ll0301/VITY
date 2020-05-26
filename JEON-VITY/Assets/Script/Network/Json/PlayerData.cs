using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string username = "";
    public string message = "";
    public bool inRoom ;
    public bool roomOwner;
    public string roomName = "";
    public string ownerName = "";

    // 룸정보 
    public bool readyCheck;
    public bool groupCheck;
    public bool privateRoom;
    public string roomPasswd = "";
    public bool startGame;

    // 1회성
    public int playerCount; // 방으로 들어갈때 안에있는 플레이어수를 체크할 것. 
    public bool refreshOn; // 새로고침 버튼을 누를때
    public bool panelOff; // 방장인 상황에서 게임패널을 끌때 사용

}
