using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour
{
    //input 
    protected FixedJoystick Joystick;   // 왼쪽 캐릭터 이동 조이스틱 
    protected FixedTouchField TouchField;   // 화면을 터치하여 캐릭터의 시선변경 
    protected FixedButtonA ButtonA; // 공격버튼 
    protected FixedButtonB ButtonB; // 점프버튼 
    protected TestPlayer Player; // 플레이어 스크립트 

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
        Player = GetComponent<TestPlayer>(); // 플레이어 스크립트 접근 
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
        var characterLeft = Quaternion.AngleAxis(InputRotationX + 90, Vector3.up) * Vector3.forward;

        //look and run direction 
        var runDirection = characterFoward * (Input.GetAxis("Vertical") + Joystick.AxisNormalized.y) + characterLeft * (Input.GetAxis("Horizontal") + Joystick.AxisNormalized.x);
        var lookDirection = Quaternion.AngleAxis(InputRotationY, characterLeft) * characterFoward;

        //set player values 
        // 인풋버튼과 캐릭터의 움직임을 연결한다. 
        Player.Input.RunX = runDirection.x;
        Player.Input.RunZ = runDirection.z;
        Player.Input.LookX = lookDirection.x;
        Player.Input.LookZ = lookDirection.z;
        Player.Input.Attack = ButtonA.Pressed;
        Player.Input.Jump = ButtonB.Pressed;

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
}
