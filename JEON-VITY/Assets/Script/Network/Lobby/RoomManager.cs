
using System;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour
{
    [Header("Room Panel Obj")]
    public GameObject roomPanel; // 게임방 화면 
    public GameObject roomPrefab; // 로비에 있는 룸 오브젝트 
    public GameObject playerCount;//


    public string ownerName;
    public string roomPass;
    public bool groupCheck;
    public int count;


    //PlayerData playerData = new PlayerData();

    // 게임방 패널 열기 
    public void OpenRoomPanel()
    {
        //var rm = GameObject.Find("LobbyManager").GetComponent<RoomManager>();
        //Count = rm.Count;
        var lo = GameObject.Find("LobbyManager").GetComponent<LobbyManager>();
        //현재 방의 인원수를 체크함 
       count = Convert.ToInt32(playerCount.name);
        

            //Debug.Log("게임방 카운트"+count);
            //Debug.Log("단체전 : " + playerData.groupCheck);
            //// 현재 인원수를 체크  꽉차있으면 실행하지 않는다.
            //Debug.Log("Count" + Count);
            // 비밀번호를 가져옴 
            roomPass = roomPrefab.GetComponentInChildren<InputField>().text.ToString();
        //방장이름가져옴 
        ownerName = roomPrefab.name;
        // 단체전인지 개인전인지 구분함 
        groupCheck = Convert.ToBoolean(roomPrefab.GetComponentInChildren<InputField>().name.ToString());


        

        // 단체전일때 
        if (groupCheck)
        {
            if (count > 3)
            {
                var la = GameObject.Find("lobbyCanvas").GetComponent<LobbyAlert>();
                la.OpenAlert();
                return;
            }

            //    Debug.Log("방비밀번호" + roomPass);

            //비밀번호가 없으면 
            if (roomPass.Equals(""))
            {
                try
                {
                    lo.InGameRoom(roomPrefab.name);
                    //Debug.Log("방문자 : " + lo.clientName);
                    //Debug.Log("방장은 : " + roomPrefab.name);
                }
                catch (Exception e)
                {
                    Debug.Log("Socket error : " + e.Message);
                }
            }
            //비밀번호가 있으면 
            else
            {
                var rpp = GameObject.Find("lobbyCanvas").GetComponent<RoomPassPopup>();
                rpp.OpenPopup(roomPrefab.name, roomPass);
            }
        }
        else //개인전일때 
        {
            if (count > 1)
            {
                var la = GameObject.Find("lobbyCanvas").GetComponent<LobbyAlert>();
                la.OpenAlert();
                return;
            }

            //    Debug.Log("방비밀번호" + roomPass);

            //비밀번호가 없으면 
            if (roomPass.Equals(""))
            {
                try
                {
                    // 방장이름 보내기 
                    lo.InGameRoom(roomPrefab.name);
                    //Debug.Log("방문자 : " + lo.clientName);
                    //Debug.Log("방장은 : " + roomPrefab.name);
                }
                catch (Exception e)
                {
                    Debug.Log("Socket error : " + e.Message);
                }
            }
            //비밀번호가 있으면 
            else
            {
                var rpp = GameObject.Find("lobbyCanvas").GetComponent<RoomPassPopup>();
                rpp.OpenPopup(roomPrefab.name, roomPass);
            }
        }
        
    }

}
