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
using System.Threading;
using UnityEngine.UI;
using UnityEngine.Networking;

public class OtherPlayer : MonoBehaviour
{
    public GameObject cobj;
    public Transform _transform;
    public GameObject hatchet2;
    public GameObject hatchet;

    [Header("**Hand Controll**")]
    public GameObject playerHand;
    public string currentItem;
    public bool grabItem;
    public bool throwMode;

    [Header("**Random HairStyle**")]
    public GameObject Head1;
    public GameObject Head2;
    public GameObject Head3;
    public GameObject Head4;
    public GameObject Head5;


    [Header("**Random BodyStyle**")]
    public GameObject Body1;
    public GameObject Body2;
    public GameObject Body3;
    public GameObject Body4;
    public GameObject Body5;
    public GameObject nameText;
    NetworkManager _networkManager;
    Player _player;
    public const float Speed = 3f; // 캐릭터 이동스피드 
    public const float JumpForce = 4.5f; // 캐릭터 점프 높이 

    protected Rigidbody Rigidbody;   // 게임오브젝트가 물리적제어로 동작하게한다.   
    protected Quaternion LookRotation; // 사원수 EULER 좌표계의 GIMBALLOCK 현상을 해결하기위해 고안된 좌표. 
    protected Animator Animator; // 
    protected bool Grounded;  // 캐릭터가 바닥에 있을때

    public string ownerName;
    public string names;
    public byte[] bytes = new byte[65535];
    public int timer;

    UdpClient socket;
    IPEndPoint ep;
    IPEndPoint ep2;
    Thread ServerCheck_thread; // 서버에서 보내는 패킷을 체크하기 위한 스레드 
    Queue<string> netBuffer = new Queue<string>();// 버퍼를 저장하기 위한 큐 
    public string host;   // 호스트 주소 
    public int port;  // 포트번호 
    //Socket sock;
    //IPAddress ip;
    //IPEndPoint endPoint;
    object Buffer_lock = new object(); // queue 처리 충돌 방지용 lock

    //public long byteTmp;
    public string position;
    public bool socketReady;
    public bool Alive;
    public bool group;
    public bool Winner;
    //public string tmp;
    // GameObject NetworkManager;
    //IPEndPoint epRemote;
    Vector3 vec;
    string roomFilename = "room.json"; // 플레이어 상태 저장 
    string roomPath; // json 파일 저장경로 
    public RoomData roomData = new RoomData();
    public RoomWrapper wrapper = new RoomWrapper();
    //public RoomData roomData = new RoomData();
    // Start is called before the first frame update


    public int health = 10;
    public GameObject handObj;
    public BoxCollider hand;
    public CapsuleCollider CapsuleCollider;
    private void Awake()
    {
        timer = 5;
        roomPath = Application.persistentDataPath + "/" + roomFilename;
        _player = GameObject.Find("Character(Clone)").GetComponent<Player>();
        _networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
     
        roomData = _networkManager.roomData; // networkManager 스크립트 캐싱
        // 캐릭터이름지정 
        names = nameText.GetComponent<TextMesh>().text.ToString();
        // 캐릭터 오브젝트와 transform 캐싱 
        cobj = GameObject.Find(names + "(Clone)");
        _transform = cobj.transform;
        vec = transform.position;
        // 컴포넌트에 접근한다. 
        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponentInChildren<Animator>();
        hand = handObj.GetComponentInChildren<BoxCollider>();
        CapsuleCollider = cobj.GetComponentInChildren<CapsuleCollider>();

        _networkManager.mCharacter.Add(cobj);

        currentItem = "";
        grabItem = false;

        Alive = true;
        Winner = false;
        if (roomData.group.Equals("2"))
            group = false;
        if (roomData.group.Equals("4"))
            group = true;
    }
    
    private void Start()
    {
        //print(playerHealth);
        // 상대캐릭터의 좌표값을 받을 코루틴 시작 
        // ConnectToServer();
        ServerOn();
        StartCoroutine(TransformUpdate());
        StartCoroutine(Buffer_update());

    }


