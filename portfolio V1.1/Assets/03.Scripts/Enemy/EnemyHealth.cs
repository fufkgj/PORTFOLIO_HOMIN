using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private const string bulletTag = "Bullet";

    [SerializeField]
    private float initHp;
    [SerializeField]
    public float curHp;
    private Animator anim;
    private readonly int hashDie = Animator.StringToHash("IsDie");
    [SerializeField]
    private float Damage;

    public bool isDie = false;
    public GameObject bloodEffect;

    void OnEnable()
    {
        initHp = GameManager.instance.gameData.EnemyHp;
        curHp = initHp;
        Damage =  GameManager.instance.gameData.PlayerPower;
    }
    void Start()
    {      
    }
    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == bulletTag)//총알에 피격됐을 경우 체력 감소
        {   
            curHp = Mathf.Max(curHp -= Damage, 0f); //Damage만큼 체력 감소
            Vector3 pos = transform.position;
            Quaternion rot = transform.rotation;
            Instantiate(bloodEffect, pos, rot);  //맞은 위치에 bloodEffect를 실행
        }
    }
}
