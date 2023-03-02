using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealth : MonoBehaviour
{
    private const string enemyTag = "HitPoint";
    [SerializeField]
    private float initHp;
    [SerializeField]
    public float curHp;
    private Animator anim;
    private Aim aim;
    private readonly int hashDie = Animator.StringToHash("Die");
    [SerializeField]
    private float Damage;

    public bool isDie = false;

    [Header("UI")]
    public Image HpBar;

    void UpdateSetup()
    {
        initHp = GameManager.instance.gameData.PlayerHp;
        curHp += GameManager.instance.gameData.PlayerHp - curHp;        
    }
    void Start()
    {
        aim = GetComponent<Aim>();
        anim = GetComponent<Animator>();
        initHp = GameManager.instance.gameData.PlayerHp;
        curHp = initHp;
        HpBar.color = Color.green;
        StartCoroutine(DamageData());
    }
    IEnumerator DamageData() // 데미지 값을 gameData에서 가져오는 스크립트
    {
        Damage = 5*GameManager.instance.gameData.EnemyPower;
        yield return new WaitForSeconds(0.2f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == enemyTag) //적에게 피격시
        {
            curHp = Mathf.Max(curHp -= Damage, 0f); //Damage만큼 Hp감소
            DisplayHpBar();
        }
        if(curHp<= 0) 
        {          
            Die();
        }
    }
    public void Update()
    {
    }
    private void DisplayHpBar() //HP UI
    {
        curHp = Mathf.Clamp(curHp, 0, initHp);
        HpBar.fillAmount = (float)curHp / (float)initHp;  // Hpbar를 현재 Hp만큼 채움
        if (HpBar.fillAmount <= 0.3f)  // 남은 Hp가 30% 이하면 Hpbar 색을 빨간색으로 변경
            HpBar.color = Color.red;
        else if (HpBar.fillAmount <= 0.5f)
            HpBar.color = Color.yellow;
         
    }
    public void Die()
    {
        anim.SetTrigger(hashDie); //사망 애이메이션 재생
        isDie = true;
        Invoke("GameOver",10f); //대기 후 GameOver 실행      
    }
    public void GameOver()
    {
        GameManager.instance.GameOver();
    }
    public void GetHealth()
    {
        curHp += 10;
    }
}
