using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class GameData
    {
        public int Coin = 0;  // 가진 돈

        // 이 밑의 변수는 float으로 변경 후 곱연산으로 변경!

        public float PlayerHp = 100f;  //플레이어 체력
        public float PlayerPower = 10f; // 플레이어 공격력
        public float PlayerSpeed = 3f;  // 플레이어 속도

        public int HQHp = 100;  //본진 체력

        public float FireRate = 2f;  // 공격 속도
        public int MyBullet = 100;  //가진 총알 총 수
        public int GunBullet = 1; //탄창 수      
        public float ReloadingTime = 1.0f;
        public float ReloadingAnim = 1.0f;
        public bool haveRifle = false;
        public bool haveMachinegun = false;

        public int maxEnemy = 30;
        public float EnemySpeed = 1f;  // 적 속도
        public float EnemyHp = 100f;  // 적 체력
        public float EnemyPower = 1f;  // 적 속도
        public float EnemyAttackS = 1f; //적 공격 속도

        public int Wave = 1; // 현재 웨이브
    }

