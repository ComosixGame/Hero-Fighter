using System;
using UnityEngine;
using System.Collections.Generic;
using MyCustomAttribute;

public class GameManager : Singleton<GameManager>
{
    public Transform player {get; private set;}
    private int money;
    public event Action OnStartGame;
    public event Action OnPause;
    public event Action OnResume;
    public event Action<bool> OnEndGame;
    public event Action OnNewGame;
    public event Action<int> OnUpdateMoney;
    public event Action<Transform> OnLose;
    public event Action OnEnemiesDestroyed;
    public event Action OnGoneCheckPoint;
    public UIMenu uiMenu;
    private PlayerData playerData;
    public SettingData settingData;

    private ObjectPoolerManager objectPoolerManager;

    private void OnEnable() {
        
    }

    private void Start() {
        Application.targetFrameRate = 60;
    }

    public void SetPlayer(Transform player) {
        this.player =  player;
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        objectPoolerManager = ObjectPoolerManager.Instance;
        playerData = PlayerData.Load();
        Debug.Log("LV" + playerData.LatestLevel);
        OnStartGame?.Invoke();
        uiMenu = FindObjectOfType<UIMenu>();
        uiMenu.SetStartGameAnimation();
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
        Time.timeScale = 0.3f;
        uiMenu.GameWin();
    }

    public void GameLose()
    {
        uiMenu.GameLose();
        OnLose?.Invoke(player);
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
        //Invoke for Sound
        OnEndGame?.Invoke(win);
    }

    public void UpdateMoney(int amount) {
        money += amount;
        OnUpdateMoney?.Invoke(money);
    }

    public void GoneCheckPoint()
    {
        OnGoneCheckPoint?.Invoke();
    }

    public void DestroyGameObjectPooler()
    {
        objectPoolerManager.DeleteObjectPoolerManager();
    }

}
