using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum State
    {
        TRACEGATE,
        TRACEPLAYER, 
        ATTACK,
        DIE
    }
    public State state = State.TRACEGATE;

    [SerializeField]
    private Transform playerTr;
    private Transform enemyTr;
    [SerializeField]
    private Transform gateTr; // 기본 공격 목표
    private Animator anim;
    private EnemyAttack enemyAttack;
    private EnemyMove enemyMove;
    private EnemyHealth enemyHealth;
    public float attackDist = 5.0f;
    public float traceDist = 10.0f;
    public bool isDie = false;

    public GameObject HitPointL;
    public GameObject HitPointR;

    private readonly int hashMove = Animator.StringToHash("isMove");
    private readonly int hashSpeed = Animator.StringToHash("Speed");
    private readonly int hashDie = Animator.StringToHash("IsDie");
    private readonly int hashOffset = Animator.StringToHash("Offset");
    private readonly int hashPlayerDie = Animator.StringToHash("PlayerDie");
    private readonly int hashWalkSpeed = Animator.StringToHash("WalkSpeed");
    private readonly int hashDieIdx = Animator.StringToHash("DieIdx");

    void Awake()
    {
        //플레이어의 위치 확인
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerTr = player.GetComponent<Transform>();
        //게이트의 위치 확인
        var gate = GameObject.FindGameObjectWithTag("Gate");
        if (gate != null)
            gateTr = gate.GetComponent<Transform>();

        enemyTr = GetComponent<Transform>();
        enemyAttack = GetComponent<EnemyAttack>();
        enemyMove = GetComponent<EnemyMove>();
        enemyHealth = GetComponent<EnemyHealth>();
        anim = GetComponent<Animator>();
        anim.SetFloat(hashWalkSpeed, Random.Range(0.5f,3f));
        anim.SetInteger(hashDieIdx, Random.Range(0, 6));
    }
    private void OnEnable()
    {
        StartCoroutine(CheckState());
        StartCoroutine(Action());
    }
    IEnumerator CheckState()//적 Ai의 상태 체크 
    {
        yield return new WaitForSeconds(0.1f);
        while (!isDie)
        {    //상태가 사망이면 코루틴 함수를 종료시킴
            if (state == State.DIE) yield break;
            float playerdist = Vector3.Distance(playerTr.position, enemyTr.position);
            float gatedist = Vector3.Distance(gateTr.position, enemyTr.position);
            if (playerdist <= traceDist) //플레이어가 추적 범위 내에 있을 경우
            {
                state = State.TRACEPLAYER;

                if (playerdist <= attackDist) //플레이어와의 거리가 공격 범위 내일 경우
                {
                    state = State.ATTACK;
                }
                else
                    state = State.TRACEPLAYER;
            }
            else //플레이어가 추적 범위 밖에 있을 경우
            {
                state = State.TRACEGATE;

                if (gatedist <= attackDist)
                {
                    state = State.ATTACK;
                }
            }
            if(enemyHealth.curHp<=0)
            {
                state = State.DIE;
            }
            yield return new WaitForSeconds(0.3f); //0.3초 동안 대기하는 동안 제어권을 양보 
        }
    }
    IEnumerator Action()
    {
        while (!isDie)
        {
            yield return new WaitForSeconds(0.2f);
            switch (state)
            {
                case State.TRACEGATE: //타겟이 게이트
                    enemyAttack.isAttack = false;
                    enemyMove.traceTarget = gateTr.position;
                    enemyAttack.targetTr = gateTr.transform;
                    break;
                case State.TRACEPLAYER: // 타겟이 플레이어
                    enemyAttack.isAttack = false;
                    enemyMove.traceTarget = playerTr.position;
                    enemyAttack.targetTr = playerTr.transform;
                    break;
                case State.ATTACK: //타겟이 공격 범위 내
                    enemyMove.Stop();
                    anim.SetBool(hashMove, false);
                    if (enemyAttack.isAttack == false)
                        enemyAttack.isAttack = true;                  
                    break;
                case State.DIE: //사망
                    Die();
                    break;
            }
        }
    }
    public void Die() //사망
    {
        GameManager.instance.KillData();
        HitPointL.SetActive(false); // 공격판정 콜라이더를 끔
        HitPointR.SetActive(false);
        gameObject.tag = "Untagged"; 
        isDie = true;
        enemyMove.Stop(); //움직임 멈춤
        anim.SetInteger(hashDieIdx, Random.Range(0, 6));  // 사망 애니메이션 랜덤 재생
        anim.SetTrigger(hashDie);      
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        --GameManager.instance.currentEnemy;
        StopAllCoroutines();
        StartCoroutine(PushObjectPool());
    }
    void Update()
    {
    }
    IEnumerator PushObjectPool() //오브젝트 초기화
    {
        yield return new WaitForSeconds(3f);
        isDie = false;
        gameObject.tag = "Enemy";
        state = State.TRACEGATE;
        GetComponent<CapsuleCollider>().enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
        gameObject.SetActive(false);
    }
}
