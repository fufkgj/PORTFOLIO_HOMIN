using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]

public class EnemyMove : MonoBehaviour
{
    private float Speed =2f; //이동속도
    private NavMeshAgent agent;
    private Transform enemyTr;
    private Vector3 _traceTarget;
    public GameDataObject gameData;
    public Vector3 traceTarget
    {
        get { return _traceTarget; }
        set
        {
            _traceTarget = value;
            TraceTarget(_traceTarget);
        }
    }
    void Start()
    {
        enemyTr = GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        agent.updateRotation = false;
        agent.speed = Speed * gameData.EnemySpeed;
    }

    void TraceTarget(Vector3 pos)
    {
        if (agent.isPathStale) return;
        agent.destination = pos;
        agent.isStopped = false;
    }
    public void Stop()//추적을 정지 시키는 함수 
    {
        agent.isStopped = true;
        //바로 정지 하기 위해서 속도를 0으로 설정
        agent.velocity = Vector3.zero;
    }
    void Update()
    {
        if (GetComponent<EnemyAI>().isDie == true) return;
        if (agent.isStopped == false)
        {
            Quaternion rot = Quaternion.LookRotation(agent.desiredVelocity);
            enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot,
                                             Time.deltaTime);
        }
    }
}
