using System;
using UnityEngine;
using MyCustomAttribute;

public class GameManager : Singleton<GameManager>
{
    public Transform player {get; private set;}
    private int money;
    public event Action OnPause;
    public event Action OnResume;
    public event Action<int> OnUpdateMoney;

    public void SetPlayer(Transform player) {
        this.player =  player;
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

    public void UpdateMoney(int amount) {
        money += amount;
        OnUpdateMoney?.Invoke(money);
    }

}
