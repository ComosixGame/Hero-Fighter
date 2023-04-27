using System;
using UnityEngine;

public class StartGameEvent : MonoBehaviour
{
    [SerializeField] private float delayStart;
    public static event Action OnStart;
    private GameManager gameManager;

    private void Awake() {
        gameManager = GameManager.Instance;
    }

    private void OnEnable() {
        Invoke("StartGame", delayStart);
    }

    private void StartGame()
    {
        gameManager.playerDestroyed = false;
        OnStart?.Invoke();
    }

}
