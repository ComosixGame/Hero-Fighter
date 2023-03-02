using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : Singleton<LoadSceneManager>
{
    private AsyncOperation operation;
    public event Action<float> OnLoad;

    public void LoadScene(int index)
    {
        StartCoroutine(LoadAsync(index));
    }

    public void LoadScene(string path)
    {
        StartCoroutine(LoadAsync(path));
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

}
