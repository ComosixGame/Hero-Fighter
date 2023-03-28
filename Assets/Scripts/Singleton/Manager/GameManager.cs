using System;
using UnityEngine;
using Cinemachine;
using MyCustomAttribute;


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
    public event Action OnStartGame;
    public event Action OnPause;
    public event Action OnResume;
    public event Action<bool> OnEndGame;
    public event Action OnNewGame;
    public event Action<int> OnUpdateMoney;
    public event Action OnCheckedPoint;
    public event Action OnInitUiDone;
    [ReadOnly] public int chapterIndex;
    [ReadOnly] public int levelIndex;
    private PlayerData playerData;
    public SettingData settingData;
    private ObjectPoolerManager objectPooler;

    protected override void Awake()
    {
        base.Awake();
        objectPooler = ObjectPoolerManager.Instance;
    }

    private void Start() {
        Application.targetFrameRate = 60;
    }

    private void OnEnable() {
#if UNITY_EDITOR
        objectPooler.OnCreatedObjects += () => {
            if(debugMode) {
                player = GameObject.FindWithTag("Player")?.transform;
                StartGame();
            }

            if(withoutUI) {
                InitUiDone();
            }
        };
#endif
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        OnStartGame?.Invoke();
        playerData = PlayerData.Load();
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
        if (win && playerData.LatestLevel == levelIndex+1)
        {
            playerData = PlayerData.Load();
            int nextLevel = playerData.LatestLevel+1;
            playerData.levels.Add(nextLevel);
            playerData.Save();
        }
        DestroyGameObjectPooler();
        OnNewGame?.Invoke();
    }

    public void UpdateMoney(int amount) {
        money += amount;
        OnUpdateMoney?.Invoke(money);
    }

    public void CheckedPoint()
    {
        OnCheckedPoint?.Invoke();
    }

    public void DestroyGameObjectPooler()
    {
        objectPooler.ResetObjectPoolerManager();
    }

    public void SelecteCharacter(string id) {
        playerData.selectedCharacter = id;
    }


}
