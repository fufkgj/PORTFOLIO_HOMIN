using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    [Header("Enemy pool")]
    [SerializeField]
    GameObject SpawnPoint;
    [SerializeField]
    GameObject Player;
    [SerializeField]
    GameObject UI;
    [SerializeField]
    Transform[] Points;
    public GameObject[] EnemyPrefab;
    [SerializeField]
    private GameObject CardUI;
    [SerializeField]
    private GameObject MarketUI;
    public int Maxenemy = 100;
    public int WaveEnemy; // 웨이브 출현 적
    public int EnemyCount = 0; //생성된 적
    public int currentEnemy; //남은 적

    [Header("object pool")]
    public GameObject bulletPrefab;
    public int maxPool = 10;
    List<GameObject> bulletPool = new List<GameObject>();
    public List<GameObject> enemyPool = new List<GameObject>();

    [SerializeField]
    private FireCtrl fireCtrl;
    [SerializeField]
    private DataManager dataManager;
    public GameDataObject gameData;
    [SerializeField]
    private PlayerMove playerMove;
    [SerializeField]
    private PlayerHealth playerHealth;
    [SerializeField]
    private Button PauseBtn;
    [SerializeField]
    private Button MenuBtn;
    [SerializeField]
    private Button ResumeBtn;
    [SerializeField]
    private Button MarketSbtn;
    [SerializeField]
    private Button NextBtn;
    [SerializeField]
    private Button BuyRifleBtn;
    [SerializeField]
    private Button BuyMachinegunBtn;
    [SerializeField]
    private Button WaveStartBtn;
    [SerializeField]
    private Button ShopBtn;
    [SerializeField]
    private Button Quitbtn;

    public bool isGameOver = false;
    public bool isWait = false;
    public bool isPause = false;
    public static GameManager instance = null;

    [Header("UI")]
    [SerializeField]
    private Text Coin;
    [SerializeField]
    private GameObject BuyR;
    [SerializeField]
    private GameObject BuyM;
    [SerializeField]
    private GameObject MenuUI;
    [SerializeField]
    private GameObject GameoverUI;

    [Header("Card")]
    public GameObject card;
    public Card selectcard;
    public bool isSelect = false;

    //public bool Load = false;

    [Header("Main")]
    [SerializeField]
    private Button newbtn;
    [SerializeField]
    private Button loadbtn;
    [SerializeField]
    private Button exitbtn;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        newbtn = GameObject.Find("UI_Canvas").transform.GetChild(0).GetChild(0).gameObject.GetComponent<Button>();
        loadbtn = GameObject.Find("UI_Canvas").transform.GetChild(0).GetChild(1).gameObject.GetComponent<Button>();
        exitbtn = GameObject.Find("UI_Canvas").transform.GetChild(0).GetChild(2).gameObject.GetComponent<Button>();
        isPause = false;
        dataManager = GetComponent<DataManager>();
        dataManager.Initialize();
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "StartScene")
        {
            MaindLoading();
        }
        else if(scene.name == "GameScene")
        {
            Loading();
        }
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void Update()
    {
       if(SceneManager.GetActiveScene().name == "StartScene")
        {
            newbtn.onClick.AddListener(OnClickNewGameBtn);
            loadbtn.onClick.AddListener(OnClickRoadGameBtn);
            exitbtn.onClick.AddListener(OnGameQuit);
        }

        if (SceneManager.GetActiveScene().name == "GameScene")
        { 
                Coin = GameObject.Find("UI").transform.GetChild(0).transform.GetChild(3).transform.GetChild(0).gameObject.GetComponent<Text>();
                Coin.text = "Coin : " + gameData.Coin.ToString("0000");             
            if (currentEnemy <= 0 && isSelect == false)
            {
                WaveClear();
                playerMove.isWait = true;
            }
            if (Input.GetKeyDown(KeyCode.Escape) )
            {
                OnPauseClick();
            }
            PauseBtn.onClick.AddListener(OnPauseClick);
            ResumeBtn.onClick.AddListener(OnPauseClick);
            exitbtn.onClick.AddListener(OnGameQuit);
            BuyMachinegunBtn.onClick.AddListener(BuyMachineGun);
            BuyRifleBtn.onClick.AddListener(BuyRifle);
            ShopBtn.onClick.AddListener(ClickShopBtn);
            WaveStartBtn.onClick.AddListener(ClickNext);
            NextBtn.onClick.AddListener(ClickNext);
            Quitbtn.onClick.AddListener(OnGameQuit);
        }        
        //WaveEnemy = gameData.maxEnemy;      
    }
    public void Loading() // 씬 로딩
    {
        SpawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");
        Player = GameObject.FindGameObjectWithTag("Player");
        UI = GameObject.Find("UI");
        CardUI = UI.transform.GetChild(2).gameObject;
        GameoverUI = UI.transform.GetChild(3).gameObject;
        GameoverUI.SetActive(false);
        playerMove = Player.GetComponent<PlayerMove>();
        playerHealth = Player.GetComponent<PlayerHealth>();
        fireCtrl = Player.GetComponent<FireCtrl>();
        Points = SpawnPoint.GetComponentsInChildren<Transform>();
        MarketUI = UI.transform.GetChild(1).gameObject;     
        BuyR = MarketUI.transform.GetChild(0).GetChild(0).gameObject;
        BuyM = MarketUI.transform.GetChild(0).GetChild(1).gameObject;
        MenuUI = UI.transform.GetChild(0).GetChild(5).gameObject;
        PauseBtn = UI.transform.GetChild(0).GetChild(4).gameObject.GetComponent<Button>();
        exitbtn = UI.transform.GetChild(0).GetChild(5).GetChild(1).gameObject.GetComponent<Button>();
        ResumeBtn = UI.transform.GetChild(0).GetChild(5).GetChild(0).gameObject.GetComponent<Button>();
        BuyMachinegunBtn = BuyM.GetComponent<Button>();
        BuyRifleBtn = BuyR.GetComponent<Button>();
        ShopBtn = CardUI.transform.GetChild(2).GetComponent<Button>();
        WaveStartBtn = MarketUI.transform.GetChild(1).GetComponent<Button>();
        NextBtn = CardUI.transform.GetChild(1).GetComponent<Button>();
        Quitbtn = GameoverUI.transform.GetChild(1).GetChild(0).GetComponent<Button>();
        WaveEnemy = gameData.maxEnemy;
        currentEnemy = WaveEnemy;
        CreatePoolling();
    } 
    public void MaindLoading() //메인메뉴 씬 로딩
    {
        newbtn = GameObject.Find("UI_Canvas").transform.GetChild(0).GetChild(0).gameObject.GetComponent<Button>();
        loadbtn = GameObject.Find("UI_Canvas").transform.GetChild(0).GetChild(1).gameObject.GetComponent<Button>();
        exitbtn = GameObject.Find("UI_Canvas").transform.GetChild(0).GetChild(2).gameObject.GetComponent<Button>();
    }
    public void GameOver() //게임 오버
    {
        isGameOver = true;
        Time.timeScale = (isGameOver)? 0.0f : 1.0f; //게임의 시간을 0으로 설정
        GameoverUI.SetActive(true);
    }
    public GameObject GetBullet()//총알 오브젝트 풀링 생성
    {
        for (int i = 0; i < bulletPool.Count; i++)
        {
            if (bulletPool[i].activeSelf == false)
            {
                return bulletPool[i];
            }
        }
        return null;
    }
    private void CreatePoolling()// 적 오브젝트 풀링 생성
    {
        GameObject objectPool = new GameObject("ObjectPool_Player");
        for (int i = 0; i < maxPool; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, objectPool.transform);
            bullet.name = "bullet" + i.ToString();
            bullet.SetActive(false);
            bulletPool.Add(bullet);
        }

       GameObject EnemyPool = new GameObject("EnemyPool");
        for (int i = 0; i < Maxenemy; i++)
        {
            GameObject enemy = Instantiate(EnemyPrefab[0], EnemyPool.transform);
            enemy.name = "zombie" + i.ToString();
            enemy.SetActive(false);
            enemyPool.Add(enemy);
        }
        if (Points.Length > 0 )
            StartCoroutine(CreateZombie());

    }
    IEnumerator CreateZombie() //적 등장 코루틴
    {
        while (!isGameOver || !isWait)
        {
            yield return new WaitForSeconds(2f);
            if (isGameOver || isWait) yield break;
            foreach (GameObject enemy in enemyPool)
            {
                if (enemy.activeSelf == false && EnemyCount < WaveEnemy)
                {
                    int idx = Random.Range(0, Points.Length);
                    enemy.transform.position = Points[idx].position;
                    enemy.SetActive(true);
                    EnemyCount++;
                    break;
                }
            }
        }
    } 
    public void useBullet()// 사용한 총알
    {
        gameData.MyBullet -= fireCtrl.remainingBullet;
    }
    //UI
    public void OnPauseClick()//일시 정지
    {
        isPause = !isPause;
        Time.timeScale = (isPause) ? 0.0f : 1.0f;
        if (isPause)
            MenuUI.SetActive(true);
        else
            MenuUI.SetActive(false);
        
    }
    public void KillData()// 적 처치 시 받는 돈과 총알
    {
        gameData.MyBullet += Random.Range(0, 30);
        gameData.Coin += Random.Range(50, 300);
        
    }
    
    public void Ability(Card card) //카드의 효과  
    {
        if (isSelect == false)   
        {
            isSelect = true;
            switch (selectcard.CardAbility)
            {
                case CardAbility.Get_bullet:
                    gameData.MyBullet += 100;                    
                    break;
                case CardAbility.PlayerHP_ten:
                    playerHealth.GetHealth();
                    break;
                case CardAbility.Enemy_Speed:
                    gameData.EnemySpeed += 0.1f;
                    break;
                case CardAbility.Enemy_Power:
                    gameData.EnemyPower += 0.1f;
                    break;
                case CardAbility.ReloadTime:
                    gameData.ReloadingTime -= 0.05f;
                    break;
                case CardAbility.FireRateTime:
                    gameData.FireRate -= 0.1f;
                    break;
                case CardAbility.MaxEnemy:
                    gameData.maxEnemy += 5;
                    break;
                case CardAbility.Money:
                    gameData.Coin += 250;
                    break;
            }
        }
    }

    public void WaveClear()  //웨이브가 클리어 되면 카드 UI를 켜고 게임을 잠시 멈춤
    {      
        CardUI.SetActive(true);
        isWait = true;            
    }
    public void ClickNext()  //다음 버튼 클릭 스크립트
    {     
        if (isSelect == true)  //카드를 선택했을 경우에만 실행 
        {
            EnemyCount = 0;  //생성된 적 초기화
            gameData.maxEnemy += Random.Range(5, 10);  // 추가 적  생성
            gameData.EnemyHp += Random.Range(5, 20);   // 적 체력 증가
            gameData.EnemyPower += gameData.EnemyPower * 0.2f;  //적 공격력 증가
            gameData.EnemySpeed += gameData.EnemySpeed * 0.1f;  // 적 속도 증가
            WaveEnemy = gameData.maxEnemy;   //남은 적 초기화
            currentEnemy = WaveEnemy;            
            MarketUI.SetActive(false);  
            CardUI.SetActive(false);   
            playerMove.isWait = false;
            isWait = false;
            isSelect = false;
            StartCoroutine(CreateZombie());  //좁비 생성 코루틴 실행
        }     
    }
    // 상점
    public void ClickShopBtn() //마켓 버튼 클릭 스크립트
    {
        if (isSelect == true)  //카드를 선택했을 경우
        {
            CardUI.SetActive(false);
            MarketUI.SetActive(true);  //카드 UI를 끄고 마켓 UI를 켠다.
        }
    } 
    //무기 구매
    public void BuyRifle()  
    {
        if (gameData.Coin >= 1000 && !gameData.haveRifle)
        {
            gameData.Coin -= 1000;
            gameData.haveRifle = true;
            BuyR.SetActive(false);
        }
    }
    public void BuyMachineGun()
    {
        if (gameData.Coin >= 5000 && !gameData.haveMachinegun)
        {
            gameData.Coin -= 5000;
            gameData.haveMachinegun = true;
            BuyM.SetActive(false);
        }
    }
    // 버튼
    public void OnClickNewGameBtn() //새 게임 시작 
    {
        OnResetData();
        SceneManager.LoadScene("GameScene");
        Loading();
        
    }
    public void OnClickRoadGameBtn() // 이어 하기
    {
        SceneManager.LoadScene("GameScene");
    }
    public void OnResetData()  //게임 데이터 초기화
    {
        gameData.Coin = 1000;  // 가진 돈      

        gameData.PlayerHp = 100f;  //플레이어 체력
        gameData.PlayerPower = 10f; // 플레이어 공격력
        gameData.PlayerSpeed = 3f;  // 플레이어 속도

        gameData.HQHp = 100;  //본진 체력

        gameData.FireRate = 2f;  // 공격 속도
        gameData.MyBullet = 100;  //가진 총알 총 수
        gameData.GunBullet = 1; //탄창 수      
        gameData.ReloadingTime = 1.0f;  // 재장전 시간
        gameData.haveRifle = false;  
        gameData.haveMachinegun = false;

        gameData.maxEnemy = 1;
        gameData.EnemySpeed = 1f;  // 적 속도
        gameData.EnemyHp = 10f;  // 적 체력
        gameData.EnemyPower = 5f;  // 적 공격력
        gameData.EnemyAttackS = 1f; //적 공격 속도
    }
    public void OnGameQuit() // 게임 종료
    {
        Application.Quit();
    }
    public void OnMainMenu() // 메인 메뉴
    {
        SceneManager.LoadScene("StartScene");   
    }

}
