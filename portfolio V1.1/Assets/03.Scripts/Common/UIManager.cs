using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    public GameDataObject gameData;
    public void OnClickNewGameBtn()
    {
        OnResetData();
        SceneManager.LoadScene("GameScene");
    }
    public void OnClickRoadGameBtn()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void OnResetData()
    {
        gameData.Coin = 0;  // 가진 돈

        // 이 밑의 변수는 float으로 변경 후 곱연산으로 변경!

        gameData.PlayerHp = 100f;  //플레이어 체력
        gameData.PlayerPower = 10f; // 플레이어 공격력
        gameData.PlayerSpeed = 3f;  // 플레이어 속도

        gameData.HQHp = 100;  //본진 체력

        gameData.FireRate = 2f;  // 공격 속도
        gameData.MyBullet = 100;  //가진 총알 총 수
        gameData.GunBullet = 1; //탄창 수      
        gameData.ReloadingTime = 1.0f;
        gameData.haveRifle = false;
        gameData.haveMachinegun = false;

        gameData.maxEnemy = 30;
        gameData.EnemySpeed = 1f;  // 적 속도
        gameData.EnemyHp = 100f;  // 적 체력
        gameData.EnemyPower = 1f;  // 적 공격력
        gameData.EnemyAttackS = 1f; //적 공격 속도
    }
    public void OnGameQuit()
    {
        Application.Quit();
    }
}