    // 패킷 반복적으로 받기 
    private IEnumerator Buffer_update()
    {
      //  Debug.Log("한번만 실행");
        while (true)
        {
            //yield return new WaitForSeconds(0.02f);
            yield return YieldInstructionChache.WaitForSeconds(0.02f);
            //yield return null;
            BufferSystem();
        }
    }

    void ServerOn()
    {
        socket = new UdpClient();
        ep = new IPEndPoint(IPAddress.Parse(host), port);
        socket.Connect(ep);
        ep2 = new IPEndPoint(IPAddress.Any, 0);
        //sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        //ip = IPAddress.Parse(host);
        //endPoint = new IPEndPoint(ip, port);
        //sock.Connect(endPoint);
        ServerCheck_thread = new Thread(ServerCheck);
        ServerCheck_thread.Start();
    }
    void ServerCheck()
    {
        while (true)
        {
            try
            {
                bytes = socket.Receive(ref ep2);// 서버에서 온 패킷을 버퍼에 담기 
                string t = Encoding.UTF8.GetString(bytes); // 큐에 버퍼를 넣을 준비 
                t = t.Replace("\0", string.Empty); //버퍼 마지막에 공백이 있는지 검색하고 공백을 삭제
                lock (Buffer_lock) //queue 충돌방지 
                {
                    netBuffer.Enqueue(t); // 큐에 버퍼를 저장한다. 
                    
                }
                // 버퍼를 사용후 초기화 시킨다 // 이전패킷보다 다음패킷이 적을경우 패킷값이 이상하게 나온다.
                System.Array.Clear(bytes, 0, bytes.Length);
            }
            catch { } // try - catch로 정해놓은 버퍼의 크기 이상의 패킷량이 왔을경우 대비 
        }
    }
    void BufferSystem()
    {
        //큐의 크기가 0이 아니면 작동, 만약 WHILE 을 안하면 프레임마다 버퍼를 처리하는데 많은 패킷을 처리할때는 
        // 처리되는 양보다 쌓이는 양이 많아져 작동이 제대로 이루어지지 않음 
        while(netBuffer.Count != 0)
        {
            string b = null;
            lock (Buffer_lock)
            {
                b = netBuffer.Dequeue(); // 큐에 담겨있는 버퍼를 스트링에 넣고 사용하기
               CharacterController(b);
            }
           // Debug.Log("server ->" + b);// 버퍼를 사용한다. 
        }
    }
    
    //코루틴 waitforsecond 사용시 가비지 컬렉터 사용을 줄이기 위함
    internal static class YieldInstructionChache
    {
        class FloatComparer : IEqualityComparer<float>
        {
            bool IEqualityComparer<float>.Equals(float x, float y)
            {
                return x == y;
            }
            int IEqualityComparer<float>.GetHashCode(float obj)
            {
                return obj.GetHashCode();
            }
        }
        public static readonly WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();
        public static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();

        private static readonly Dictionary<float, WaitForSeconds> _timeInterval = new Dictionary<float, WaitForSeconds>(new FloatComparer());

        public static WaitForSeconds WaitForSeconds(float seconds)
        {
            WaitForSeconds wfs;
            if (!_timeInterval.TryGetValue(seconds, out wfs))
                _timeInterval.Add(seconds, wfs = new WaitForSeconds(seconds));
            return wfs;
        }
    }

    // 상대방 움직임 체크하는 코루틴 
    private IEnumerator TransformUpdate()
    {
        //Debug.Log("한번만 실행");
        while (true)
        {
            //yield return null;
            //yield return new WaitForSeconds(0.02f);
            yield return YieldInstructionChache.WaitForSeconds(0.02f);
            // 아이템을 집었는지 안집었는지 판단 . 
            //currentItem = playerHand.GetComponent<PlayerHand>().currentItem;
            //grabItem = playerHand.GetComponent<PlayerHand>().grabItem;
            //----------------------------------------------------------------
            // 캐릭터가 땅에 있는 상태인지 공중에 뜬 상태인지 계속 업데이트 해준다.
            Animator.SetBool("Grounded", Grounded);
            // 중점과 반지름을 주면  가상의 원을 만들어 안에있거나 접촉하는 collider들을 반환하는 함수 
            Grounded = Physics.OverlapSphere(transform.position, 0.138f, 1).Length > 1;


            // json 데이터의 포장지 느낌   
            wrapper.roomData = roomData;
            string room = JsonUtility.ToJson(wrapper, true);
            System.IO.File.WriteAllText(roomPath, room); // json 파일에 작성 
            var data = Encoding.UTF8.GetBytes(room);
            // Debug.Log("b" + data.ToString());
            socket.Send(data, data.Length);
        }
    }

