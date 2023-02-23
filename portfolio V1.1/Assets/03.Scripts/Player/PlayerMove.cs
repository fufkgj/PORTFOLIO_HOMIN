using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private float h = 0f;  //A,D 키보드
    private float v = 0f;  //W,S 키보드
    private float r = 0f;  //마우스 좌 우
    private float u = 0f;  //마우스 상 하

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
        r = Input.GetAxis("Mouse X");
        u = Input.GetAxis("Mouse Y");
        Move();
        Jump();      
    }
    private void Move()
    {
        if (!health.isDie && !isWait)
        {
            if (Input.GetKey(KeyCode.W))
            {
                if (Input.GetKey(KeyCode.LeftShift))
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
            if (Input.GetKey(KeyCode.S))
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


            Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);
            tr.Translate(moveDir.normalized * movespeed * Time.deltaTime, Space.Self);
            Vector3 rotDir = (Vector3.up * r);
            tr.Rotate(rotDir.normalized * rotspeed * Time.deltaTime);

            anim.SetFloat("xDir", h);
            anim.SetFloat("yDir", v);
        }
        if (isWait)
        {
            anim.SetFloat("xDir", 0);
            anim.SetFloat("yDir", 0);
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isJump == false) 
        {
            rbody.AddForce(Vector3.up * jump, ForceMode.Impulse);
            anim.SetTrigger("Jump");
        }
    }
    private void OnCollisionStay(Collision col)
    {
        if(col.collider.tag == "Tile" || col.collider.tag == "Stage")
        {
            anim.SetBool("isJump", false);
            isJump = false;
        }
    }
    private void OnCollisionExit(Collision col)
    {
        if (col.collider.tag == "Tile" || col.collider.tag == "Stage")
        {
            anim.SetBool("isJump", true);
            isJump = true;
        }
    }
    
}
