using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public class DataManager : MonoBehaviour
{
    private string dataPath;

    public void Initialize()
    {
        dataPath = Application.persistentDataPath + "GameData.dat";
    }

    public void Save(GameData gameData)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(dataPath);

        GameData data = new GameData();

        data.Coin = gameData.Coin;

        data.PlayerHp = gameData.PlayerHp;
        data.PlayerPower = gameData.PlayerPower;
        data.PlayerSpeed = gameData.PlayerSpeed;

        data.HQHp = gameData.HQHp;

        data.FireRate = gameData.FireRate;
        data.MyBullet = gameData.MyBullet;
        data.GunBullet = gameData.GunBullet;
        data.ReloadingTime = gameData.ReloadingTime;
        data.ReloadingAnim = gameData.ReloadingAnim;
        data.haveRifle = gameData.haveRifle;
        data.haveMachinegun = gameData.haveMachinegun;

        data.maxEnemy = gameData.maxEnemy;
        data.EnemyHp = gameData.EnemyHp;
        data.EnemyPower = gameData.EnemyPower;
        data.EnemySpeed = gameData.EnemySpeed;
        data.EnemyAttackS = gameData.EnemyAttackS;

        data.Wave = gameData.Wave;
        
       
    bf.Serialize(file, data);
        file.Close();
    }
    public GameData Load()
    {
        if(File.Exists(dataPath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(dataPath, FileMode.Open);
            GameData data = (GameData)bf.Deserialize(file);
            file.Close();
            return data;
        }
        else
        {
            GameData data = new GameData();
            return data;
        }
    }
   
}
