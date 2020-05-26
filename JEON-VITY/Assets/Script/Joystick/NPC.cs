using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class NPC : MonoBehaviour
{

    public GameObject hatchet2;
    public GameObject npcObj;
    //public Transform _transform;
    public GameObject npcPoint;
    public Transform _transform;
    //public GameObject nameText;

    public const float Speed = 3f; // 캐릭터 이동스피드 
    public const float JumpForce = 4.5f; // 캐릭터 점프 높이 

    protected Rigidbody Rigidbody;   // 게임오브젝트가 물리적제어로 동작하게한다.   
    protected Quaternion LookRotation; // 사원수 EULER 좌표계의 GIMBALLOCK 현상을 해결하기위해 고안된 좌표. 
    protected Animator Animator; // 
    protected bool Grounded;  // 캐릭터가 바닥에 있을때
    public string ownerName;
    public string names;
    public string position;
    public int health = 10;
    public bool Death;
    public CapsuleCollider CapsuleCollider;

    public List<Transform> waypoints;
    private Transform targetWaypoint;
    private int targetWaypointIndex = 0;
    private float minDistance = 0.1f;
    private int lastWaypintIndex;

    private float movementSpeed = 1.5f;
    private float rotationSpeed = 8f;

    public bool Idle;
    public bool IdleConfirm;
    public float idleTime;

    public bool isOwner;


    public NPCData NPCData = new NPCData();
    public NPCWrapper wrapper = new NPCWrapper();

    UdpClient sendSocket;
    IPEndPoint ep;
    IPEndPoint ep2;


    UdpClient receiveSocket;
    IPEndPoint recEp;
    IPEndPoint recEp2;

    public string sendhost;   // 호스트 주소 
    public int sendport;  // 포트번호 
    public int receiveport;  // 포트번호 
    Thread ServerCheck_thread; // 서버에서 보내는 패킷을 체크하기 위한 스레드 
    Queue<string> netBuffer = new Queue<string>();// 버퍼를 저장하기 위한 큐 
    object Buffer_lock = new object(); // queue 처리 충돌 방지용 lock
    public byte[] bytes = new byte[65535];
    Vector3 vecP;
    private void Awake()
    {
        npcObj = GameObject.Find(names+"(Clone)");
       // _transform = npcObj.transform;
        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponentInChildren<Animator>();
        CapsuleCollider = npcObj.GetComponentInChildren<CapsuleCollider>();
        _transform = npcObj.transform;
        vecP = transform.position;
        npcPoint = GameObject.Find("Network/"+names);
        if (npcPoint != null)
        {
            waypoints = npcPoint.GetComponent<WaypointController>().waypoints;
            Death = false;
            Idle = false;
            IdleConfirm = false;
            lastWaypintIndex = waypoints.Count - 1;
            targetWaypoint = waypoints[targetWaypointIndex];
        }
    }

    private void Start()
    {
        if (isOwner)
        {
            sendSocket = new UdpClient();
            ep = new IPEndPoint(IPAddress.Parse(sendhost), sendport);
            sendSocket.Connect(ep);
            ep2 = new IPEndPoint(IPAddress.Any, 0);
            Animator.SetBool("Grounded", true);
            NPCData.isOwner = isOwner;
            NPCData.death = false;
            NPCData.idle = false;
            NPCData.firstData = true;
            NPCData.owner = ownerName;
            NPCData.name = names;
            NPCData.pX = _transform.position.x.ToString();
            NPCData.pY = _transform.position.y.ToString();
            NPCData.pZ = _transform.position.z.ToString();
            NPCData.rY = _transform.eulerAngles.y.ToString();
            NPCSendData();
        }
        else
        {
            Animator.SetBool("Grounded", true);
            ServerOn();
            NPCData.owner = ownerName;
            NPCData.name = names;
            StartCoroutine(Send2Transform());
            StartCoroutine(Buffer_update());

        }
    }


    private void FixedUpdate()
    {
        if (!isOwner)
            return;
        if (npcPoint == null)
        {
            //Rigidbody.mass = 1f;
            return;
        }


        if (health < 0)
        {
            if (!Death)
            {
                CapsuleCollider.enabled = false;
                Animator.SetBool("Grounded", true);
                Animator.SetBool("Died", true);
                Death = true;
                NPCData.death = true;
                NPCSendData();
            }
            return;
        }
        
        //----------------------------------------------------------------
        // 캐릭터가 땅에 있는 상태인지 공중에 뜬 상태인지 계속 업데이트 해준다.
       // Animator.SetBool("Grounded", Grounded);
        // 중점과 반지름을 주면  가상의 원을 만들어 안에있거나 접촉하는 collider들을 반환하는 함수 
        //Grounded = Physics.OverlapSphere(transform.position, 0.138f, 1).Length > 1;
        
        if (Idle)
        {
            if (!IdleConfirm)
            {
                Animator.SetFloat("RunZ", 0f);
                IdleConfirm = true;
                StartCoroutine(IdleTimer());
                NPCData.idle = true;
            }
            return;
        }

            float movementStep = movementSpeed * Time.deltaTime;
            float rotationStep = rotationSpeed * Time.deltaTime;

            Animator.SetFloat("RunZ", 0.5f);

            Vector3 directionToTarget = targetWaypoint.position - transform.position;
            Quaternion rotationToTarget = Quaternion.LookRotation(directionToTarget);

            transform.rotation = Quaternion.Slerp(transform.rotation, rotationToTarget, rotationStep);

            float distance = Vector3.Distance(transform.position, targetWaypoint.position);
            //Debug.Log("Distance:" + distance);
            CheckDistanceToWaypoint(distance);
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, movementStep);

            if (!NPCData.death)
            StartCoroutine(SendTransform());
        
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


    // 패킷 반복적으로 받기 
    private IEnumerator Buffer_update()
    {
        //  Debug.Log("한번만 실행");
        while (true)
        {
            //yield return new WaitForSeconds(0.02f);
            //yield return YieldInstructionChache.WaitForSeconds(0f);
            yield return null;
            BufferSystem();
        }
    }
    void ServerOn()
    {
        receiveSocket = new UdpClient();
        recEp = new IPEndPoint(IPAddress.Parse(sendhost), receiveport);
        receiveSocket.Connect(recEp);
        recEp2 = new IPEndPoint(IPAddress.Any, 0);
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
                bytes = receiveSocket.Receive(ref recEp2);// 서버에서 온 패킷을 버퍼에 담기 
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
        while (netBuffer.Count != 0)
        {
            string b = null;
            lock (Buffer_lock)
            {
                b = netBuffer.Dequeue(); // 큐에 담겨있는 버퍼를 스트링에 넣고 사용하기
                ReceiveNPCTransform(b);
            }
            // Debug.Log("server ->" + b);// 버퍼를 사용한다. 
        }
    }


    // 방장일때 npc 위치포인트 보내기  
    private IEnumerator SendTransform()
    {
        yield return new WaitForFixedUpdate();
        NPCData.idle = Idle;
        NPCData.firstData = false;
        NPCData.pX = _transform.position.x.ToString();
        NPCData.pY = _transform.position.y.ToString();
        NPCData.pZ = _transform.position.z.ToString();
        NPCData.rY = _transform.eulerAngles.y.ToString();
        NPCSendData();
    }

    // 방장이 아닐때 npc 위치포인트 받기 
    private IEnumerator Send2Transform()
    {
        while (true)
        {
            yield return null;
            //yield return YieldInstructionChache.WaitForSeconds(0f);
            //----------------------------------------------------------------
            // 캐릭터가 땅에 있는 상태인지 공중에 뜬 상태인지 계속 업데이트 해준다.
            // Animator.SetBool("Grounded", Grounded);
            // 중점과 반지름을 주면  가상의 원을 만들어 안에있거나 접촉하는 collider들을 반환하는 함수 
            //  Grounded = Physics.OverlapSphere(transform.position, 0.138f, 1).Length > 1;

            // json 데이터의 포장지 느낌   
            wrapper.npcData = NPCData;
            string npc = JsonUtility.ToJson(wrapper, true);
            var data = Encoding.UTF8.GetBytes(npc);
            // Debug.Log("b" + data.ToString());
            receiveSocket.Send(data, data.Length);
        }
    }

    // 방장이  아닐때 npc transform 값 받기 
    private void ReceiveNPCTransform(string d)
    {
        try
        {
            JObject obj = JObject.Parse(d);
            string name = obj["name"].ToString();
            if (name.Equals(names))
            {
                string x = obj["x"].ToString();
                string y = obj["y"].ToString();
                string z = obj["z"].ToString();
                string ry = obj["ry"].ToString();
                string dea = obj["death"].ToString();
                string idl = obj["idle"].ToString();
                bool death = Convert.ToBoolean(dea);
                bool idle = Convert.ToBoolean(idl);


                if (death)
                {
                    CapsuleCollider.enabled = false;
                    Animator.SetBool("Grounded", true);
                    Animator.SetBool("Died", true);
                    StopAllCoroutines();
                }

                if (idle)
                {
                    Animator.SetFloat("RunZ", 0f);
                }
                else
                {
                    Animator.SetFloat("RunZ", 0.5f);
                }
                vecP.x = float.Parse(x);
                vecP.y = float.Parse(y);
                vecP.z = float.Parse(z);
                _transform.position = vecP;
                _transform.rotation = Quaternion.Euler(0, float.Parse(ry), 0);
            }
        }
        catch (Exception e)
        {
            Debug.Log("Socket error : " + e.Message);
        }
    }

    public void NPCSendData()
    {
        // json 데이터의 포장지 느낌   
        wrapper.npcData = NPCData;
        string npc = JsonUtility.ToJson(wrapper, true);
        var data = Encoding.UTF8.GetBytes(npc);
        // Debug.Log("b" + data.ToString());
           sendSocket.Send(data, data.Length);
    }

    public void CheckDistanceToWaypoint(float currentDistance)
    {
        if(currentDistance <= minDistance)
        {
            targetWaypointIndex++;
            UpdateTargetWaypoint();
        }
    }

    public void UpdateTargetWaypoint()
    {
        if(targetWaypointIndex > lastWaypintIndex)
        {
            targetWaypointIndex = 0;
        }
        targetWaypoint = waypoints[targetWaypointIndex];
    }


    private IEnumerator IdleTimer()
    {
       // float random = Random.Range(0f, 10f);
        yield return new WaitForSeconds(2.5f);// 랜덤 
        Animator.SetFloat("RunZ", 0.25f);
        Idle = false;
        IdleConfirm = false;
    }


    private void OnDisable()
    {
        //ServerCheck_thread.Abort();// 스레드 종료 // 종료시키지 않으면 유니티 멈춤
        //sendSocket.Close();
        //receiveSocket.Close();
    }

}
