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

    private void OnEnable()
    {
        
    }
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
    IEnumerator DamageData()
    {
        Damage = 5*GameManager.instance.gameData.EnemyPower;
        yield return new WaitForSeconds(0.2f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == enemyTag)
        {
            curHp = Mathf.Max(curHp -= Damage, 0f);
            DisplayHpBar();
        }
        if(curHp<= 0)
        {          
            Die();
        }
    }
    public void Update()
    {
        if (curHp <= 0)
        {
            Die();
        }
    }
    private void DisplayHpBar()
    {
        curHp = Mathf.Clamp(curHp, 0, initHp);
        HpBar.fillAmount = (float)curHp / (float)initHp;
        if (HpBar.fillAmount <= 0.3f)
            HpBar.color = Color.red;
        else if (HpBar.fillAmount <= 0.5f)
            HpBar.color = Color.yellow;
         
    }
    public void Die()
    {
        anim.SetTrigger(hashDie);
        isDie = true;
        Invoke("GameOver",10f);       
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
