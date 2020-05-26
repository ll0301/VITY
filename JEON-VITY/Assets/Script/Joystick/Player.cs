using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("**Hand Controll**")]
    public GameObject playerHand;
    public string currentItem;
    public bool grabItem;
    public bool throwMode;
    public bool throwItem;
    public GameObject hatchet2;

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
    public GameObject character;
    public Transform _transform;

   // GameObject NetworkManager;
    NetworkManager _networkManager;
    Controller _controller;
    public float tmp;
    public bool Death;
    public bool DeathConfirm;
    public bool group;
    public string position;
    public int moveCount;
    public bool winnerConfirm;
    string roomFilename = "room.json"; // 플레이어 상태 저장 
    string roomPath; // json 파일 저장경로 
    public RoomData roomData = new RoomData();
    public RoomWrapper wrapper = new RoomWrapper();

    UdpClient socket;
    IPEndPoint ep;
    IPEndPoint ep2;
  
    public string host;   // 호스트 주소 
    public int port;  // 포트번호 
    //[HideInInspector] // 인스펙터창에서감춘다.  
    public InputStr Input;   
    public struct InputStr
    {
        public float LookX;   // 캐릭터 보는 방향 위아래 
        public float LookZ; // 캐릭터 보는방향 양옆
        public float RunX; // 캐릭터 이동방향 앞뒤 
        public float RunZ; // 캐릭터이동방향 양옆 
        public bool Jump;
        public bool Attack;
        public bool Run;
        public bool Pickup;
        public bool Drop;
        public bool Aim;
    }
    public string names;
    public int timer;
    public const float Speed =3f; // 캐릭터 이동스피드 
    public const float JumpForce = 4.5f; // 캐릭터 점프 높이 
    public int health = 10;
    public GameObject handObj;
    public BoxCollider hand;
    public CapsuleCollider CapsuleCollider;

    protected Rigidbody Rigidbody;   // 게임오브젝트가 물리적제어로 동작하게한다.   
    protected Quaternion LookRotation; // 사원수 EULER 좌표계의 GIMBALLOCK 현상을 해결하기위해 고안된 좌표. 
    protected Animator Animator; // 
    protected bool Grounded;  // 캐릭터가 바닥에 있을때
    private void Awake()
    {
        timer = 5;
        roomPath = Application.persistentDataPath + "/" + roomFilename;
        _networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        roomData = _networkManager.roomData; // networkManager 스크립트 캐싱 
        character = GameObject.Find("Character(Clone)");
        _networkManager.clientCharacter = character;
        names = _networkManager.username;
        _transform = character.transform;
        _controller = character.GetComponent<Controller>();
        tmp = _controller.InputRotationX;
        // 컴포넌트에 접근한다. 
        Rigidbody = GetComponent<Rigidbody>(); 
        Animator = GetComponentInChildren<Animator>();
        hand = handObj.GetComponentInChildren<BoxCollider>();
        CapsuleCollider = character.GetComponentInChildren<CapsuleCollider>();
        Death = false;
        DeathConfirm = false;
        winnerConfirm = false;
        throwMode = false;
        throwItem = false;

        currentItem = "";
        grabItem = false;

    }

    private void Start()
    {
        socket = new UdpClient();
        ep = new IPEndPoint(IPAddress.Parse(host), port);
        socket.Connect(ep);
        ep2 = new IPEndPoint(IPAddress.Any, 0);
        _networkManager.mCharacter.Add(character);


        if (roomData.group.Equals("2"))
            group = false;
        if (roomData.group.Equals("4"))
            group = true;
    }

    // 고정적인 시간으로 반복실행하는 함수 
    // update와 달리 프레임 기반이 아니라 동일한 시간으로 동작해서 물리계산이 시작된다. 
    void FixedUpdate()
    {


        if (DeathConfirm)
        {
            if (_networkManager.dieCount == 3)
            {
                _networkManager.dieCount = 0;
                Debug.Log("You're Loser!!");
                _networkManager.gameoverPop.SetActive(true);
                _networkManager.joystick.SetActive(false);
                _networkManager.gameoverPop.GetComponentInChildren<Text>().text = "You Lose!";
                _networkManager.gameoverTimer.text = timer.ToString();
                _networkManager.groupNaviOpen.SetActive(false);
                StartCoroutine(SendUserLose());
                StartCoroutine(Timer());
                StartCoroutine(BackToLobby());
            }
            return;
        }

        if(_networkManager.dieCount == 3)
        {
            roomData.winner = true;
        }

        if (roomData.winner)
        {
            if (!winnerConfirm)
            {
                _networkManager.RoomSendData(); // 전송
                winnerConfirm = true;
                StopAllCoroutines();
                //Debug.Log("You Died");
             //   Animator.SetBool("Winner", true);

                if (group)
                {
                    _networkManager.gameoverPop.SetActive(true);
                    _networkManager.joystick.SetActive(false);
                    _networkManager.gameoverPop.GetComponentInChildren<Text>().text = "You Win!";
                    _networkManager.gameoverTimer.text = timer.ToString();
                    roomData.winner = true;
                    roomData.move = true;
                    //_networkManager.roomData.receivePacket = false;
                    RoomSendData();
                    StartCoroutine(SendUserWin());
                    StartCoroutine(Timer());
                    StartCoroutine(BackToLobby());
                }

            }
            return;
        }

            // 에너지가 없을때 
            if (health < 0)
            {
                Animator.SetBool("Died", true);
                CapsuleCollider.enabled = false;
                hand.enabled = false;
                Rigidbody.mass = 1000f;
                if (!Death)
                {
                    // Debug.Log("너 뒤졌음");
                    Death = true;
                    roomData.alive = false;
                    _networkManager.RoomSendData(); // 전송
                    StartCoroutine(DeathTimer());
                }
            }


        //플레이어가 공격을 했다면
        if (Animator.GetBool("Attack"))
        {
            // 공격상황을 fasle로 만들고
            Animator.SetBool("Attack", false);
            // 메인 레이어로 돌아간다. \
        }

        // 공격버튼이 눌리면 
        if (Input.Attack && !Animator.GetBool("Attack"))
        {
            hand.isTrigger = true;
            // Debug.Log("공격");
            Animator.SetBool("Attack", true);
            StartCoroutine(PunchTimer()); // 펀치 트리거가 1초뒤 꺼지게끔 
        }
        

        // 아이템 버리기 구현 
        if (Input.Drop && !currentItem.Equals(""))
        {
            playerHand.GetComponent<PlayerHand>().DropItem(currentItem);
            roomData.drop = true;
            RoomSendData();
            StartCoroutine(DropTimer());
        }


        if (Animator.GetBool("PickUp"))
        {
            Animator.SetBool("PickUp", false);
        }

        //플레이어가 pickup 버튼을 누르면 
        if (Input.Pickup && !Animator.GetBool("PickUp"))
        {
            playerHand.SetActive(true);
            //playerHand.GetComponent<PlayerHand>().handCollider.SetActive(true);
            Animator.SetBool("PickUp", true);
        }


        
        Animator.SetBool("Grounded", Grounded);  // 캐릭터가 땅에 있는 상태인지 공중에 뜬 상태인지 계속 업데이트 해준다. 

        ////////////// 문제시 복구
        ////달릴때
        if (Input.Run)
        {
            var localVelocity = Quaternion.Inverse(transform.rotation) * (Rigidbody.velocity / Speed);
            Animator.SetFloat("RunX", localVelocity.x);  // 캐릭터의 진행방향 앞인지 뒤인지 체크 
            Animator.SetFloat("RunZ", localVelocity.z);  // 캐릭터의 진행방향 왼쪽인지 오른쪽인지
            var inputRun = Vector3.ClampMagnitude(new Vector3(Input.RunX, 0, Input.RunZ), 1);
            Rigidbody.velocity = new Vector3(inputRun.x * Speed, Rigidbody.velocity.y, inputRun.z * Speed);
            StartCoroutine(sendTransform(inputRun.magnitude, localVelocity.x, localVelocity.z));
        }
        else if (!Input.Run) // 걸을때 
        {
            var localVelocity = Quaternion.Inverse(transform.rotation) * (Rigidbody.velocity / 2f);
            Animator.SetFloat("RunX", localVelocity.x);
            Animator.SetFloat("RunZ", localVelocity.z);
            var inputRun = Vector3.ClampMagnitude(new Vector3(Input.RunX, 0, Input.RunZ), 0.6f);
            Rigidbody.velocity = new Vector3(inputRun.x * 2f, Rigidbody.velocity.y, inputRun.z * 2f);
            //Debug.Log(inputRun.magnitude);
            StartCoroutine(sendTransform(inputRun.magnitude, localVelocity.x, localVelocity.z));
        }
        
        ////////////// 문제시 복구

        //position
        // maxLength로 고정된 magnitude와 함께 vector 사본을 반환한다. 
        // var inputRun = Vector3.ClampMagnitude(new Vector3(Input.RunX, 0, Input.RunZ), 1);
        var inputLook = Vector3.ClampMagnitude(new Vector3(Input.LookX, 0, Input.LookZ), 1);
        //rotation to go target 
        if (inputLook.magnitude > 0.01f)
            // input rook의 크기 
            // Quaternion.AngleAxis (각도,축) =  축 기준으로 각도만큼 회전 
            LookRotation = Quaternion.AngleAxis(Vector3.SignedAngle(Vector3.forward, inputLook, Vector3.up), Vector3.up);

        // 중점과 반지름을 주면  가상의 원을 만들어 안에있거나 접촉하는 collider들을 반환하는 함수 
        Grounded = Physics.OverlapSphere(transform.position, 0.138f, 1).Length > 1;
        //Debug.Log(Physics.OverlapSphere(transform.position, 0.138f, 1).Length);

        // 아이템을 잡고 있을 때 
        if (grabItem)
        {
       //     Debug.Log("throwMode"+throwMode);
           // Debug.Log("throwItem" + throwItem);
            //인풋버튼이 눌리고 thorw가 false일때 ... 
            // 던지기자세를 취한다. 
            if (Input.Aim && !Animator.GetBool("Throw") && !throwMode)
            {
                Animator.SetBool("Throw", true);
                //   Debug.Log("던지기 준비");
                throwMode = true;
                roomData.throwMode = true;
                roomData.throwItem = false;
                //Debug.Log("1");
            }
            // 버튼에서 손을떼면 무기가 발사된다. 
            else if (!Input.Aim && Animator.GetBool("Throw") && throwMode)
            {
                Animator.SetBool("Throw", false);
                throwMode = false;
                throwItem = true;
                roomData.throwMode = false;
                roomData.throwItem = true;
                playerHand.GetComponent<PlayerHand>().ThrowItem(names);
                //Debug.Log("3");
            }
        }


        //JUMP 
        if (Input.Jump)
        {
            if (Grounded) // 캐릭터가 땅에 있으면 
            {
                // 점프를 하게되는 로직 
                Rigidbody.velocity = new Vector3(Rigidbody.velocity.x, JumpForce, Rigidbody.velocity.z);
            }
        }

        transform.rotation = LookRotation;
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

    private IEnumerator PunchTimer()
    {
        yield return new WaitForSeconds(1f);// 고정프레임 업데이트 함수가 호출될때까지 기다린다. 
        hand.isTrigger = false;
    }

    private IEnumerator DropTimer()
    {
        yield return new WaitForSeconds(1f);// 고정프레임 업데이트 함수가 호출될때까지 기다린다. 
        roomData.drop = false;
        RoomSendData();
    }


    // 내캐릭터의 트랜스폼값 서버로 보내기 
    private IEnumerator sendTransform(float inputRun, float lovelX, float lovelZ)
    {
        yield return new WaitForFixedUpdate();// 고정프레임 업데이트 함수가 호출될때까지 기다린다. 

        ////  Debug.Log("inputRun"+ inputRun.magnitude);
        //// 유저의 포지션값과 || 로테이션값이 변경될때에만 서버에 보낸다. 
        //if (Math.Truncate(inputRun * 10) * 0.2 > 0.0f
        //  || tmp != _controller.InputRotationX
        //  || !Grounded
        //  || Input.Attack
        //  || Input.Drop
        //  || Input.Pickup
        //  )
        //{
           // Debug.Log("너 움직이고있음");
            tmp = _controller.InputRotationX;
            //var nm = NetworkManager.GetComponent<NetworkManager>();
            roomData.pX = _transform.position.x.ToString();
            roomData.pY = _transform.position.y.ToString();
            roomData.pZ = _transform.position.z.ToString(); // 내캐릭터의 포지션 x,y,z 값을 전송한다. 
            roomData.rY = _controller.InputRotationX.ToString(); // 내캐릭터의 바라보는 방향을 전송한다.
            roomData.runX = lovelX.ToString(); // 달리는 방향(앞,뒤)을 전송한다.
            roomData.runZ = lovelZ.ToString(); // 달리는 방향(양옆)을 전송한다. 
            roomData.move = true;        // 현재 움직이고 있는 상황 
            //roomData.receivePacket = false; // 패킷을 리시브하지 않는 상황 
            moveCount = 0;

            if (Input.Attack) // 공격버튼이 눌리면 
                //  Debug.Log(Input.Attack);
                roomData.punch = true; // 펀치했음을 전송 
            else
                roomData.punch = false; // 펀치 하지 않았음을 전송 

        if (Input.Pickup)
            roomData.pickup = true;
        else
            roomData.pickup = false;



        if (throwItem)
        {
            throwItem = false;
            StartCoroutine(ThrowTimer());
        }
        
        
        //_networkManager.move = true;
        RoomSendData(); // 전송

       // }
        //else
        //{

        //    // 펀치를 날렸으면 
        //    if (roomData.punch)
        //    {
        //        roomData.move = true;
        //      //  roomData.receivePacket = false;
        //        // 계속 펀치를 날리지 않도록 
        //        roomData.punch = false;
        //        //   _networkManager.move = true;
        //        RoomSendData();
        //    }
        //    else
        //    {
        //        moveCount++;
        //        //_networkManager.move = false;
        //        roomData.move = false;
        //        //  roomData.receivePacket = true;
        //        //_networkManager.RoomSaveData();

        //        roomData.throwItem = false;
        //        roomData.throwMode = false;
        //        roomData.pickup = false;
        //        roomData.drop = false;

        //        //문제시 삭제
        //        roomData.pX = _transform.position.x.ToString();
        //        roomData.pY = _transform.position.y.ToString();
        //        roomData.pZ = _transform.position.z.ToString(); // 내캐릭터의 포지션 x,y,z 값을 전송한다. 
        //        roomData.rY = _controller.InputRotationX.ToString(); // 내캐릭터의 바라보는 방향을 전송한다.
        //        roomData.runX = lovelX.ToString(); // 달리는 방향(앞,뒤)을 전송한다.
        //        roomData.runZ = lovelZ.ToString(); // 달리는 방향(양옆)을 전송한다. 
        //        RoomSendData();
        //        // 문제시 삭제 

        //        if (moveCount == 1)
        //        {
        //            //Debug.Log("count : " + moveCount);
        //            //RoomSendData();  문제시 복구 
        //        }
        //    }
        //}
    }

    private IEnumerator ThrowTimer()
    {
        yield return new WaitForSeconds(1f);
        roomData.throwItem = false;
        RoomSendData();
    }


    // 죽었을때 타이머 
    private IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(3f);
        if (!group)
        {
            StopAllCoroutines();
            Debug.Log("You Died");
            DeathConfirm = true;

            Debug.Log("You're Loser!!");
            _networkManager.gameoverPop.SetActive(true);
            _networkManager.joystick.SetActive(false);
            _networkManager.gameoverPop.GetComponentInChildren<Text>().text = "You Lose!";
            _networkManager.gameoverTimer.text = timer.ToString();
            StartCoroutine(SendUserLose());
            StartCoroutine(Timer());
            StartCoroutine(BackToLobby());

        }
        if (group)
        {
            StopAllCoroutines();
            Debug.Log("You Died");
            DeathConfirm = true;

            if (position.Equals("A"))
                _networkManager.statusA.text = "DIED";
            if (position.Equals("B"))
                _networkManager.statusB.text = "DIED";
            if (position.Equals("C"))
                _networkManager.statusC.text = "DIED";
            if (position.Equals("D"))
                _networkManager.statusD.text = "DIED";

            _networkManager.groupNaviClose.SetActive(false);
            _networkManager.groupNaviOpen.SetActive(true);
            _networkManager.btnClose.SetActive(false);
            _networkManager.btnQuit.SetActive(true);
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
    IEnumerator SendUserLose()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", _networkManager.username);
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
    public void RoomSendData()
    {
        // json 데이터의 포장지 느낌   
        wrapper.roomData = roomData;
        string room = JsonUtility.ToJson(wrapper, true);
        System.IO.File.WriteAllText(roomPath, room); // json 파일에 작성 
        var data = Encoding.UTF8.GetBytes(room);
        // Debug.Log("b" + data.ToString());
        socket.Send(data, data.Length);
    }
    private void OnDisable()
    {
        socket.Close();
    }
}
