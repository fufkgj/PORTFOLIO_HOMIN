using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyFire : MonoBehaviour
{
    public enum WeaponType
    {
        PISTOL = 0,
        RIFLE,
        //SHOTGUN,
        MACHINEGUN
    }
    public WeaponType curWeapon = WeaponType.PISTOL;
    [SerializeField]
    public int WeaponNum;
    [SerializeField]
    public GameObject[] Weapon;
    public Transform[] FirePosList;
    

    [SerializeField]
    private Transform tr;
    public GameObject BulletPrefab;
    public Transform FirePos;

    [SerializeField]
    private GameObject[] Enemy;
    public Transform targetTr;

    [SerializeField]
    private float currentDist = 0f; //현재 거리
    [SerializeField]
    private float closeDist = 100f; //가까운 거리
    [SerializeField]
    private float TargetDist = 100f; //타겟과의 거리    
    [SerializeField]
    private float attackRange = 30.0f;  // 사정거리
    [SerializeField]
    int closeDistIdx = 0; //가장 가까운 인덱스
    int TargetIdx = -1;  // 타겟팅 할 인덱스

    [SerializeField]
    private Animator anim;
    public Vector3 vec;


    public GameDataObject gameData;
    [SerializeField]
    public int maxBullet;  // 탄창 최대 총알 수
    [SerializeField]
    public int gunBullet;   //현재 남은 탄
    [SerializeField]
    private float ReloadingTime;
    [SerializeField]
    private int MyBullet;
    private bool isReloading;
    public float nextFire;
    public float fireRate;


    void Start()
    {
        isReloading = false;
        tr = GetComponent<Transform>();
        anim = GetComponent<Animator>();  // 캐릭터의 애니메이터     
        ReloadingTime = gameData.ReloadingTime;
    }

    void Update()
    {
        WeaponCheck();
        gunBullet = maxBullet;
        Enemy = GameObject.FindGameObjectsWithTag("Enemy");      
        if (Enemy.Length != 0) //적이 존재할 경우
        {
            closeDist = 100f;
            TargetDist = 100f;
            TargetIdx = -1;
            
            for (int i = 0; i < Enemy.Length; i++) //가장 가까운 적을 찾는 스크립트
            {
                currentDist = Vector3.Distance(FirePos.position, Enemy[i].transform.position); //오브젝트와 i번째 적과의 거리 측정
                RaycastHit hit;              
                Debug.DrawRay(FirePos.position, Enemy[i].transform.position - FirePos.position,Color.red);
                bool isHit = Physics.Raycast(FirePos.position, Enemy[i].transform.position - FirePos.position, out hit); // i번째 적과 오브젝트 사이에 장애물이 없을 경우를 판단
                Debug.Log(hit.transform.name);

                if (isHit && hit.transform.CompareTag("Enemy"))
                {
                    if (TargetDist <= currentDist) 
                    {
                        TargetIdx = i;
                        TargetDist = currentDist;                                           
                    }
                    if (closeDist >= currentDist) //전에 측정한 제일 가까운 적보다 이번에 측정한 적이 가까울경우
                    {
                        closeDistIdx = i;  //이번에 측정한 적을 제일 가까운 적으로 설정
                        closeDist = currentDist;
                    }
                }
                             
            }
            if (TargetIdx == -1) 
            {
                TargetIdx = closeDistIdx;
            }
                     
        }       

        targetTr = Enemy[closeDistIdx].transform;
        Fire();
    }
    void WeaponCheck() //활성화된 총기 체크
    {
        if (WeaponNum == 0)
        {
            curWeapon = WeaponType.PISTOL;
        }
        else if (WeaponNum == 1) 
        {
            curWeapon = WeaponType.RIFLE;
        }
        else if (WeaponNum ==2)
        {
            curWeapon = WeaponType.MACHINEGUN;
        }
        switch (curWeapon)
        {
            case WeaponType.PISTOL:
                Weapon[0].SetActive(true);
                Weapon[1].SetActive(false);
                Weapon[2].SetActive(false);
                anim.SetInteger("Weapon", 0);
                fireRate = 1.5f * gameData.FireRate;  //사격 속도
                FirePos = FirePosList[0];
                break;
            case WeaponType.RIFLE:
                Weapon[0].SetActive(false);
                Weapon[1].SetActive(true);
                Weapon[2].SetActive(false);
                fireRate = 0.5f * gameData.FireRate;  //사격 속도
                anim.SetInteger("Weapon", 1);
                FirePos = FirePosList[1];
                break;
            case WeaponType.MACHINEGUN:
                Weapon[0].SetActive(false);
                Weapon[1].SetActive(false);
                Weapon[2].SetActive(true);
                fireRate = 0.3f * gameData.FireRate;  //사격 속도
                anim.SetInteger("Weapon", 1);
                FirePos = FirePosList[2];
                break;
        }
    }
    void Fire() //사격 스크립트
    {
        if (closeDist <= attackRange) //제일 가까운 적이 공격 범위 안에 있을 경우 사격
        {
            Quaternion rot = Quaternion.LookRotation(targetTr.transform.position - transform.position); // 타겟 방향으로 오브젝트를 회전
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime*20f );
            FirePos.LookAt(targetTr.transform.position); //타겟을 바라보고 사격
            Debug.DrawRay(FirePos.position, FirePos.forward * attackRange, Color.blue); //레이캐스트를 그린다

            if (!isReloading && gunBullet > 0) 
            {
                if (Time.time > nextFire)
                {
                    --gunBullet;
                    var _bullet = GameManager.instance.GetBullet();
                    if (_bullet != null)
                    {

                        _bullet.transform.position = FirePos.position;
                        _bullet.transform.rotation = FirePos.rotation;
                        _bullet.SetActive(true);
                    }
                    if (gunBullet == 0)
                    {
                            StartCoroutine(Reloading());
                    }
                    nextFire = Time.time + fireRate;
                }
            }
        }
    }
    IEnumerator Reloading() //재장전 스크립트
    {
        isReloading = true;
        anim.SetBool("isReloading", true);
        yield return new WaitForSeconds(ReloadingTime);
        gunBullet = maxBullet;  //재장전 탄창 만큼 탄 충전
        isReloading = false;
    }

}
