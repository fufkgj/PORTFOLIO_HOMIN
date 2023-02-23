using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HQHP : MonoBehaviour
{
    private const string enemyTag = "HitPoint";
    [SerializeField]
    private float initHp;
    [SerializeField]
    private float curHp;
    [SerializeField]
    private float Damage;

    [Header("UI")]
    public Image HpBar;

    void Start()
    {
        initHp = GameManager.instance.gameData.HQHp;
        curHp = initHp;
        HpBar.color = Color.green;
        StartCoroutine(DamageData());
    }
    IEnumerator DamageData()
    {
        Damage = GameManager.instance.gameData.EnemyPower;
        yield return new WaitForSeconds(0.2f);
    }
    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == enemyTag)
        {
            curHp = Mathf.Max(curHp -= Damage, 0f);
            DisplayHpBar();
        }
        if (curHp <= 0)
        {
            Invoke("GameOver", 10f);
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
    public void GameOver()
    {
        GameManager.instance.GameOver();
    }
}
