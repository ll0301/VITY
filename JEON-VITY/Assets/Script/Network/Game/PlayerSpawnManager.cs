using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;

public class PlayerSpawnManager : MonoBehaviour
{
   // public GameObject NPC; //npc 
    public GameObject otherCharacter; // 다른 유저
    public GameObject character; // 나의캐릭터
    public GameObject spawnA; // 게임시작 포지션 
    public GameObject spawnB;
    public GameObject spawnC;
    public GameObject spawnD;



   // public int hairChoice;
    //public int bodyChoice;
    public int spawnChoice;

    //내위치 지정  
    public void Spawn(int count, int group, string name, string owner, bool isOwner)
    {
        var nm = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        nm.loadingCanvas.SetActive(false);
        nm.joystick.SetActive(true);
        spawnChoice = Random.Range(1, 10); // 개인전일경우 2개포인트중 랜덤으로 
                                           // hairChoice = Random.Range(1, 5);
                                           // bodyChoice = Random.Range(1, 5);
        nm.voiceChatInput.GetComponent<InputField>().text = owner;
        var npc = GameObject.Find("NPCSpawn").GetComponent<NPCSpawnManager>();


        //개인전
        if (group == 2)
        {
            //MyNPC(count, group, spawnChoice);

            // 카운트1은  게임방에서의 몇번째 순서인가를 나타낸다. 
            if (count == 1)
            {
                if (spawnChoice <= 5) 
                {
                    spawnA.SetActive(true);  // 포인트가 생성되고 
                    character.GetComponent<Player>().nameText.GetComponentInChildren<TextMesh>().text = name;
                    Instantiate(character, spawnA.transform.position, Quaternion.identity);
                    nm.myPoint = "A";
                    //캐릭터가 생성되고  서버에 나의 위치를 전송하기위해 포인트 저장 
                   npc.NPCSpawn(owner,"A",group,isOwner);
                }
                 else if(spawnChoice > 5)
                {
                    spawnC.SetActive(true);
                    character.GetComponent<Player>().nameText.GetComponentInChildren<TextMesh>().text = name;
                    Instantiate(character, spawnC.transform.position, Quaternion.identity);
                    nm.myPoint = "C";
                   npc.NPCSpawn(owner,"C",group,isOwner);
                }
            }
            else if (count == 2)
            {
                if (spawnChoice <= 5)
                {
                    spawnB.SetActive(true);
                    character.GetComponent<Player>().nameText.GetComponentInChildren<TextMesh>().text = name;
                    Instantiate(character, spawnB.transform.position, Quaternion.identity);
                    nm.myPoint = "B";
                   npc.NPCSpawn(owner,"B",group,isOwner);
                }
                else if (spawnChoice > 5)
                {
                    spawnD.SetActive(true);
                    character.GetComponent<Player>().nameText.GetComponentInChildren<TextMesh>().text = name;
                    Instantiate(character, spawnD.transform.position, Quaternion.identity);
                    nm.myPoint = "D";
                    npc.NPCSpawn(owner,"D",group,isOwner);
                }
            }
          
        }
        //단체전
        else if(group == 4)
        {

            if (count == 1)
            {
                spawnA.SetActive(true);
                character.GetComponent<Player>().position = "A";
                character.GetComponent<Player>().nameText.GetComponentInChildren<TextMesh>().text = name;
                Instantiate(character, spawnA.transform.position, Quaternion.identity);
                nm.myPoint = "A";
                npc.NPCSpawn(owner, "A", group,isOwner);
                nm.userA.text = name;
                nm.statusA.text = "ALIVE";
            }
            else  if (count == 2)
            {
                spawnB.SetActive(true);
                character.GetComponent<Player>().position = "B";
                character.GetComponent<Player>().nameText.GetComponentInChildren<TextMesh>().text = name;
                Instantiate(character, spawnB.transform.position, Quaternion.identity);
                nm.myPoint = "B";
                npc.NPCSpawn(owner, "B", group,isOwner);
                nm.userB.text = name;
                nm.statusB.text = "ALIVE";
            }
            else  if (count== 3)
            {
                spawnC.SetActive(true);
                character.GetComponent<Player>().position = "C";
                character.GetComponent<Player>().nameText.GetComponentInChildren<TextMesh>().text = name;
                Instantiate(character, spawnC.transform.position, Quaternion.identity);
                nm.myPoint = "C";
                npc.NPCSpawn(owner, "C", group,isOwner);
                nm.userC.text = name;
                nm.statusC.text = "ALIVE";
            }
            else if (count == 4)
            {
                spawnD.SetActive(true);
                character.GetComponent<Player>().position = "D";
                character.GetComponent<Player>().nameText.GetComponentInChildren<TextMesh>().text = name;
                Instantiate(character, spawnD.transform.position, Quaternion.identity);
                nm.myPoint = "D";
                npc.NPCSpawn(owner, "D", group,isOwner);
                nm.userD.text = name;
                nm.statusD.text = "ALIVE";
            }
        }

        nm.ConnectToUdpServer();
    }


