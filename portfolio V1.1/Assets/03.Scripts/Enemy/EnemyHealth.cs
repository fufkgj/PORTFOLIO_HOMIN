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
        if (other.gameObject.tag == bulletTag)
        {   
            curHp = Mathf.Max(curHp -= Damage, 0f);
            Vector3 pos = transform.position;
            Quaternion rot = transform.rotation;
            Instantiate(bloodEffect, pos, rot);
        }
    }
}
