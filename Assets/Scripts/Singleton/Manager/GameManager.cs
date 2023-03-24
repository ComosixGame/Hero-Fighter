using System;
using UnityEngine;
using Cinemachine;
using MyCustomAttribute;


public class GameManager : Singleton<GameManager>
{
    [SerializeField] private bool debugMode;
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
    public int chapterIndex;
    public int levelIndex;
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
        objectPooler.OnCreatedObjects += () => {
            if(debugMode) {
                StartGame();
            }
        };
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
        if (win)
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

}
