using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Json;
using System.Text;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour
{

    public List<GameObject> mCharacter;

    public GameObject loadingCanvas; // 로딩중 화면을 보여주는 캔버스 
    public GameObject joystick; // 캐릭터 조종할 조이스틱
    public GameObject buttonA;
    public GameObject buttonB;
    public GameObject handler;
    public GameObject joysticks;
    public GameObject gameoverPop;// 게임종료 팝업
    public GameObject gamequitPop;
    public Text gameoverTimer;
    public GameObject clientCharacter;

    [Header("**groupNavi**")]
    public GameObject voiceChatInput;

    [Header("**groupNavi**")]
    public GameObject groupNaviOpen;
    public Text userA;
    public Text statusA;
    public Text userB;
    public Text statusB;
    public Text userC;
    public Text statusC;
    public Text userD;
    public Text statusD;
    public GameObject groupNaviClose;
    public GameObject btnClose;
    public GameObject btnQuit;
    public int dieCount;

    public void GroupNaviOpen()
    {
        groupNaviOpen.SetActive(true);
        groupNaviClose.SetActive(false);
    }
    public void GroupNaviClose()
    {
        groupNaviOpen.SetActive(false);
        groupNaviClose.SetActive(true);
    }



    [Header("**Socket**")]
    public string host;   // 호스트 주소 
    public int port;  // 포트번호 
    public UdpClient socket; // udp 소켓
    public bool udpSocketReady = false; // 처음 접속한것인지  
    public bool characterSet = false; // 캐릭터가 셋팅이 되었는지
    //public bool move = false; // 캐릭터가 움직이고 있는지 
    public string owner; // 방장이름 
    public string username; // 내 이름 
    public int playerNumber; // 방안에서의 넘버 
    public int group; // 그룹인지 개인전인지 
    public string myPoint; // 나의 시작 포지션 
    public bool isOwner;// 오너인지 아닌지 
    // IPEndPoint는 
    //스스로의 주소(서버의 주소, 즉 IPAddress)를 지정하는 클래스이다
    IPEndPoint epRemote; 
   
    byte[] bytes; // 패킷으로 전송받은 데이터가 담길곳 
    //JSON 
    string roomFilename = "room.json"; // 플레이어 상태 저장 
    string roomPath; // json 파일 저장경로 
    public RoomData roomData = new RoomData(); // 전달할 나의 정보  

    private void Awake()
    {
        Application.targetFrameRate = 60;
        var lm = GameObject.Find("Lobby/LobbyBox/LobbyManager").GetComponent<LobbyManager>();
        username = lm.clientName;
        owner = lm.ownerName;
        playerNumber = lm.playerCount;
        group = lm.groupCount;
        lm.lobbyBox.SetActive(false);
    }

    private void Start()
    {
        if(owner.Equals(username))
        {
            isOwner = true;
        }
        else
        {
            isOwner = false;
        }

        //Debug.Log("cobj" + GameObject.Find(name + "(Clone)").name);
        roomPath = Application.persistentDataPath + "/" + roomFilename; 
        var ps = GameObject.Find("PlayerSpawn").GetComponent<PlayerSpawnManager>();
        //고정프레임
        // Debug.Log("게임시작!");
        // 시작위치 정해주기 
       ps.Spawn(playerNumber, group, username, owner, isOwner);

    //패킷수신 대기
    // IPAddress.Any는 서버자체에 존재하는 IP를 사용
        epRemote = new IPEndPoint(IPAddress.Any, 0);
        bytes = socket.Receive(ref epRemote);
        // 패킷받기 코루틴 시작 
        StartCoroutine(receivePacket());
        //StartCoroutine(LoadTimer());


        // 단체전이면 그룹 네비게이션이 활성화됨 
        if(group == 4)
        {
            GroupNaviOpen();
        }
    }

    public void ConnectToUdpServer()
    {
        if (udpSocketReady)
            return;
        try
        {

            udpSocketReady = true;
            roomData.firstData = true;
            roomData.owner = owner;
            roomData.username = username;
            roomData.group = group.ToString();
            roomData.myPoint = myPoint;
            roomData.playerNumber = playerNumber.ToString();
            roomData.alive = true;
            roomData.winner = false;
            roomData.characterSet = false;
            roomData.move = false;
            // roomData.receivePacket = false;
            roomData.punch = false;
            roomData.throwItem = false;//
            roomData.throwMode = false;//
            roomData.pickup = false;//
            roomData.drop = false;//
            roomData.pX = "";
            roomData.pY = "";
            roomData.pZ = "";
            roomData.rY = "";
            roomData.runX = "";
            roomData.runZ = "";

            RoomSendData();
        }
        catch (Exception e)
        {
            Debug.Log("Socket error : " + e.Message);
        }

        roomData.firstData = false;
        RoomSaveData();
    }



    // 룸정보를 json 데이터로 보낸다.. 
    public void RoomSendData()
    {
        socket = new UdpClient();
        IPEndPoint ep = new IPEndPoint(IPAddress.Parse(host), port);
        socket.Connect(ep);
        // json 데이터의 포장지 느낌   
        RoomWrapper wrapper = new RoomWrapper();
        wrapper.roomData = roomData;
        string room = JsonUtility.ToJson(wrapper, true);
        System.IO.File.WriteAllText(roomPath, room); // json 파일에 작성 
        //string s = System.IO.File.ReadAllText(roomPath);
        //// Json 데이터 보내기 
        //MemoryStream memoryStream = new MemoryStream();
        ////.NET 객체를 JSON으로 변환하고 다시 복원할때 사용한다.  
        //DataContractJsonSerializer serRoom = new DataContractJsonSerializer(typeof(RoomWrapper));
        //serRoom.WriteObject(memoryStream, wrapper);
        //memoryStream.Position = 0;
        //StreamReader sr = new StreamReader(memoryStream);
        var data = Encoding.UTF8.GetBytes(room);
        //  Debug.Log("b" + data.ToString());
        socket.Send(data, data.Length);

    }
   
    // 서버로부터 게임화면 넘어갈때 패킷받기 
    public void ReceivePacket(string data)
    {
        bytes = null;
        try
        {
            JObject obj = JObject.Parse(data);
            string setGame = obj["setGame"].ToString();
            string li = obj["listSize"].ToString();
            bool list = Convert.ToBoolean(li);
            //Debug.Log("listSize" + list);
            // 서버에 있는 json 파일에 내가 속해있는 방의 유저의 정보가 모두 들어왔는지를 판단.
            // 모두 들어와있지 않으면 
            if (!list)
            {
                //Debug.Log("한번더");
                // 한번더 패킷을 받아 업데이트한다. 
                RoomSendData();
                epRemote = new IPEndPoint(IPAddress.Any, 0);
                bytes = socket.Receive(ref epRemote);
            }
            // 모두 들어와있으면 
            if (list)
            {
                socket.Close();
                //  Debug.Log(data);
                JArray playerList = JArray.Parse(setGame);
                {
                    foreach (JObject gobj in playerList.Children<JObject>())
                    {
                        string nm = gobj["name"].ToString();
                        string po = gobj["position"].ToString();
                        // 다른유저 세팅 
                        if (!nm.Equals(username))
                        {
                            var ps = GameObject.Find("PlayerSpawn").GetComponent<PlayerSpawnManager>();
                            ps.OtherSpawn(owner,nm, po, group, isOwner); // 스폰매니저를 실행하여 각자의 위치를 지정한다. 

                           // Debug.Log("다른유저위치1");
                           // Debug.Log(nm);
                           // Debug.Log(po);
                           // Debug.Log(group);
                        }
                    }
                }
                // 코루틴 종료 
                StopCoroutine(receivePacket());
            }
        }
        catch (Exception e)
        {
            Debug.Log("Socket error : " + e.Message);
        }
    }
    // 패킷받기 코루틴 
    private IEnumerator receivePacket()
    {
        while(true)
        {
            if (bytes != null && !roomData.characterSet)
            {
                string data = Encoding.UTF8.GetString(bytes);
                //  Debug.Log("상대접속패킷받기");
                ReceivePacket(data);
            }
            yield return new WaitForSeconds(0.02f);
        }
    }


    public void BackToLobby()
    {
        CloseSocket();
        GameObject.Find("Lobby").transform.Find("LobbyBox").gameObject.SetActive(true);
        var lm = GameObject.Find("Lobby").transform.Find("LobbyBox/LobbyManager").GetComponent<LobbyManager>();
        lm.ComebackLobby();
        SceneManager.LoadScene("ComebackLobby");

    }

    public void BackToRoomOut()
    {
        StartCoroutine(SendUserLose());
        CloseSocket();
        GameObject.Find("Lobby").transform.Find("LobbyBox").gameObject.SetActive(true);
        var lm = GameObject.Find("Lobby").transform.Find("LobbyBox/LobbyManager").GetComponent<LobbyManager>();
        lm.ComebackRoomOut();
        SceneManager.LoadScene("ComebackLobby");
    }

    // 유저의 패배카운트 업데이트하기 
    IEnumerator SendUserLose()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        using (UnityWebRequest www = UnityWebRequest.Post("http://15.164.101.199/sendlose.php", form))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                // popupText.text = www.downloadHandler.text;
            }
            else
            {
                //Debug.Log(www.downloadHandler.text);
            }
        }
    }

    private IEnumerator LoadTimer()
    {
        yield return new WaitForSeconds(3f);// 5초뒤
        loadingCanvas.SetActive(false);
        Renderer bA = buttonA.GetComponent<Renderer>();
        //joystick.SetActive(true);
    }

    //jsondata 저장 하기 
    public void RoomSaveData()
    {
        // json 데이터의 포장지 느낌   
        RoomWrapper wrapper = new RoomWrapper();
        wrapper.roomData = roomData;
        string room = JsonUtility.ToJson(wrapper, true);
        System.IO.File.WriteAllText(roomPath, room); // json 파일에 작성 
    }

    public void CloseSocket()
    {
        if (udpSocketReady)
            return;
        socket.Close();
        roomData.throwItem = false;
        roomData.throwMode = false;
        roomData.pickup = false;
        roomData.drop = false;
        udpSocketReady = false;
        roomData.characterSet = false;
        roomData.move = false;
       // roomData.receivePacket = false;
        roomData.punch = false;
        roomData.alive = false;
        roomData.winner = false;
        roomData.owner = "";
        roomData.username = "";
        roomData.myPoint = "";
        roomData.group = "";
        roomData.playerNumber = "";
        roomData.pX = "";
        roomData.pY = "";
        roomData.pZ = "";
        roomData.rY = "";
        roomData.runX = "";
        roomData.runZ = "";
        RoomSaveData();

    }


    // 게임포기하기 팝업 오픈 
    public void QuitGame()
    {
        gamequitPop.SetActive(true);
    }

    // 게임포기
    public void QuitOK()
    {
        gamequitPop.SetActive(false);
        var ch = clientCharacter.GetComponent<Player>(); // 나갈때 내 캐릭터를 죽임
        ch.health = -10;
    }
    // 게임포기팝업 닫기 
    public void QuitCancel()
    {
        gamequitPop.SetActive(false);
    }

    void OnApplicationQuit()
    {
        CloseSocket();
    }

}
