using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FireCtrl : MonoBehaviour
{
    public enum WeaponType //무기의 종류
    {
        PISTOL = 0,
        RIFLE,
        //SHOTGUN,
        MACHINEGUN 
    }
    public WeaponType curWeapon = WeaponType.PISTOL; //캐릭터가 들고있는 무기를 저장 할 변수, 기본무기 피스톨
    [SerializeField]
    public GameObject[] Weapon;  // 무기
    public Transform[] FirePosList;
    public Transform FirePos; //총알이 발사 될 위치 
    private int layerMask;
    private PlayerMove player;
    private PlayerHealth health;

    public GameDataObject gameData;
    public int remainingBullet; //재장전시 충전되는 총알 수
    [SerializeField]
    public int maxBullet;  // 탄창 최대 총알 수
    [SerializeField]
    public int gunBullet;   //현재 남은 탄
    [SerializeField]
    private float ReloadingTime;
    [SerializeField]
    private float ReloadingAnim;
    [SerializeField]
    private int MyBullet;
    private bool isReloading = false;
    public ParticleSystem[] muzzleFlash;
    [SerializeField]
    private ParticleSystem Flash;

    public float nextFire;
    public float fireRate;
   
    [SerializeField]
    private Animator anim;


    [Header("UI")]
    public Text MyBulletTxt;
    public Text MaxBullletTxt;
    public Text gunBulletTxt;

    [Header("Sound")]
    public int ClipNum;
    public AudioClip[] Clip;
    public AudioSource FireSound;


    void Start()
    {
        anim = GetComponent<Animator>();
        player = GetComponent<PlayerMove>();
        health = GetComponent<PlayerHealth>();       
        MyBullet = gameData.MyBullet;  //총 탄약      
        ReloadingTime = 1 * gameData.ReloadingTime;  // 재장전 시간
        ReloadingAnim = 1f * gameData.ReloadingAnim;  
        gunBullet = maxBullet;  //탄창을 최대로 채우고 시작
        FirePos = FirePosList[0];
        Flash = muzzleFlash[0];
        Flash.Stop();
        ClipNum = 0;
        FireCheck();
    }
    void Update()
    {      
        Debug.DrawRay(FirePos.position, FirePos.forward * 20f, Color.green); //레이캐스트를 그린다
        MyBullet = gameData.MyBullet;
        
        remainingBullet = maxBullet;
        BulletDisplay();
        WeaponChange();
        FireCheck();
    }

    private void FireCheck() // 사격 판단
    {
        if (!health.isDie)
        {
            if (Input.GetMouseButton(0) && !player.isRunning && !isReloading && gunBullet > 0 &&GameManager.instance.isWait == false)
            {
                if (Time.time > nextFire)
                {
                    --gunBullet;
                    Fire();
                    if (gunBullet == 0)
                    {
                        if (MyBullet > 0)
                        {
                            StartCoroutine(Reloading()); //재장전 스크립트 실행
                        }

                    }
                    nextFire = Time.time + fireRate; 
                }             
            }
            else
                Flash.Stop();

            if (Input.GetKeyDown(KeyCode.R) && MyBullet > 0 && gunBullet != maxBullet) 
            {
                StartCoroutine(Reloading()); 
            }
            
        }
    }
    private void Fire()//사격 스크립트
    {
        var _bullet = GameManager.instance.GetBullet();
        if (_bullet != null)
        {
            FireSound.PlayOneShot(Clip[ClipNum]);
            _bullet.transform.position = FirePos.position;
            _bullet.transform.rotation = FirePos.rotation;
            _bullet.SetActive(true);
            Flash.Play();
        }
    }
    private void BulletDisplay()  //총알 정보 UI
    {
        MyBulletTxt.text = MyBullet.ToString("00000");
        gunBulletTxt.text = gunBullet.ToString("000");
        MaxBullletTxt.text ="/ "+ maxBullet.ToString("000");
    }
    void WeaponChange() //무기 변경
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && ClipNum != 0)
        {
            curWeapon = WeaponType.PISTOL;
            gunBullet = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && gameData.haveRifle == true &&ClipNum != 1)
        {
            curWeapon = WeaponType.RIFLE;
            gunBullet = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && gameData.haveMachinegun == true && ClipNum!=2)
        {
            curWeapon = WeaponType.MACHINEGUN;
            gunBullet = 0;
        }
        switch (curWeapon)
        {
            case WeaponType.PISTOL:
                Weapon[0].SetActive(true);
                Weapon[1].SetActive(false);
                Weapon[2].SetActive(false);
                anim.SetInteger("Weapon", 0);               
                fireRate = 1.2f * gameData.FireRate;  //사격 속도
                maxBullet = 15 * gameData.GunBullet; //최대 탄창
                FirePos = FirePosList[0];
                Flash = muzzleFlash[0];
                ClipNum = 0;
                break;
            case WeaponType.RIFLE:
                Weapon[0].SetActive(false);
                Weapon[1].SetActive(true);
                Weapon[2].SetActive(false);
                fireRate = 0.3f * gameData.FireRate;  //사격 속도
                maxBullet = 30 * gameData.GunBullet; //최대 탄창
                anim.SetInteger("Weapon", 1);
                FirePos = FirePosList[1];
                Flash = muzzleFlash[1];
                ClipNum = 1;
                break;
            case WeaponType.MACHINEGUN:
                Weapon[0].SetActive(false);
                Weapon[1].SetActive(false);
                Weapon[2].SetActive(true);
                fireRate = 0.1f * gameData.FireRate;  //사격 속도
                maxBullet = 100 * gameData.GunBullet; //최대 탄창
                anim.SetInteger("Weapon", 1);
                FirePos = FirePosList[2];
                Flash = muzzleFlash[2];
                ClipNum = 2;
                break;
        }
    }
    IEnumerator Reloading() // 재장전 스크립트
    {
        isReloading = true;
        anim.SetBool("isReloading", true);
        anim.SetFloat("ReloadSpeed", ReloadingAnim);
        yield return new WaitForSeconds(ReloadingTime);
        if (gameData.MyBullet <= maxBullet)  // 내가 가진 탄 수가 탄창에 들어갈 탄 수 보다 적을 경우
        {
            remainingBullet = gameData.MyBullet;  //재장전 되는 탄은 내가 가진 모든 탄
        }
        else
        {
            remainingBullet = maxBullet;   //재장전 되는 탄은 최대 탄   
        }        
        GameManager.instance.useBullet();
        MyBullet -= remainingBullet - gunBullet;  //재장전 한 만큼 최대 탄 감소 
        gunBullet = remainingBullet;  //재장전 탄창 만큼 탄 충전       
        isReloading = false;
        anim.SetBool("isReloading", false);
    }
}
