using System;
using UnityEngine;

public class StartGameEvent : MonoBehaviour
{
    [SerializeField] private float delayStart;
    public static event Action OnStart;

    private void OnEnable() {
        Invoke("StartGame", delayStart);
    }

    private void StartGame()
    {
        OnStart?.Invoke();
    }
}
