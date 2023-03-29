using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : Singleton<LoadSceneManager>
{
    public event Action OnStart;
    public event Action<float> OnLoad;
    public event Action OnDone;
    private AsyncOperation operation;
    private GameManager gameManager;

    protected override void Awake()
    {
        base.Awake();
        gameManager =  GameManager.Instance;
    }

    public void LoadScene(int index)
    {
        OnStart?.Invoke();
        StartCoroutine(LoadAsync(index));
        operation.completed += SceneLoaded;
    }

    public void LoadScene(string path)
    {
        OnStart?.Invoke();
        StartCoroutine(LoadAsync(path));
        operation.completed += SceneLoaded;
    }

    IEnumerator LoadAsync(int index)
    {
        operation = SceneManager.LoadSceneAsync(index);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            OnLoad?.Invoke(progress);
            yield return null;
        }
    }

    IEnumerator LoadAsync(string path)
    {
        operation = SceneManager.LoadSceneAsync(path);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            OnLoad?.Invoke(progress);
            yield return null;
        }
    }

    private void SceneLoaded(AsyncOperation operation) {
        // gameManager.StartGame();
        OnDone?.Invoke();
    }
}
