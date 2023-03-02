using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private float h = 0f;  //A,D 키보드
    private float v = 0f;  //W,S 키보드

    public float movespeed = 0f;
    public float rotspeed = 100f;
    public float jump = 10f;
    [SerializeField]
    private Transform tr;
    [SerializeField]
    private CapsuleCollider col;
    [SerializeField]
    private Rigidbody rbody;
    [SerializeField]
    private Animator anim;

    public bool isJump = false;
    public bool isRunning = false;
    public bool isWait = false;

    private PlayerHealth health;

    private readonly int hashMove = Animator.StringToHash("Movespeed");

    private void Awake()
    {
        tr = GetComponent<Transform>();
        col = GetComponent<CapsuleCollider>();
        rbody = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        health = GetComponent<PlayerHealth>();

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        Move();
        Jump();      
    }
    private void Move() //플레이어의 이동
    {
        if (!health.isDie && !isWait)
        {
            if (Input.GetKey(KeyCode.W))
            {
                if (Input.GetKey(KeyCode.LeftShift)) //달리기 스크립트
                {
                    movespeed = 6f;
                    isRunning = true;
                    anim.SetFloat(hashMove, 1.5f);
                }
                else
                {
                    isRunning = false;
                    movespeed = 4f;
                    anim.SetFloat(hashMove, 1f);
                }
            }
            else
                anim.SetFloat(hashMove, 0);
            if (Input.GetKey(KeyCode.S)) //뒷걸음질 시 이동속도 감소
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    movespeed = 3.5f;
                    isRunning = true;
                    anim.SetFloat(hashMove, 1.5f);
                }
                else
                {
                    isRunning = false;
                    movespeed = 3f;
                    anim.SetFloat(hashMove, 1f);
                }
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    anim.SetFloat(hashMove, 1.5f);
                }
                else
                {
                    anim.SetFloat(hashMove, 1f);
                }
            }


            Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h); //플레이어 이동 방향
            tr.Translate(moveDir.normalized * movespeed * Time.deltaTime, Space.Self);  // 플레이어가 이동 방향으로 movespeed의 속도로 이동

            anim.SetFloat("xDir", h);
            anim.SetFloat("yDir", v);
        }
        if (isWait)  //대기 중에는 애니메이션 고정 
        {
            anim.SetFloat("xDir", 0);
            anim.SetFloat("yDir", 0);
        }
    }

    private void Jump() //점프 스크립트
    {
        if (Input.GetKeyDown(KeyCode.Space) && isJump == false) 
        {
            rbody.AddForce(Vector3.up * jump, ForceMode.Impulse); // 위쪽으로 힘을 가함
            anim.SetTrigger("Jump");
        }
    }
    private void OnCollisionStay(Collision col)
    {
        if(col.collider.tag == "Tile" || col.collider.tag == "Stage") //Tile과 Stage와 접촉중인 경우 점프 가능
        {
            anim.SetBool("isJump", false);
            isJump = false;
        }
    }
    private void OnCollisionExit(Collision col) //Tile이나 Stage와 떨어지면 점프를 할 수 없음
    {
        if (col.collider.tag == "Tile" || col.collider.tag == "Stage")
        {
            anim.SetBool("isJump", true);
            isJump = true;
        }
    }
    
}