    private void CharacterController(string d) {
        try
        {
            JObject obj = JObject.Parse(d);
           // Debug.Log(obj);
            string name = obj["name"].ToString();
            if (name.Equals(names))
            {
                string x = obj["x"].ToString();
                string y = obj["y"].ToString();
                string z = obj["z"].ToString();
                string ry = obj["ry"].ToString();
                string rx = obj["runX"].ToString();
                string rz = obj["runZ"].ToString();
                string p = obj["punch"].ToString();
                string al = obj["alive"].ToString();
                string mv = obj["move"].ToString();
                string wn = obj["winner"].ToString();
                string tm = obj["throwMode"].ToString();
                string ti = obj["throwItem"].ToString();
                string pu = obj["pickup"].ToString();
                string dr = obj["drop"].ToString();
                bool drop = Convert.ToBoolean(dr);
                bool pickup = Convert.ToBoolean(pu);
                bool throwItem = Convert.ToBoolean(ti);
                bool throwMode = Convert.ToBoolean(tm);
                bool punch = Convert.ToBoolean(p);
                bool alive = Convert.ToBoolean(al);
                bool move = Convert.ToBoolean(mv);
                bool winner = Convert.ToBoolean(wn);

                if (winner)
                {
                 //   Animator.SetBool("Winner", true);
                  //  StopAllCoroutines();
                  //  ServerCheck_thread.Abort();
                    Debug.Log(names + " Win");
                }

                if (move)
                {
                    Rigidbody.mass = 1f;
                }
                else
                    Rigidbody.mass = 1000f;
                //Debug.Log(Alive);
                if (!alive)
                {
                    if (Alive)
                    {
                        if (health > 0)
                        {
                            //Debug.Log("상대 뒤졌음");
                            Animator.SetBool("Died", true);
                            CapsuleCollider.enabled = false;
                            hand.enabled = false;
                            Rigidbody.mass = 1000f;
                        }
                        Alive = false;
                        //Debug.Log("코루틴종료 실행");
                        StartCoroutine(DeathTimer());
                    }
                }
                if (health < 0)
                {
                    //Debug.Log("상대 뒤졌음");
                    Animator.SetBool("Died", true);
                    CapsuleCollider.enabled = false;
                    hand.enabled = false;
                    Rigidbody.mass = 1000f;
                }
                else
                {
                    if (!x.Equals(""))
                    {
                        // Debug.Log("fy" + fy);
                        // Debug.Log("fz" + fz);
                        //     Debug.Log("rry" + rry);
                        Animator.SetFloat("RunX", float.Parse(rx));  // 캐릭터의 진행방향 양옆 
                        Animator.SetFloat("RunZ", float.Parse(rz));  // 캐릭터의 진행방향 앞뒤

                        //아이템줍기 
                        if (pickup && !Animator.GetBool("PickUp"))
                        {
                            Debug.Log("줍줍");
                            playerHand.SetActive(true);
                            playerHand.GetComponent<OtherPlayerHand>().handCollider.SetActive(true);
                            Animator.SetBool("PickUp", true);
                            // hatchet.SetActive(true);
                            //grabItem = true;
                            //currentItem = "Hatchet";
                            StartCoroutine(DropTimerFalse());
                        }
                        if (!pickup)
                        {
                            Animator.SetBool("PickUp", false);
                        }


                        if (grabItem)
                        {
                            //  Debug.Log("grabItem"+grabItem);
                            //Debug.Log("grabItem" + grabItem);
                            //Debug.Log("currentItem" + currentItem);
                            //던지기준비
                            if (throwMode && !Animator.GetBool("Throw"))
                            {
                                Debug.Log("던지기모드");
                                hatchet.SetActive(true);
                                Animator.SetBool("Throw", true);
                            }
                            //던짐 
                            if (throwItem && Animator.GetBool("Throw"))
                            {
                                Debug.Log("던짐");
                                Animator.SetBool("Throw", false);
                                //hatchet.SetActive(false);
                                grabItem = false;
                                currentItem = "";
                                playerHand.GetComponent<OtherPlayerHand>().ThrowItem(names);
                                //StartCoroutine(DropTimer());
                            }
                        }
                       

                        //버리기
                        // 아이템 버리기 구현 
                        if (drop && !currentItem.Equals(""))
                        {
                            Debug.Log("버림");
                           // playerHand.SetActive(false);
                            playerHand.GetComponent<OtherPlayerHand>().DropItem();
                          //hatchet.SetActive(false);
                            grabItem = false;
                            currentItem = "";
                            //StartCoroutine(DropTimer());
                        }


                        if (punch)
                        {
                            hatchet.SetActive(false);
                            hand.isTrigger = true;
                            Animator.SetBool("Attack", true);
                            StartCoroutine(PunchTimer()); // 1초뒤 펀치 트리거 꺼지도록 
                        }
                        else
                        {
                            Animator.SetBool("Attack", false);
                        }
                        vec.x = float.Parse(x);
                        vec.y = float.Parse(y);
                        vec.z = float.Parse(z);
                        _transform.position = vec;
                        _transform.rotation = Quaternion.Euler(0, float.Parse(ry), 0);
                    }
                }
              
                
            }
        }
        catch (Exception e)
        {
            Debug.Log("Socket error : " + e.Message);
        }
    }

