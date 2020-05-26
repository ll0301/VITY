using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.Text;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class LobbyManager : MonoBehaviour
{
    // 로비를 이루고있는 모든 오브젝트를 담아놓은 오브젝트
    public GameObject lobby;
    public GameObject lobbyBox;

    public GameObject playerBox;

    public GameObject loadingObj;

    public GameObject gamequitPop;

    public RoomManager roomManager;

    public GameObject roomHeart;


    //Lobyy chat scroll
    [Header("**Lobby Chat Scroll Manager**")] 
    public Scrollbar chatScroll;  // 채팅창 스크롤 
    public bool chatSend;  // 메세지를 보냈는지
    public float chatItemH; // 채팅스크롤 고정하기위한 변수 
    public int chatCount; 
    public string lastChat;
    public List<float> chatPosition;

    //Room chat scroll
    [Header("**Room Chat Scroll Manager**")]
    public Scrollbar roomChatScroll; // 룸안에서 채팅창 스크롤 
    public bool roomChatSend;
    public float roomChatItemH;
    public int roomChatCount;
    public string roomLastChat;
    public List<float> roomChatPosition;
    public List<GameObject> mRoomChatList;



    //Lobby Chat
    [Header("**Lobby Chat Obj**")]
    public GameObject chatContainer;  // 메세지가 순서대로 담기는곳
    public GameObject messagePrefab; // 각각의 메세지 
    public string clientName;  // 클라이언트 이름 
    public bool socketReady;

    //Looby RoomList 
    [Header("**Lobby RoomList Obj**")]
    public GameObject roomListContainer; // 게임방 리스트가 담기는곳
    public GameObject roomListPrefab;//  각각의 게임방 
    public List<GameObject> mRoomList;
    string roomPass;// 게임방 비밀버호 
    string roomName; // 게임방 이름 


    [Header("**Room Panel Obj**")]
    // RoomPanel
    public GameObject playerContainer; // 플레이어리스트를 채울 컨테이너
    public GameObject playerPrefab; // 플레이어 오브젝트 
    public List<GameObject> mPlayerList;
    public GameObject roomPanel; // 게임방 화면 
    public GameObject roomChatContatiner; // 채팅 컨테이너 
    public GameObject roomChatContent; // 메세지아이템 
    public GameObject roomMessagePrefab; 
    public GameObject roomOnIcon;
    public GameObject roomOffIcon;
    public GameObject messageView;
    public string ownerName;
    public bool roomRefreshOn = false;
    public bool createRoom = false;
    public bool roomChat = false;
    //public bool readyLitener = false;
    public List<string> mReadyList;
    public int playerCount;
    public int groupCount;

    [Header("**Canvas**")]
    public Canvas title_canvas;
    public Canvas lobby_canvas;
    public Canvas room_canvas;
    public Canvas item_canvas;
    public CanvasGroup titleCG;
    public CanvasGroup lobbyCG;
    public CanvasGroup roomCG;
    public CanvasGroup itemCG;
    public GameObject itemCharacter;
    public GameObject screenItem;

    [Header("**Socket**")]
    //Defualt host / port values 
    public string host;   // 호스트 주소 
    public int port;  // 포트번호 
    public TcpClient socket; //tcp 서비스 제공
    // ftp나 http와 같은 애플리케이션 계층 프로토콜을 개발하는데 주로 사용된다. 
    public NetworkStream stream;  // networkstream 은 write() 와 read()를 통해서 바이트데이터를 송 수신한다. 
    public StreamWriter writer;  // 이 클래스를 사용하여 텍스트파일을 열고 작성하고 닫는다. 
    public StreamReader reader;  // 이 클래스를 사용하여 텍스트 파일을 열고 읽고 닫는다. 

    //JSON 
    string playerFilename = "player.json"; // 플레이어 상태 저장 
    string playerPath; // json 파일 저장경로 
   public PlayerData playerData = new PlayerData();
    //RoomData roomData = new RoomData();

    private void Awake()
    {
        DontDestroyOnLoad(lobby);
        clientName = GameObject.Find("idbox").GetComponentInChildren<Text>().text;
        roomManager = roomListPrefab.GetComponent<RoomManager>();
    }

    void Start()
    {
        // 프로젝트 디렉토리에 파일을 경로를 만들고 읽고 쓰는 작업을 할수있다.
        playerPath = Application.persistentDataPath + "/" + playerFilename;
        // roomPath = Application.persistentDataPath + "/" + roomFilename;
        // Debug.Log(playerPath); 
       // StartCoroutine(RefreshTimer());

    }
    
    // 업데이트 
    private void Update()
    {

        //Debug.Log("로비");
        ScrollManager();// 스크롤 조절 
        RoomScrollManager(); //룸 스크롤 조절 
        if (socketReady) // 소켓이 준비가 되어있으면 = 서버연결이 되어있으면 
        {


            // 새로고침 버튼을 누를때만 true  -> 다시 false
            if (playerData.refreshOn)
            {
                playerData.refreshOn = false;
                JsonWrapper wrapper = new JsonWrapper();
                wrapper.playerData = playerData;
                string player = JsonUtility.ToJson(wrapper, true);
                System.IO.File.WriteAllText(playerPath, player); // json 파일에 작성 
            }

            // 게임방장이 방을 나가면 
            if (playerData.panelOff)
            {
                playerData.ownerName = "";
                playerData.panelOff = false;
                JsonWrapper wrapper = new JsonWrapper();
                wrapper.playerData = playerData;
                string player = JsonUtility.ToJson(wrapper, true);
                System.IO.File.WriteAllText(playerPath, player); // json 파일에 작성 
            }

            // 읽을 데이터가 없는경우  networkstream.read 메서드가 차된되는데
            // dataavailable 속성을 사용하여 들어오는 네트워크버퍼에 읽을 데이터가 대기중인지 여부를 확인한다. 
            if (stream.DataAvailable) // dataavailable이 true를 반환하는 경우 read 연산이 즉시 완료된다. 
            {
                // 자바서버로 부터 넘어온 메세지 
                string data = reader.ReadLine();  // 텍스트파일을 한번에 한줄씩 읽기 

                if (data != null)
                {
                    //처음 서버에 접속했을때 게임방 리스트 파싱
                    ConnectLobbyRoom(data);

                    // 다른클라이언트가 방을 생성했을때 
                    LobbyAddRoom(data);

                    //메세지를 파싱하여 업로드 
                    LobbyMessage(data);

                    // 방장이 방을 생성했을때 정보 뿌리기
                    GetRoomDataForOwner(data);
                    // 방문자가 방에 들어올때 정보 뿌리기
                    GetRoomDataForClient(data);
                    // 방문자가 방에 들어갈때 서버에 자기정보를 보낸다
                    AddPlayerListClient(data);
                    // 방문자가 방에서 나갈때 다른 플레이어에게 알린다. 
                    RemovePlayerListClient(data);
                    // 룸에서 새로고침  
                    RoomRefreshPlayerList(data);
                    //방장 변경!
                    ChangeRoomOwner(data);
                    // 방에 아무도 없으면 로비에서 방을 지운다! 모두!
                    RemoveRoom(data);
                    //레디버튼
                    OnReadyButton(data);
                    //레디 취소 
                    OffReadyButton(data);
                    // 게임화면전환
                    NetworkSceneChange(data);
                    //룸메세지
                    RoomMessage(data);
                    //강제퇴장
                    KickOut(data);
                }
            }
        }

    }

    
    // 서버와의 연결  
    public void ConnectToServer()
    {
        // If already connected, ignore this function
        if (socketReady)
            return;  // 소켓준비가 되어있면 리턴한다. 
       //clientName =GameObject.Find("Lobby/LobbyBox/titleCanvas/screen_title/background/InputField_username").GetComponent<InputField>().text;


        if (clientName.Equals(""))
        {
            // 팝업띄우기 
            var pop = GameObject.Find("titleCanvas").GetComponent<PopupOpener>();
            pop.message.GetComponent<Text>().text = "아이디를 입력하세요.";
            pop.OpenPopup();
            return;
        }
         
        //소켓생성

        try
        {
            socket = new TcpClient(host, port);   // tcpclient(주소,포트)  소켓을 만들고
            stream = socket.GetStream();  // tcp네트워크 스트림을 리턴한다.  이 네트워크 스트림을 이용해서 네트워크로 데이터를 송수신 하게된다.
            writer = new StreamWriter(stream, Encoding.Unicode);  // 데이터 열고 작성
            reader = new StreamReader(stream, Encoding.Unicode); // 데이터  열고 읽기
            socketReady = true;  // 소켓이 준비가 되었다.  
            StatusSend(clientName);

            //JSON 저장 
            // player 형식의 인스터트를 생성 
            // 클라이언트의 상태를 업데이트 
            playerData.ownerName = "";
            playerData.message = "";
            playerData.roomName = "";
            playerData.username = clientName;
            playerData.roomPasswd = "";
            playerData.panelOff = false;
            playerData.inRoom = false;
            playerData.roomOwner = false;
            playerData.refreshOn = false;
            playerData.privateRoom = false;
            playerData.groupCheck = false;
            playerData.readyCheck = false;
            playerData.startGame = false;
            playerData.playerCount = 0;
            PlayerSaveData();
            playerCount = 0;
            string msg = "접속하였습니다.";
            //처음접속시 채팅장에 접속확인 
            GameObject go = Instantiate(messagePrefab, chatContainer.transform) as GameObject; // as 는 참조형식  (형변환) 
            go.GetComponentInChildren<Text>().text = msg;
            chatCount++;
            go.name = "ChatMessage" + chatCount.ToString();
            lastChat = go.name;
        }
        catch (Exception e)
        {
            Debug.Log("Socket error : " + e.Message);
            // 팝업띄우기 
            var pop = GameObject.Find("titleCanvas").GetComponent<PopupOpener>();
            pop.message.GetComponent<Text>().text = e.Message;
            pop.OpenPopup();
            return;
        }

        // canvas lobby로 전환 
        var ct = GameObject.Find("titleCanvas").GetComponent<TitleFade>();
        ct.Fade();
        // 인풋 비우기 
        //GameObject.Find("InputField_username").GetComponent<InputField>().text = "";
    }

    //처음 서버에 접속했을때 게임방 리스트 파싱 (업데이트)
    public void ConnectLobbyRoom(string data)
    {
        if (playerData.inRoom)
            return;
     //   Debug.Log("처음접속 시 룸 정보 : " + data);
        //처음 접속시 게임방 리스를 json어레이에서 파싱해서 보여준다.  
        try
        {
            JObject obj = JObject.Parse(data);
            string room = obj["roomList"].ToString();
            // 룸 리스트담겨있는 json array 파싱
            JArray roomList = JArray.Parse(room);
            {
                foreach (JObject o in roomList.Children<JObject>())
                {
                    string owner = o["owner"].ToString();
                    string rName = o["roomName"].ToString();
                    string pw = o["password"].ToString();
                    string pr = o["private"].ToString();
                    string gc = o["groupCheck"].ToString();
                    bool privateRoom = Convert.ToBoolean(pr);
                    bool groupCheck = Convert.ToBoolean(gc);
                    //Debug.Log("groupCheck" + groupCheck);

                    string player = o["playerList"].ToString();
                    JArray playerList = JArray.Parse(player);
                    {
                        //플레이어 리스트에서 방에참여한 회원정보 가져오기 
                        foreach (JObject o2 in playerList.Children<JObject>())
                        {
                            string id = o2["id"].ToString();
                            //string msg = o2["message"].ToString();

                        }
                    }

                    // 서버로부터 전달받은 룸정보에 방장이름이 비어있지 않으면 방리스트 추가된다. 
                    if (!owner.Equals(""))
                    {
                        //Debug.Log(playerList.Count);
                        GameObject mRoom = Instantiate(roomListPrefab, roomListContainer.transform) as GameObject; // as 는 참조형식  (형변환) 
                        if (groupCheck)
                        {
                            mRoom.GetComponentInChildren<Text>().text = playerList.Count + "/4     " + rName;  // 단체전 방이름과 인원수 
                        }
                        else
                        {
                            mRoom.GetComponentInChildren<Text>().text = playerList.Count + "/2     " + rName;  //개인전 방이름과 인원수 
                        }
                        mRoom.GetComponentInChildren<InputField>().text = pw; // 비밀번호 셋팅 
                        mRoom.GetComponentInChildren<InputField>().name = groupCheck.ToString(); // 개인전 단체전 구분  
                        mRoom.GetComponentInChildren<Text>().name = playerList.Count.ToString(); // 인원수체크 
                        if (privateRoom)
                            {
                                mRoom.GetComponentInChildren<RawImage>().enabled = true;
                            }
                            else
                            {
                                mRoom.GetComponentInChildren<RawImage>().enabled = false;
                            }
                       
                        mRoom.name = owner;
                            mRoomList.Add(mRoom);
                        //     Debug.Log("최초의 방갯수"+mRoomList.Count);
                        //Debug.Log(" 오브젝트 이름  :  " +mRoom.tag);
                     
                    }
                }
            }
        }
        catch (System.Net.Sockets.SocketException socketEx) { Console.WriteLine("[Error]:{0}", socketEx.Message); }
        catch (System.Exception commonEx) { Console.WriteLine("[Error]:{0}", commonEx.Message); }
    }
    // 다른클라이언트가 방을 생성했을때 (업데이트_)
    public void LobbyAddRoom(string data)
    {
        if (playerData.inRoom)
            return;
        // 서버에서 방이 추가되었을때 방을 추가해준다. 
        try
        {
            JObject obj = JObject.Parse(data);
            // aadRoom 으로 넘어온 룸의 정보를 추가해준다.  
            string owner = obj["addRoom"]["owner"].ToString();
            string room = obj["addRoom"]["roomName"].ToString();
            string pList = obj["addRoom"]["playerList"].ToString();
            string pr = obj["addRoom"]["private"].ToString();
            bool privateRoom = Convert.ToBoolean(pr);
            JArray playerList = JArray.Parse(pList);
            {
                //플레이어 리스트에서 방에참여한 회원정보 가져오기 
                foreach (JObject o2 in playerList.Children<JObject>())
                {
                    string id = o2["id"].ToString();
                    // string msg = o2["message"].ToString();
                }
            }
            OnRefreshBtn(); // 추가되면 모두가 리셋 
            //  Debug.Log("추가 하면" + mRoomList.Count);
            //  Debug.Log(" 오브젝트 이름  :  " + mRoom.name);

        }
        catch (System.Net.Sockets.SocketException socketEx) { Console.WriteLine("[Error]:{0}", socketEx.Message); }
        catch (System.Exception commonEx) { Console.WriteLine("[Error]:{0}", commonEx.Message); }
    }
    //메세지를 파싱하여 업로드 (업데이트)
    public void LobbyMessage(string data)
    {
        if (playerData.inRoom)
            return;
        var cm = GameObject.Find(lastChat).GetComponent<ChatItemManager>();
        chatItemH = cm.messagePrefab.GetComponent<RectTransform>().rect.height;
        // 메세지를 json object에서 파싱하여 업로드한다. 
        try
        {
            JObject obj = JObject.Parse(data);
            //채팅 스크롤 맨아래로 자동으로 이동 
            // 메세지 보냈을때만 실행하도록 
            chatSend = true;
            string message = obj["message"].ToString();
            string name = obj["username"].ToString();
            string ir = obj["inRoom"].ToString();
            string msg = name + " : " + message;
            bool getInroom = Convert.ToBoolean(ir);
            if (getInroom)
                return;
            GameObject go = Instantiate(messagePrefab, chatContainer.transform) as GameObject; // as 는 참조형식  (형변환) 
            go.GetComponentInChildren<Text>().text = msg;
            chatCount++;
            go.name = "ChatMessage"+chatCount.ToString();
            lastChat = go.name;
        }
        catch (System.Net.Sockets.SocketException socketEx) { Console.WriteLine("[Error]:{0}", socketEx.Message); }
        catch (System.Exception commonEx) { Console.WriteLine("[Error]:{0}", commonEx.Message); }

        // 인풋 비우기 
        GameObject.Find("InputField_robbyMessage").GetComponent<InputField>().text = "";
    }


    //*********************************************************************************************
    //룸에서 메세지 업로드 
    // message 컨테이너 
    public void RoomMessage(string data)
    {
        if (!playerData.inRoom)
            return;
        if (playerData.startGame)
            return;
        //var cm = GameObject.Find(roomLastChat).GetComponent<RoomChatItem>();
        //roomChatItemH = cm.roomMessagePrefab.GetComponent<RectTransform>().rect.height;
        try
        {
            JObject obj = JObject.Parse(data);
            roomChatSend = true;
            string message = obj["message"].ToString();  // 전달받은 메세지 
            string name = obj["username"].ToString();   /// 메세지보낸사람
            string ow = obj["ownerName"].ToString(); // 메세지 보낸 방의 방장 
            string msg = name + " : " + message; 
            string ir = obj["inRoom"].ToString(); // 룸안에서 온 메세지인지 로비에서온 메세지인지 구분 
            bool getInroom = Convert.ToBoolean(ir);
            if (!getInroom)
                return;
            if (!ow.Equals(ownerName))
                return;
            if (message.Equals("refreshvity"))
                return;
            if (message.Equals("gameoverMsg"))
                return;
            GameObject go = Instantiate(roomMessagePrefab, roomChatContent.transform) as GameObject; // as 는 참조형식  (형변환) 
            go.GetComponentInChildren<Text>().text = msg;
            roomChatCount++;
            go.name = "RoomChatMessage" + roomChatCount.ToString();
            roomLastChat = go.name;
            mRoomChatList.Add(go);
            if (name.Equals(clientName))
            {
                playerData.message = "";
                PlayerSaveData();
            }
        }
        catch (System.Net.Sockets.SocketException socketEx) { Console.WriteLine("[Error]:{0}", socketEx.Message); }
        catch (System.Exception commonEx) { Console.WriteLine("[Error]:{0}", commonEx.Message); }
        // 인풋 비우기 
        GameObject.Find("InputField_roomMessage").GetComponent<InputField>().text = "";

    }
    //룸에서 메세지보내기 
    public void RoomSendToMessage()
    {
        if (!socketReady)
            return;
        if (!playerData.inRoom)
            return;

        // sendinput 이라는 게임오브젝트에 들어있는 텍스트를 message 라는 스트링변수에 담고 
        string message = GameObject.Find("InputField_roomMessage").GetComponent<InputField>().text;
        // Debug.Log("MESSAGE" + message);
        // 연결되지않으면 리턴 
        playerData.message = message;
        PlayerSaveData();


    }
    // 룸 컨테이너 온오프 
    public void RoomChatOnOff()
    {
        if (!roomChat)
        {
            roomChatContatiner.SetActive(true); // 채팅창활성화
            roomOffIcon.SetActive(true);
            roomOnIcon.SetActive(false);  // 채팅창 아이콘 내리는걸로 변경 
            messageView.SetActive(false);
            roomChat = true;
        }
        else
        {
            roomChatContatiner.SetActive(false); // 채팅창비활성
            roomOffIcon.SetActive(false);
            roomOnIcon.SetActive(true);  // 채팅창 아이콘 내리는걸로 변경 
            messageView.SetActive(true);
            roomChat = false;
        }
    }

    //*********************************************************************************************
    
    // 로비새로고침버튼 
    public void OnRefreshBtn()
    {

        // 연결되지않으면 리턴 
        if (!socketReady)
            return;
        //Debug.Log("리스트" + mRoomList.Count);
        foreach (GameObject obj in mRoomList)
        {
            if (obj == null)
                return;
            //   Debug.Log(obj);
            // 리스트에 저장해놓은 이름으로 오브젝트 삭제
            DestroyImmediate(GameObject.Find(obj.name));
        }
        while (mRoomList.Count > 0)
            mRoomList.RemoveAt(0);
        playerData.refreshOn = true;
        //Debug.Log("게임방 리스트 새로고침!");
        PlayerSaveData();
    }


    //방들어가기
    public void InGameRoom(string name)
    { 
        ownerName = name;
        //Debug.Log("ownerName : " + ownerName);
       // StopAllCoroutines();
        try
        {
            playerData.message = "";
            playerData.ownerName = ownerName;
            playerData.inRoom = true;
            PlayerSaveData();
        }
        catch (Exception e)
        {
            Debug.Log("Socket error : " + e.Message);
        }

        roomHeart.GetComponent<HeartCountManager>().HeartUpdate();

        roomChatContatiner.SetActive(true); // 채팅창활성화
        roomOffIcon.SetActive(true);
        roomOnIcon.SetActive(false);  // 채팅창 아이콘 내리는걸로 변경 
        messageView.SetActive(false);
        roomChat = true;

        Room_Active();
    }
    //방생성
    public void CreateGameRoom()
    {
        if (playerData.roomOwner)
            return;
        var lc = GameObject.Find("lobbyCanvas").GetComponent<CreateGameManager>();
        roomName = GameObject.Find("InputField_roomname").GetComponent<InputField>().text;
        roomPass = GameObject.Find("InputField_roompasswd").GetComponent<InputField>().text;
        //oneToOne = lc.onetoone;
        //groupMatch = lc.groupMatch;
        //privateRoom = lc.privateRoom;

        //Debug.Log("개인전 : " + lc.onetoone.isOn);
        //Debug.Log("단체전 : " + lc.groupMatch.isOn);
        try
        {
            //json 
            // 클라이언트의 상태를 업데이트 
            playerData.username = clientName;
            playerData.roomName = roomName;
            playerData.message = "";
            playerData.inRoom = true;
            playerData.roomOwner = true;
            playerData.ownerName = clientName;
            playerData.privateRoom = lc.privateRoom.isOn;
            playerData.roomPasswd = roomPass;
            playerData.groupCheck = lc.groupMatch.isOn;
            playerData.playerCount = 1;
            //Debug.Log("roomPass : " + roomPass);
            //Debug.Log("privateRoom : " + playerData.privateRoom);

            PlayerSaveData();
        }
        catch (Exception e)
        {
            Debug.Log("Socket error : " + e.Message);
        }
        //StopAllCoroutines();
        roomChatContatiner.SetActive(true); // 채팅창활성화
        roomOffIcon.SetActive(true);
        roomOnIcon.SetActive(false);  // 채팅창 아이콘 내리는걸로 변경 
        messageView.SetActive(false);
        roomChat = true;
        playerCount = 1;
        lc.ClosePopup();
        ownerName = clientName;
        Room_Active();
    }
    //게임방 나가기
    public void CloseRoomPanel()
    {
        if (playerData.readyCheck)
        {
            var rum = GameObject.Find("player_box").GetComponent<RoomUiManager>();
            Debug.Log("레디 안풀고 나감");
            rum.ready_on.SetActive(true);
            rum.ready_off.SetActive(false);
            rum.OffReadyButton();
        }
        //Debug.Log("리스트" + mRoomList.Count);
        foreach (GameObject obj in mPlayerList)
        {
            //   Debug.Log(obj);
            // 리스트에 저장해놓은 이름으로 오브젝트 삭제
            DestroyImmediate(GameObject.Find(obj.name));
        }
        while (mPlayerList.Count > 0)
            mPlayerList.RemoveAt(0);
        //Debug.Log("closeRoomPanel 실행확인");
        while (mReadyList.Count > 0)
            mReadyList.RemoveAt(0);
        try
        {
            //json 
            // 클라이언트의 상태를 업데이트 
            playerData.roomName = "";
            playerData.inRoom = false;
            playerData.roomOwner = false;
            playerData.refreshOn = false;
            playerData.privateRoom = false;
         //   playerData.groupCheck = false;
            playerData.roomPasswd = "";
            playerData.message = "";
            playerData.panelOff = true;
            playerData.readyCheck = false;
            playerData.playerCount = 0;
           
            PlayerSaveData();
        }
        catch (Exception e)
        {
            Debug.Log("Socket error : " + e.Message);
        }

        foreach (GameObject obj in mRoomChatList)
        {
            if (obj == null)
                return;
            //   Debug.Log(obj);
            // 리스트에 저장해놓은 이름으로 오브젝트 삭제
            DestroyImmediate(GameObject.Find(obj.name));
        }
        while (mRoomChatList.Count > 0)
            mRoomChatList.RemoveAt(0);
        roomChatContatiner.SetActive(false); // 채팅창비활성화
        roomOffIcon.SetActive(false);
        roomOnIcon.SetActive(true);  // 채팅창 아이콘 내리는걸로 변경 
        roomChat = false;
        roomChatCount = 0;
        playerCount = 0;
        groupCount = 0;
        ownerName = "";
        roomRefreshOn = false;
        createRoom = false;
        Lobby_Active();
        //StartCoroutine(RefreshTimer());
        OnRefreshBtn();
    }


    //방장이 방을만들때 처음만 실행 
    public void GetRoomDataForOwner(string data)
    {
        if (!playerData.roomOwner) // 방장이 아니면 실행안함 
            return;
        if (roomRefreshOn) // 새로고침누를때 실행 안함. 
            return;
        if (createRoom)
            return;
        //  Debug.Log("새로고침시 방장은 여기로");
        try
        {
            JObject obj = JObject.Parse(data);
          //  Debug.Log("방장받아라!");
            // aadRoom 으로 넘어온 룸의 정보를 추가해준다.  
            string owner = obj["addRoom"]["owner"].ToString();
            string room = obj["addRoom"]["roomName"].ToString();
            string pList = obj["addRoom"]["playerList"].ToString();

            JArray playerList = JArray.Parse(pList);
            {
                //플레이어 리스트에서 방에참여한 회원정보 가져오기 
                foreach (JObject o2 in playerList.Children<JObject>())
                {
                    string id = o2["id"].ToString();
                    // string msg = o2["message"].ToString();
                    // Debug.Log("id" + id);
                    GameObject p = Instantiate(playerPrefab, playerContainer.transform) as GameObject;
                    p.GetComponentInChildren<Text>().text = id;
                    p.GetComponent<RoomUiManager>().ownerIcon.SetActive(true);
                    p.GetComponent<RoomUiManager>().start_on.SetActive(true);
                    ownerName = owner;
                    mPlayerList.Add(p);
                    createRoom = true;
                    
                    roomHeart.GetComponent<HeartCountManager>().HeartUpdate();

                }
                //GameObject msg = Instantiate(roomMessagePrefab, roomChatContainer.transform) as GameObject;
                //msg.GetComponentInChildren<Text>().text = clientName+ "님이 방을 생성하였습니다.";
                //mChatList.Add(msg);
            }
        }
        catch (System.Net.Sockets.SocketException socketEx) { Console.WriteLine("[Error]:{0}", socketEx.Message); }
        catch (System.Exception commonEx) { Console.WriteLine("[Error]:{0}", commonEx.Message); }
    }
    // 방문자에게 정보뿌리기 
    public void GetRoomDataForClient(string data)
    {
        if (!playerData.inRoom) // 로비이면 실행안함 
            return;
        if (roomRefreshOn) // 룸에서 새로고침누르면 실행안함 
            return;
        if (playerData.readyCheck) // 레디버튼누르면 실행안함 
            return;
        try
        {
            JObject obj = JObject.Parse(data);
            //Debug.Log("방문자받아라!");
            string room = obj["roomList"].ToString();
            // 룸 리스트담겨있는 json array 파싱
            JArray roomList = JArray.Parse(room);
            {
                foreach (JObject o in roomList.Children<JObject>())
                {
                    string owner = o["owner"].ToString();
                    string rName = o["roomName"].ToString();
                    string player = o["playerList"].ToString();
                    if (owner == ownerName)
                    {
                        JArray playerList = JArray.Parse(player);
                        {
                            //Debug.Log(" 인원수 " + playerList.Count);
                            playerData.playerCount = playerList.Count + 1;
                            PlayerSaveData();

                            //인원수초과 계산
                            if (playerData.playerCount > 4)
                            {
                                CloseRoomPanel();
                                return;
                            }

                            //플레이어 리스트에서 방에참여한 회원정보 가져오기 
                            foreach (JObject o2 in playerList.Children<JObject>())
                            {
                                string id = o2["id"].ToString();
                                string msg = o2["message"].ToString();
                                string ready = o2["ready"].ToString();
                                bool readyCheck = Convert.ToBoolean(ready);

                                GameObject p = Instantiate(playerPrefab, playerContainer.transform) as GameObject;
                                p.GetComponentInChildren<Text>().text = id;
                                if (owner.Equals(id))
                                {
                                    p.GetComponent<RoomUiManager>().ownerIcon.SetActive(true);
                                }
                                else
                                {
                                    if (playerData.roomOwner)
                                    {
                                        p.GetComponent<RoomUiManager>().kickOut_icon.SetActive(true);
                                    }
                                }

                                if (readyCheck)
                                {
                                    p.GetComponent<RoomUiManager>().readyIcon.SetActive(true);
                                    mReadyList.Add(id);
                                }
                                mPlayerList.Add(p);
                                playerCount = playerData.playerCount;
                                //   Debug.Log(ownerName);
                                // Debug.Log("id!!" + id);
                                //    Debug.Log("msg!!!" + msg);
                            }
                        }
                    }
                }
            }
        }
        catch (System.Net.Sockets.SocketException socketEx) { Console.WriteLine("[Error]:{0}", socketEx.Message); }
        catch (System.Exception commonEx) { Console.WriteLine("[Error]:{0}", commonEx.Message); }
       

    }
    // 방들어갈때 플레이어리스트 새로고침해준다. 
    public void AddPlayerListClient(string data)
    {
        if (roomRefreshOn) // 게임방 새로고침 누르면 실행안함 
            return;
        if (!playerData.inRoom) // 로비에있으면 실행안함 
            return;
        // 서버에서 방이 추가되었을때
        try
        {
            JObject obj = JObject.Parse(data);

          //  Debug.Log("손님리스트 추가!");
            //Debug.Log(obj.ToString());
            // aadRoom 으로 넘어온 룸의 정보를 추가해준다.  
            string id = obj["addPlayer"]["id"].ToString();
            string message = obj["addPlayer"]["message"].ToString();
            string ow = obj["addPlayer"]["ow"].ToString();
            if (!ow.Equals(playerData.ownerName))
            return;
            OnRefreshRoom();// 방 새로고침 
        }
        catch (System.Net.Sockets.SocketException socketEx) { Console.WriteLine("[Error]:{0}", socketEx.Message); }
        catch (System.Exception commonEx) { Console.WriteLine("[Error]:{0}", commonEx.Message); }
    
    }
    // 방을 나갈때 플레이어 리스트 새로고침해주기 
    public void RemovePlayerListClient(string data)
    {
        if (roomRefreshOn) // 게임방 새로고침 누르면 실행안함 
            return;
        if (!playerData.inRoom) // 로비에있으면 실행안함 
            return;

        try
        {
            JObject obj = JObject.Parse(data);
            //Debug.Log(obj.ToString());
            // aadRoom 으로 넘어온 룸의 정보를 추가해준다.  
            string id = obj["removePlayer"]["id"].ToString();
            string message = obj["removePlayer"]["message"].ToString();
            string ow = obj["removePlayer"]["ow"].ToString();
            if (!ow.Equals(playerData.ownerName))
                return;
            mReadyList.Remove(id);
            playerData.playerCount = playerData.playerCount - 1;
            playerCount = playerData.playerCount;
            OnRefreshRoom();// 방 새로고침 
        }
        catch (System.Net.Sockets.SocketException socketEx) { Console.WriteLine("[Error]:{0}", socketEx.Message); }
        catch (System.Exception commonEx) { Console.WriteLine("[Error]:{0}", commonEx.Message); }

    }
    // 방장이 방을 나갈때 사람이 남아있으면 플레이어리스트 새로고침
    public void ChangeRoomOwner(string data)
    {
        if (playerData.roomOwner)
            return;
        if (roomRefreshOn) // 게임방 새로고침 누르면 실행안함 
            return;
        if (!playerData.inRoom) // 로비에있으면 실행안함 
            return;

        try
        {
            JObject obj = JObject.Parse(data);
           // Debug.Log("방장나감 : ");
            // aadRoom 으로 넘어온 룸의 정보를 추가해준다.  
            string currentOw = obj["changeOwner"]["currentOwner"].ToString();
            string changeOw = obj["changeOwner"]["changeOwner"].ToString();     
            //넘어온 방장이름과 현재 방장이름이 같고 바뀔이름이 자신의 아이디와 같으면 
            if (currentOw.Equals(playerData.ownerName)&& changeOw.Equals(playerData.username))
            {
                // 해당유저를 방장으로 세팅해준다. 
                mReadyList.Remove(currentOw);
                ownerName = changeOw;
                playerData.roomOwner = true;
                playerData.ownerName = changeOw;
                playerData.playerCount = playerData.playerCount-1;
                OnRefreshRoom();
               // Debug.Log("두번째유저 방장되고 정보바꾼다. ");
            }
            //넘어온 방장이름과 현재 방장이름이 같고 바뀔이름이 자신의 아이디와 같지않으면 
            if (currentOw.Equals(playerData.ownerName) && !changeOw.Equals(playerData.username))
            {
                // 방장정보를 업데이트해준다. 
                mReadyList.Remove(currentOw);
                ownerName = changeOw;
                playerData.ownerName = changeOw;
                OnRefreshRoom();
               // Debug.Log("나머지유저들 방장정보를 바꾼다.  ");
            }


        }
        catch (System.Net.Sockets.SocketException socketEx) { Console.WriteLine("[Error]:{0}", socketEx.Message); }
        catch (System.Exception commonEx) { Console.WriteLine("[Error]:{0}", commonEx.Message); }

    }
    // 방장이 방을 나가는데 사람이 없어서 방을 지워야 할때 
    public void RemoveRoom(string data)
    {
        if (playerData.inRoom) // 로비에 없으면 실행하지 않음 . 
            return;
        try
        {
            JObject obj = JObject.Parse(data);
            // Debug.Log("방장나감 : ");
            // aadRoom 으로 넘어온 룸의 정보를 추가해준다.  
            string removeRoom = obj["removeRoom"]["roomOwner"].ToString();
            if (removeRoom != null)
            {
                if (!removeRoom.Equals(playerData.username))
                    OnRefreshBtn();
            }
        }
        catch (System.Net.Sockets.SocketException socketEx) { Console.WriteLine("[Error]:{0}", socketEx.Message); }
        catch (System.Exception commonEx) { Console.WriteLine("[Error]:{0}", commonEx.Message); }

    }

    //강제퇴장 
    public void KickOut(string data)
    {
        if (!playerData.inRoom)
            return;
        if (playerData.roomOwner)
            return;
        try
        {
            JObject obj = JObject.Parse(data);
            //Debug.Log("ready on : " + data);

            //Debug.Log(obj.ToString());
            // aadRoom 으로 넘어온 룸의 정보를 추가해준다.  
            string id = obj["kickOffPlayer"]["id"].ToString();
            string ow = obj["kickOffPlayer"]["ow"].ToString();
            if (!ow.Equals(ownerName))
                return;
            Debug.Log("id"+id);
            Debug.Log("username" + playerData.username);

            if (id.Equals(playerData.username))
            {
                CloseRoomPanel();
            }
        }
        catch (System.Net.Sockets.SocketException socketEx) { Console.WriteLine("[Error]:{0}", socketEx.Message); }
        catch (System.Exception commonEx) { Console.WriteLine("[Error]:{0}", commonEx.Message); }
    }
    //레디 
    public void OnReadyButton(string data)
    {
        if (!playerData.inRoom)
            return;
        if (playerData.startGame)
            return;
        try
        {
            JObject obj = JObject.Parse(data);
            //Debug.Log("ready on : " + data);

            //Debug.Log(obj.ToString());
            // aadRoom 으로 넘어온 룸의 정보를 추가해준다.  
            string id = obj["readyPlayer"]["id"].ToString();
            string ow = obj["readyPlayer"]["ow"].ToString();
            if (!ow.Equals(playerData.ownerName))
                return;
            mReadyList.Add(id);
            OnRefreshRoom();// 방 새로고침 
        }
        catch (System.Net.Sockets.SocketException socketEx) { Console.WriteLine("[Error]:{0}", socketEx.Message); }
        catch (System.Exception commonEx) { Console.WriteLine("[Error]:{0}", commonEx.Message); }

    }
    //레디 취소 
    public void OffReadyButton(string data)
    {
        if (!playerData.inRoom)
            return;
        if (playerData.startGame)
            return;
        try
        {
            JObject obj = JObject.Parse(data);
            //Debug.Log("ready off : " + data);

            //Debug.Log(obj.ToString());
            // aadRoom 으로 넘어온 룸의 정보를 추가해준다.  
            string id = obj["readyOffPlayer"]["id"].ToString();
            string ow = obj["readyOffPlayer"]["ow"].ToString();
            if (!ow.Equals(playerData.ownerName))
                return;
            mReadyList.Remove(id);
            OnRefreshRoom();// 방 새로고침 
        }
        catch (System.Net.Sockets.SocketException socketEx) { Console.WriteLine("[Error]:{0}", socketEx.Message); }
        catch (System.Exception commonEx) { Console.WriteLine("[Error]:{0}", commonEx.Message); }

    }
    //룸에서 새로고침하고 회원정보 다시 받기 
    public void RoomRefreshPlayerList(string data)
    {
        if (!playerData.inRoom)
            return;
        if (!roomRefreshOn)
            return;
        if (playerData.startGame)
            return;
        try
        {
            JObject obj = JObject.Parse(data);
          //Debug.Log("새로고침 : " + data);
            string room = obj["roomList"].ToString();
            // 룸 리스트담겨있는 json array 파싱
            JArray roomList = JArray.Parse(room);
            {
                foreach (JObject o in roomList.Children<JObject>())
                {
                    string owner = o["owner"].ToString();
                    string rName = o["roomName"].ToString();
                    string player = o["playerList"].ToString();
                    if (owner == ownerName)
                    {
                        JArray playerList = JArray.Parse(player);
                        {
                          //  Debug.Log("새로고침 playerList" + playerList);
                            //플레이어 리스트에서 방에참여한 회원정보 가져오기 
                            foreach (JObject o2 in playerList.Children<JObject>())
                            {
                                string id = o2["id"].ToString();
                                string msg = o2["message"].ToString();
                                string ready = o2["ready"].ToString();
                                bool readyCheck = Convert.ToBoolean(ready);

                                GameObject p = Instantiate(playerPrefab, playerContainer.transform) as GameObject;
                                p.GetComponentInChildren<Text>().text = id;
                                if (owner.Equals(id))
                                {
                                    p.GetComponent<RoomUiManager>().ownerIcon.SetActive(true);
                                }
                                else
                                {
                                    if (playerData.roomOwner)
                                    {
                                        p.GetComponent<RoomUiManager>().kickOut_icon.SetActive(true);
                                    }
                                }
                                if (owner.Equals(clientName))
                                {
                                    p.GetComponent<RoomUiManager>().start_on.SetActive(true);
                                }
                                if (readyCheck)
                                {
                                    p.GetComponent<RoomUiManager>().readyIcon.SetActive(true);
                                }
                                mPlayerList.Add(p);
                                roomRefreshOn = false;


                                // Debug.Log(ownerName);
                                // Debug.Log("id!!" + id);
                                //    Debug.Log("msg!!!" + msg);
                            }
                        }
                    }
                }
            }
        }
        catch (System.Net.Sockets.SocketException socketEx) { Console.WriteLine("[Error]:{0}", socketEx.Message); }
        catch (System.Exception commonEx) { Console.WriteLine("[Error]:{0}", commonEx.Message); }
    }
    //룸에서의 새로고침버튼
    public void OnRefreshRoom()
    {
        // 연결되지않으면 리턴 
        if (!socketReady)
            return;
        
        //Debug.Log("리스트" + mRoomList.Count);
        foreach (GameObject obj in mPlayerList)
        {
            if (obj == null)
                return;
            //   Debug.Log(obj);
            // 리스트에 저장해놓은 이름으로 오브젝트 삭제
            DestroyImmediate(GameObject.Find(obj.name));
        }
        while (mPlayerList.Count > 0)
            mPlayerList.RemoveAt(0);

        roomRefreshOn = true;
        playerData.message = "refreshvity";
        playerData.refreshOn = true;
    //    Debug.Log("게임방 리스트 새로고침!");
     PlayerSaveData();
    }
 

    //클라이언트의 상태를 나타내는 JSON 데이터 저장   
    public void PlayerSaveData()
    {
        // json 데이터의 포장지 느낌   
        JsonWrapper wrapper = new JsonWrapper();
        wrapper.playerData = playerData;
        string player = JsonUtility.ToJson(wrapper, true);
        System.IO.File.WriteAllText(playerPath, player); // json 파일에 작성 

        // MemoryStream 은 스트림데이터를 파일이나 소켓대신 메모리에 직접 출력한다. 
        // 출력대상이 메모리인것을 제외하면 다른 stream 클래스와 동일하다. 
        MemoryStream memoryStream = new MemoryStream();
        //.NET 객체를 JSON으로 변환하고 다시 복원할때 사용한다.  
        DataContractJsonSerializer serPlayer = new DataContractJsonSerializer(typeof(JsonWrapper));
        serPlayer.WriteObject(memoryStream, wrapper);
        memoryStream.Position = 0;
        StreamReader sr = new StreamReader(memoryStream);
        StatusSend(sr.ReadToEnd());
        //Debug.Log(sr.ReadToEnd());

    }
    // v플레이어의 상태 서버와 주고받기 
    private void StatusSend(string data)
    {
        // 연결되지않으면 리턴 
        if (!socketReady)
            return;
        writer.WriteLine(data);  // 텍스트파일 한줄씩 쓰기 
        writer.Flush(); // 버퍼링된 모든 데이터를 내부 장치에 쓴다. 
    }
        // 메세지 보내기 
    public void SendToMessage()
    {
        if (!socketReady)
            return;
       
        // sendinput 이라는 게임오브젝트에 들어있는 텍스트를 message 라는 스트링변수에 담고 
        string message = GameObject.Find("InputField_robbyMessage").GetComponent<InputField>().text;
       // Debug.Log("MESSAGE" + message);
        // 연결되지않으면 리턴 
        playerData.message = message;
        PlayerSaveData();
    }


    // canvas 전환용 
    //타이틀화면 fade
    public void Title_Active()
    {
        titleCG.alpha = 1;
        titleCG.interactable = true;
        title_canvas.enabled = true;
        lobby_canvas.enabled = false;
        room_canvas.enabled = false;
        item_canvas.enabled = false;
        itemCharacter.SetActive(false);
    }
    //로비화면 fade
    public void Lobby_Active()
    {
        lobbyCG.alpha = 1;
        lobbyCG.interactable = true;
        lobby_canvas.enabled = true;
        title_canvas.enabled = false;
        room_canvas.enabled = false;
        item_canvas.enabled = false;
        itemCharacter.SetActive(false);
        screenItem.SetActive(false);
    }
    //룸화면 fade
    public void Room_Active()
    {
        roomCG.alpha = 1;
        roomCG.interactable = true;
        room_canvas.enabled = true;
        lobby_canvas.enabled = false;
        title_canvas.enabled = false;
        item_canvas.enabled = false;
        itemCharacter.SetActive(false);
        screenItem.SetActive(false);
    }

    // 아이템화면 fade 
    public void Item_Active()
    {
        itemCG.alpha = 1;
        itemCG.interactable = true;
        item_canvas.enabled = true;
        room_canvas.enabled = false;
        lobby_canvas.enabled = false;
        title_canvas.enabled = false;
        itemCharacter.SetActive(true);
        screenItem.SetActive(true);
    }
    
    //타이틀 화면으로 이동 (로그아웃 ) 
    public void MoveToTitle()
    {
        CloseSocket();
        Title_Active();

        //Debug.Log("리스트" + mRoomList.Count);
        foreach (GameObject obj in mRoomList)
        {
            if (obj == null)
                return;
            //   Debug.Log(obj);
            // 리스트에 저장해놓은 이름으로 오브젝트 삭제
            DestroyImmediate(GameObject.Find(obj.name));
        }
        while (mRoomList.Count > 0)
            mRoomList.RemoveAt(0);

        clientName = "";

    }

    public void QuitGame()
    {
        gamequitPop.SetActive(true);
    }

    public void QuitOK()
    {
        StartCoroutine(Logout());
        CloseSocket();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    public void QuitCancel()
    {
        gamequitPop.SetActive(false);
    }

    public void ItemCanvasOn()
    {
        var ct = GameObject.Find("itemCanvas").GetComponent<ItemFade>();
        ct.Fade();
    }

    public void ItemCanvasOff()
    {
        Room_Active();
    }

    //로비 채팅화면  스크롤 매니져 
    public void ScrollManager()
    {
        if (playerData.inRoom)
            return;
        float py = chatContainer.GetComponent<RectTransform>().position.y;
        float px = chatContainer.GetComponent<RectTransform>().position.x;
        float pz = chatContainer.GetComponent<RectTransform>().position.z;
        if (chatPosition.Count==0)
        {
            chatPosition.Add(py);
        }
        float minPy = chatPosition[0];
        
        // 스크롤 최소위치를 고정한다. 
        if (py < minPy)
        {
            chatContainer.transform.position = new Vector3(px, minPy, pz);
        }
        
        if (chatScroll.value > 0)
        {
            if (chatSend)
            {
                chatSend = false;
                //Debug.Log("chatscroll value" + chatScroll.value);
                chatScroll.value = 0;
            }
        }
            //룸리스트도 스크롤 최소위치 고정한다. 
            float rpy = roomListContainer.transform.position.y;
        float rpx = roomListContainer.transform.position.x;
        float rpz = roomListContainer.transform.position.z;
        // 스크롤 최소위치를 고정한다. 
        if (rpy < minPy)
        {
            roomListContainer.transform.position = new Vector3(rpx, minPy, rpz);
        }

    }

    //룸 채팅화면  스크롤 매니져 
    public void RoomScrollManager()
    {
        if (!playerData.inRoom)
            return;
        float py = roomChatContent.GetComponent<RectTransform>().position.y;
        float px = roomChatContent.GetComponent<RectTransform>().position.x;
        float pz = roomChatContent.GetComponent<RectTransform>().position.z;
        if (roomChatPosition.Count == 0)
        {
            roomChatPosition.Add(py);
        }
        float minPy = roomChatPosition[0];

        // 스크롤 최소위치를 고정한다. 
        if (py < minPy)
        {
            roomChatContent.transform.position = new Vector3(px, minPy, pz);
        }

        if (roomChatScroll.value > 0)
        {
            if (roomChatSend)
            {
                roomChatSend = false;
                //Debug.Log("chatscroll value" + chatScroll.value);
                roomChatScroll.value = 0;
            }
        }
    }


    // 게임시작을 모두에게알림
    public void StartNetworkGame()
    {
        try
        {
            playerData.startGame = true;
            PlayerSaveData();
        }
        catch (Exception e)
        {
            Debug.Log("Socket error : " + e.Message);
        }
    }


    // 게임화면전환 ****************************************************
    public void NetworkSceneChange(string data)
    {
        if (!playerData.inRoom)
            return;
        try
        {
            JObject obj = JObject.Parse(data);
            //Debug.Log("gamestart : " + data);
            //Debug.Log(obj.ToString());
            // aadRoom 으로 넘어온 룸의 정보를 추가해준다.  
            string start = obj["startPlayer"]["startGame"].ToString();
            string ow = obj["startPlayer"]["ow"].ToString();
            if (!ow.Equals(playerData.ownerName))
                return;
            playerData.startGame = true;
            groupCount = mPlayerList.Count;
            PlayerSaveData();
            
            loadingObj.SetActive(true);
            playerBox.GetComponent<RoomUiManager>().OffReadyButton();
            while (mReadyList.Count > 0)
                mReadyList.RemoveAt(0);

            StartCoroutine(GameLoadTimer());
            //NETWORK.SetActive(true);
            //var nm = GameObject.Find("Network").transform.Find("NetworkManager").GetComponent<NetworkManager>();
            //nm.username = clientName;
            //nm.owner = ownerName;
            //nm.playerNumber = playerCount;
            //nm.group = mPlayerList.Count;
            //LOBBY.SetActive(false);
        }
        catch (System.Net.Sockets.SocketException socketEx) { Console.WriteLine("[Error]:{0}", socketEx.Message); }
        catch (System.Exception commonEx) { Console.WriteLine("[Error]:{0}", commonEx.Message); }
    }

    public void ComebackLobby()
    {
        if (playerData.roomOwner)
            playerData.message = "gameoverMsg";
        else
            playerData.message = "";

        roomHeart.SetActive(true);
        roomHeart.GetComponent<HeartCountManager>().HeartUpdate();
        playerData.startGame = false;
        groupCount = 0;
        PlayerSaveData();
        StartCoroutine(ComebackTimer());
    }

    public void ComebackRoomOut()
    {
        StartCoroutine(ComebackOutTimer());
        playerData.startGame = false;
        groupCount = 0;
        PlayerSaveData();
    }
    // 게임화면전환 ****************************************************

    private IEnumerator GameLoadTimer()
    {
        yield return new WaitForSeconds(5f);// 5초뒤
        SceneManager.LoadScene("GameMap", LoadSceneMode.Single);
        //joystick.SetActive(true);
    }

    private IEnumerator ComebackTimer()
    {
        yield return new WaitForSeconds(5f);// 5초뒤
        loadingObj.SetActive(false);
        roomChatContatiner.SetActive(false); // 채팅창비활성
        roomOffIcon.SetActive(false);
        roomOnIcon.SetActive(true);  // 채팅창 아이콘 내리는걸로 변경 
        messageView.SetActive(true);
        roomChat = false;
        OnRefreshRoom();// 방 새로고침 

    }

    private IEnumerator ComebackOutTimer()
    {
        yield return new WaitForSeconds(3f);// 5초뒤

        loadingObj.SetActive(false);
        CloseRoomPanel();
    }

    IEnumerator RefreshTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);// 5초뒤
            //Debug.Log("새로고침실행");
            OnRefreshBtn();
        }        
    }

    IEnumerator Logout()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", clientName);
        using (UnityWebRequest www = UnityWebRequest.Post("http://15.164.101.199/logout.php", form))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                //Debug.Log(www.downloadHandler.text);
            }
            else
            {
                // Debug.Log(www.downloadHandler.text);
            }
        }
    }


    //소켓을 닫는다. 
    private void CloseSocket()
    {
        if (!socketReady)
            return;

        //JSON 초기화 
        playerData.playerCount = 0;
        playerData.message = "";
        playerData.username = "";
        playerData.inRoom = false;
        playerData.roomOwner = false;
        playerData.refreshOn = false;
        playerData.panelOff = false;
        playerData.roomPasswd = "";
        playerData.privateRoom = false;
        playerData.groupCheck = false;
        playerData.readyCheck = false;
        playerData.startGame = false;
        playerData.roomName = "";
        playerData.ownerName = "";
        PlayerSaveData();

        // 전부 닫아준다. 
        writer.Close();
        reader.Close();
        socket.Close();
        socketReady = false;
        chatCount = 0;
        playerCount = 0;
    }
    // 유니티 종료시 호출되는 함수 


    void OnApplicationQuit()
    {
        StartCoroutine(Logout());
        CloseSocket();
    }


    // 게임오브젝트가 비활성화 되었을때 
    // private void OnDisable()
    //{
    // CloseSocket();
    //}
}