    //다른플레이어 위치지정 
    public void OtherSpawn(string owner, string name, string position, int group, bool isOwner)
    {
        var nm = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        var npc = GameObject.Find("NPCSpawn").GetComponent<NPCSpawnManager>();
        if (position.Equals("A"))
        {
            spawnA.SetActive(true);
            otherCharacter.GetComponent<OtherPlayer>().position = position;
            otherCharacter.GetComponent<OtherPlayer>().nameText.GetComponentInChildren<TextMesh>().text = name;
            otherCharacter.name = name;
            otherCharacter.GetComponent<OtherPlayer>().ownerName = owner;
            Instantiate(otherCharacter, spawnA.transform.position, Quaternion.identity);
            npc.OtherNPCSpawn(owner,position,group, isOwner);
            nm.userA.text = name;
            nm.statusA.text = "ALIVE";
        }
        else if (position.Equals("B"))
        {
            spawnB.SetActive(true);
            otherCharacter.GetComponent<OtherPlayer>().position = position;
            otherCharacter.GetComponent<OtherPlayer>().nameText.GetComponentInChildren<TextMesh>().text = name;
            otherCharacter.name = name;
            otherCharacter.GetComponent<OtherPlayer>().ownerName = owner;
            Instantiate(otherCharacter, spawnB.transform.position, Quaternion.identity);
            npc.OtherNPCSpawn(owner,position,group, isOwner);
            nm.userB.text = name;
            nm.statusB.text = "ALIVE";
        }
        else if (position.Equals("C"))
        {
            spawnC.SetActive(true);
            otherCharacter.GetComponent<OtherPlayer>().position = position;
            otherCharacter.GetComponent<OtherPlayer>().nameText.GetComponentInChildren<TextMesh>().text = name;
            otherCharacter.name = name;
            otherCharacter.GetComponent<OtherPlayer>().ownerName = owner;
            Instantiate(otherCharacter, spawnC.transform.position, Quaternion.identity);
            npc.OtherNPCSpawn(owner,position,group, isOwner);
            nm.userC.text = name;
            nm.statusC.text = "ALIVE";
        }
        else if (position.Equals("D"))
        {
            spawnD.SetActive(true);
            otherCharacter.GetComponent<OtherPlayer>().position = position;
            otherCharacter.GetComponent<OtherPlayer>().nameText.GetComponentInChildren<TextMesh>().text = name;
            otherCharacter.name = name;
            otherCharacter.GetComponent<OtherPlayer>().ownerName = owner;
            Instantiate(otherCharacter, spawnD.transform.position, Quaternion.identity);
            npc.OtherNPCSpawn(owner,position, group, isOwner);
            nm.userD.text = name;
            nm.statusD.text = "ALIVE";
        }
        nm.characterSet = true;
        nm.roomData.characterSet = true;
        nm.RoomSendData();
    }

}