    private IEnumerator DropTimer()
    {
        yield return new WaitForSeconds(1f);// 1초뒤
        playerHand.SetActive(true);
    }

    private IEnumerator DropTimerFalse()
    {
        yield return new WaitForSeconds(1f);// 1초뒤
        playerHand.SetActive(false);
    }

    private IEnumerator PunchTimer()
    {
        yield return new WaitForSeconds(1f);// 1초뒤
        hand.isTrigger = false;
    }


    private IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(3f);

        //개인전일때 
        if (!group)
        {
            //Debug.Log("You're Winner!!");
            StopAllCoroutines();
            ServerCheck_thread.Abort();
            Debug.Log(names + " Died");

            _networkManager.gameoverPop.SetActive(true);
            _networkManager.joystick.SetActive(false);
            _networkManager.gameoverPop.GetComponentInChildren<Text>().text = "You Win!";
            _networkManager.gameoverTimer.text = timer.ToString();
            _player.roomData.winner = true;
            _player.roomData.move = true;
            //_networkManager.roomData.receivePacket = false;
            _player.RoomSendData();
            StartCoroutine(SendUserWin());
            StartCoroutine(Timer());
            StartCoroutine(BackToLobby());
        }
        if (group)
        {
            StopAllCoroutines();
            ServerCheck_thread.Abort();
            Debug.Log(names + " Died");
            if(position.Equals("A"))
                _networkManager.statusA.text = "DIED";
            if (position.Equals("B"))
                _networkManager.statusB.text = "DIED";
            if (position.Equals("C"))
                _networkManager.statusC.text = "DIED";
            if (position.Equals("D"))
                _networkManager.statusD.text = "DIED";

            _networkManager.dieCount++;
        }

    }

    private IEnumerator BackToLobby()
    {
        yield return new WaitForSeconds(5f);// 5초뒤
        _networkManager.BackToLobby();
        //joystick.SetActive(true);
    }

    private IEnumerator Timer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);// 1초뒤
            timer--;
            _networkManager.gameoverTimer.text = timer.ToString();
        }
    }



    // 유저의 패배카운트 업데이트하기 
    IEnumerator SendUserWin()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", _networkManager.username);
        using (UnityWebRequest www = UnityWebRequest.Post("http://15.164.101.199/sendwin.php", form))
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

    private void OnApplicationQuit()
    {
        ServerCheck_thread.Abort();// 스레드 종료 // 종료시키지 않으면 유니티 멈춤
        socket.Close();
    }

}




