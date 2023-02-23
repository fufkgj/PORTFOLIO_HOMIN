using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour
{
    public Transform target;
    public Vector3 vec;

    private Animator anim;
    private Transform spine;
    private PlayerHealth playerHealth;
    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        anim = GetComponent<Animator>();  // 캐릭터의 애니메이터
        spine = anim.GetBoneTransform(HumanBodyBones.Spine);  //캐릭터의 상반신 본
    }

    void LateUpdate()
    {
        if(playerHealth.curHp>0)
        {
            spine.LookAt(target.position); //캐릭터의 상반신만 타겟을 바라본다. 
            spine.rotation = spine.rotation * Quaternion.Euler(vec);
        }
    }
}
