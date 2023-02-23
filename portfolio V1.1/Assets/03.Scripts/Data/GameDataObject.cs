using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameDataSO", menuName = "Create GameData", order = 0)]
public class GameDataObject : ScriptableObject
{
    [Header("플레이어")]
    public int Coin = 0;

    public float PlayerHp = 100f;
    public float PlayerPower = 1f;
    public float PlayerSpeed = 3f;
    [Header("본진")]
    public int HQHp = 100;

    [Header("총")]
    public float FireRate = 2f;
    public int MyBullet = 100;
    public int GunBullet = 1;
    public float ReloadingTime = 1.0f;
    public float ReloadingAnim = 1.0f;
    public bool haveRifle = false;
    public bool haveMachinegun = false; 

    [Header("적")]
    public int maxEnemy = 30;
    public float EnemySpeed = 1f;
    public float EnemyHp = 100f;
    public float EnemyPower = 1f;
    public float EnemyAttackS = 1f;

    [Header("기타")]
    public int Wave = 1; 
       
}
