using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
   
    //input 
    protected FixedJoystick Joystick;   // 왼쪽 캐릭터 이동 조이스틱 
    protected FixedTouchField TouchField;   // 화면을 터치하여 캐릭터의 시선변경 
    protected FixedButtonA ButtonA; // 공격버튼 
    protected FixedButtonB ButtonB; // 점프버튼 
    protected FixedButtonR ButtonR; // 달리기, 걷기 버튼 
    protected FixedButtonX ButtonX; // 아이템 줍기 버튼
    protected FixedButtonDrop ButtonDrop; // 아이템 버리기 버튼
    protected FixedButtonAim ButtonAim; // 던지기버튼
    protected AimPoint AimPoint; // 조준점 
    protected ButtonWalk ButtonWalk; // 
    protected Player Player; // 플레이어 스크립트 

    public GameObject buttonWalk;
    
    //camera controll
    // public이기 때문에 인스펙터 창에서 수정할 수있다.  
    public Vector3 CameraPivot;  //카메라 회전 축??
    public float CameraDistance; // 카메라 거리   
    public bool isLocalPlayer = true; // network 
    protected const float RotationSpeed = 5;  // 회전속도 

    public float InputRotationX; // 0-360   
    public float InputRotationY; // 90 - 90 

    protected Vector3 CharacterPivot;   // 캐릭터 회전축? 
    protected Vector3 LookDirection;  // 캐릭터 시선방향  

    //Use this for initialization 
    void Awake()
    {
        Joystick = FindObjectOfType<FixedJoystick>();  // 왼쪽 조이스틱 스크립트 접근 
        TouchField = FindObjectOfType<FixedTouchField>(); // 터치화면 스크립트 접근
        ButtonA = FindObjectOfType<FixedButtonA>(); // 공격버튼 스크립트 접근 
        ButtonB = FindObjectOfType<FixedButtonB>(); // 점프버튼 스크립트 접근 
        ButtonR = FindObjectOfType<FixedButtonR>(); // 달리기/ONOFF 스크립트 접근 
        ButtonX = FindObjectOfType<FixedButtonX>(); // 줍기버튼접근
        ButtonDrop = FindObjectOfType<FixedButtonDrop>(); // 던지기 버튼 접근 
        ButtonAim = FindObjectOfType<FixedButtonAim>(); // 던지기 버튼 접근 
        AimPoint = FindObjectOfType<AimPoint>(); //조준점

       ButtonAim.gameObject.SetActive(false);
       AimPoint.gameObject.SetActive(false);


        ButtonWalk = FindObjectOfType<ButtonWalk>(); 
        Player = GetComponent<Player>(); // 플레이어 스크립트 접근 
        buttonWalk = GameObject.Find("Joystick/FixedJoystick/ButtonWalk");
    }

    internal static class YieldInstructionCache
    {
        public static readonly WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();
        public static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();
    }

    void FixedUpdate()
    {
        if (!isLocalPlayer)
        {
            return; 
        }
        
        // input 
        // 캐릭터는 양옆 (왼쪽,오른쪽) 은 360도 회전한다 
        InputRotationX = (InputRotationX + TouchField.TouchDist.x * RotationSpeed * Time.deltaTime) % 360f;
        // mathf.clamp  최소값 or 최대값 제한함수 
        // 캐릭터 카메라는 위아래방향으로 90도 아래로 확인 가능 
        InputRotationY = Mathf.Clamp(InputRotationY - TouchField.TouchDist.y * RotationSpeed * Time.deltaTime, -88f, 88f);
                                
        //left and forward  
                                              //카메라가 회전하는 방향으로 캐릭터의 정면이 된다. 
        var characterFoward = Quaternion.AngleAxis(InputRotationX, Vector3.up) * Vector3.forward;
       // Debug.Log("InputRotationX" + InputRotationX);
        var characterLeft = Quaternion.AngleAxis(InputRotationX + 90, Vector3.up) * Vector3.forward;

        //look and run direction 
        var runDirection = characterFoward * (Input.GetAxis("Vertical") + Joystick.AxisNormalized.y) + characterLeft * (Input.GetAxis("Horizontal") + Joystick.AxisNormalized.x);
        var lookDirection = Quaternion.AngleAxis(InputRotationY, characterLeft) * characterFoward;
        Player.Input.RunX = runDirection.x;
        Player.Input.RunZ = runDirection.z;
        Player.Input.LookX = lookDirection.x;
        Player.Input.LookZ = lookDirection.z;
        Player.Input.Attack = ButtonA.Pressed;
        Player.Input.Jump = ButtonB.Pressed;
        Player.Input.Run = ButtonR.Pressed;
        Player.Input.Pickup = ButtonX.Pressed;
        Player.Input.Aim = ButtonAim.Pressed;
        Player.Input.Drop = ButtonDrop.Pressed;
        ///걷기달기 문제시 복구가능
        if (ButtonR.Pressed)
        {
            ButtonWalk.gameObject.SetActive(false);
        }
        else
        {
            ButtonWalk.gameObject.SetActive(true);
            //  Debug.Log("x" + runDirection.x);
            // Debug.Log("z" + runDirection.z);
            // Debug.Log("lx" + lookDirection.x);
            // Debug.Log("ly" + lookDirection.y);
        }
        ///걷기달기 문제시 복구가능

        if (Player.grabItem)
        {
            StartCoroutine(TimerA());
        }
        else
        {
            StartCoroutine(TimerB());
        }



        // 캐릭터 회전구현 
        var characterPivot = Quaternion.AngleAxis(InputRotationX, Vector3.up) * CameraPivot;
        
        // 카메라 코루틴 실행 
        StartCoroutine(setCamera(lookDirection, characterPivot));
    }

    private IEnumerator setCamera(Vector3 lookDirection, Vector3 characterPivot)
    {
        yield return YieldInstructionCache.WaitForFixedUpdate;// 고정프레임 업데이트 함수가 호출될때까지 기다린다. 

        //set camera values   카메라의 방향을 실시간으로 변경한다. 
        Camera.main.transform.position = (transform.position + characterPivot) - lookDirection * CameraDistance;
        Camera.main.transform.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
    }

    private IEnumerator TimerA()
    {
        yield return new WaitForSeconds(0.5f);
        ButtonA.gameObject.SetActive(false);
        ButtonAim.gameObject.SetActive(true);
        AimPoint.gameObject.SetActive(true);

        ButtonDrop.gameObject.SetActive(true);
        ButtonX.gameObject.SetActive(false);
    }
    private IEnumerator TimerB()
    {
        yield return new WaitForSeconds(0.5f);
        ButtonA.gameObject.SetActive(true);
        ButtonAim.gameObject.SetActive(false);
        AimPoint.gameObject.SetActive(false);

        ButtonDrop.gameObject.SetActive(false);
        ButtonX.gameObject.SetActive(true);
    }
}
