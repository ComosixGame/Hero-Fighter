using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

public class LoadScene : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider loadingBar;
    public bool LoadOnAwake;
    [HideInInspector] public int nextLevel;
    private AsyncOperation operation;
    private int levelIndex;
    private GameManager gameManager;

    private void Awake() 
    {
        gameManager = GameManager.Instance;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        LoadNewScene(1);
    }

    public void LoadNewScene(int index) {
        StartCoroutine(LoadAsync(index));
        operation.completed += InitGame;
    }

    IEnumerator LoadAsync(int index) {
        operation = SceneManager.LoadSceneAsync(index);
        loadingScreen.SetActive(true);
        while(!operation.isDone) {
            float progress = Mathf.Clamp01(operation.progress/ 0.9f);
            loadingBar.value = progress;
            yield return null;
        }
    }

    private void InitGame(AsyncOperation asyncOperation) {
        gameManager.StartGame();
    }

}
