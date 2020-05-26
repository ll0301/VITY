using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    
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
    }
    public const float Speed = 3f; // 캐릭터 이동스피드 
    public const float JumpForce = 4f; // 캐릭터 점프 높이 

    protected Rigidbody Rigidbody;   // 게임오브젝트가 물리적제어로 동작하게한다.   
    protected Quaternion LookRotation; // 사원수 EULER 좌표계의 GIMBALLOCK 현상을 해결하기위해 고안된 좌표. 
    protected Animator Animator; // 
    protected bool Grounded;  // 캐릭터가 바닥에 있을때
    // Start is called before the first frame update

    void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
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

            // Debug.Log("공격");
            Animator.SetBool("Attack", true);
        }

        Animator.SetBool("Grounded", Grounded);  // 캐릭터가 땅에 있는 상태인지 공중에 뜬 상태인지 계속 업데이트 해준다. 

        // Quaternion.Inverse => 회전의 역방향을 되돌린다.  
        var localVelocity = Quaternion.Inverse(transform.rotation) * (Rigidbody.velocity / Speed);
        Animator.SetFloat("RunX", localVelocity.x);  // 캐릭터의 진행방향 앞인지 뒤인지 체크 
        Animator.SetFloat("RunZ", localVelocity.z);  // 캐릭터의 진행방향 왼쪽인지 오른쪽인지 체크 

        //position
        // maxLength로 고정된 magnitude와 함께 vector 사본을 반환한다. 
        var inputRun = Vector3.ClampMagnitude(new Vector3(Input.RunX, 0, Input.RunZ), 1);
        var inputLook = Vector3.ClampMagnitude(new Vector3(Input.LookX, 0, Input.LookZ), 1);
        //rigidbody의 속력벡터를 나타낸다.  
        Rigidbody.velocity = new Vector3(inputRun.x * Speed, Rigidbody.velocity.y, inputRun.z * Speed);

        //rotation to go target 
        if (inputLook.magnitude > 0.01f)
            // input rook의 크기 
            // Quaternion.AngleAxis (각도,축) =  축 기준으로 각도만큼 회전 
            LookRotation = Quaternion.AngleAxis(Vector3.SignedAngle(Vector3.forward, inputLook, Vector3.up), Vector3.up);

        // 중점과 반지름을 주면  가상의 원을 만들어 안에있거나 접촉하는 collider들을 반환하는 함수 
        Grounded = Physics.OverlapSphere(transform.position, 0.3f, 1).Length > 1;

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
}
