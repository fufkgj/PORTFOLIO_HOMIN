using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public Transform targetTr;
    private Transform enemyTr;
    private Animator anim;
    private EnemyMove enemyMove;

    private float AttackSpeed;
    private float AttackRate = 0.1f;
    private readonly float damping = 10f;

    public bool isAttack = false;
    
    void Start()
    {
        anim = GetComponent<Animator>();
        enemyMove = GetComponent<EnemyMove>();
        enemyTr = GetComponent<Transform>();
       
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttack)
        {
            if (Time.time >= AttackSpeed)
            {
                Attack();
                AttackSpeed = Time.time + AttackRate + Random.Range(0.1f, 0.35f);
            }
            Quaternion rot = Quaternion.LookRotation(targetTr.position -
                                                           enemyTr.position);
            enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot, Time.deltaTime
                                                   * damping);
        }
        else
            anim.SetBool("isAttack", false);
    }
    void Attack()
    {
        anim.SetBool("isAttack",true);
    }
}
