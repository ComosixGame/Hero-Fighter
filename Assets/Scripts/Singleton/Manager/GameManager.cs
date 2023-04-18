using System;
using UnityEngine;
using Cinemachine;
using MyCustomAttribute;
using TMPro;


public class GameManager : Singleton<GameManager>
{
#if UNITY_EDITOR
    [Header("Debug")]
    [SerializeField] private bool debugMode;
    [SerializeField] private bool withoutUI;
#endif
    [Header("Info")]
    [ReadOnly] public Transform player;
    [ReadOnly] public CinemachineVirtualCamera virtualCamera;
    private int money;
    public ChapterManager chapterManager;
    public EquipmentManager equipmentManager;
    public event Action OnStartGame;
    public event Action OnPause;
    public event Action OnResume;
    public event Action<bool> OnEndGame;
    public event Action OnNewGame;
    public event Action<int> OnUpdateMoney;
    public event Action OnCheckedPoint;
    public event Action OnInitUiDone;
    public event Action<int> OnSelectChapter;
    public event Action<string> OnSelectCharacter;
    public event Action<string> OnSelectHeroSkill;
    public event Action OnBuyHero;
    public event Action OnNotEnoughEnergy;
    [ReadOnly] public int chapterIndex;
    [ReadOnly] public int levelIndex;
    private PlayerData playerData;
    public SettingData settingData;
    private ObjectPoolerManager objectPooler;
    private LoadSceneManager loadSceneManager;
    private LoadingScreen loadingScreen;
    [SerializeField] private GameObject windowPopup;
    [SerializeField] private TextMeshProUGUI coin;
    public int bonousCoin;

    public LevelState levelState
    {
        get
        {
            return chapterManager.chapterStates[chapterIndex].levelStates[levelIndex];
        }
    }


    protected override void Awake()
    {
        base.Awake();
        playerData = PlayerData.Load();
        objectPooler = ObjectPoolerManager.Instance;
        loadSceneManager = LoadSceneManager.Instance;
        settingData = SettingData.Load();
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        loadSceneManager.OnDone += InitGame;
        // chapterIndex = -1;
    }

    private void OnEnable()
    {
        // playerData = PlayerData.Load();
        // coin.text = playerData.money + "";
#if UNITY_EDITOR
        objectPooler.OnCreatedObjects += () =>
        {
            if (debugMode)
            {
                player = GameObject.FindWithTag("Player")?.transform;
                StartGame();
            }

            if (withoutUI)
            {
                InitUiDone();
            }
        };
#endif
    }

    private void OnDestroy()
    {
        loadSceneManager.OnDone -= InitGame;
    }

    private void InitGame()
    {
        StartGame();
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        OnStartGame?.Invoke();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        OnPause?.Invoke();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        OnResume?.Invoke();
    }

    public void GameWin()
    {
        OnEndGame.Invoke(true);
    }

    public void GameLose()
    {

        OnEndGame.Invoke(false);
    }

    public void InitUiDone()
    {
        OnInitUiDone?.Invoke();
    }

    public void NewGame(bool win)
    {
        //Add new Level
        if (win && playerData.latestLevel == levelIndex)
        {
            int nextLevel = playerData.latestLevel + 1;
            playerData.levels.Add(nextLevel);
            playerData.Save();
        }
        DestroyGameObjectPooler();
        OnNewGame?.Invoke();
    }

    public void UpdateMoney(int amount)
    {
        money += amount;
        OnUpdateMoney?.Invoke(money);
    }

    public void CheckedPoint()
    {
        OnCheckedPoint?.Invoke();
    }

    public void SetChapter(int id)
    {
        chapterIndex = id;
        OnSelectChapter?.Invoke(id);
    }

    public void SetLevel(int id)
    {
        levelIndex = id;
    }

    public void DestroyGameObjectPooler()
    {
        objectPooler.ResetObjectPoolerManager();
    }

    public void SelecteCharacter(string id)
    {
        playerData.selectedCharacter = id;
    }

    public bool BuyHero(PlayerCharacter character)
    {
        if (playerData.money >= character.price && !CheckCharacterOwed(character))
        {
            OnBuyHero?.Invoke();
            playerData.money -= character.price;
            playerData.characters.Add(new PlayerData.Character(character.keyID));
            playerData.Save();

            return true;
        }
        else
        {
            windowPopup.SetActive(true);
            return false;
        }
    }

    public bool selectedHero(PlayerCharacter character)
    {
        if (CheckCharacterOwed(character))
        {
            playerData.selectedCharacter = character.keyID;
            playerData.Save();
            OnSelectCharacter?.Invoke(character.keyID);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void selectHeroSkillUpgrade(PlayerCharacter character)
    {
        OnSelectHeroSkill?.Invoke(character.keyID);
    }

    public bool CheckCharacterOwed(PlayerCharacter character)
    {
        PlayerData.Character playerChar = playerData.characters.Find(charac => charac.keyID == character.keyID);
        return playerChar != null;
    }


    public bool BuySkill(PlayerCharacter character, int skillId)
    {
        if (playerData.money >= character.skillStates[skillId].price)
        {
            int index = playerData.characters.FindIndex(x => x.keyID == character.keyID);
            if (equipmentManager.Characters[index].skillStates[skillId].level < 5)
            {
                playerData.money -= character.skillStates[skillId].price;
                equipmentManager.Characters[index].skillStates[skillId].level += 1;
                playerData.Save();
            }
            return true;
        }
        else
        {
            windowPopup.SetActive(true);
            return false;
        }
    }

    public PlayerCharacter DisplayHeroInUi()
    {
        playerData = PlayerData.Load();
        return equipmentManager.GetCharacter(playerData.selectedCharacter);
    }

    public void BonousCoin(bool isTriple ,int coin)
    {
        playerData = PlayerData.Load();
        if (isTriple)
        {
            coin = coin*3;
        }
        playerData.money += coin;
        playerData.Save();
    }

    public void NotEnoughEnergy()
    {
        OnNotEnoughEnergy?.Invoke();
    }
    
}
