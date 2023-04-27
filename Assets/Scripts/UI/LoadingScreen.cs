using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private GameObject windowLoading;
    [SerializeField] private Slider loadingBar;
    private LoadSceneManager loadSceneManager;
    private GameManager gameManager;

    private void Awake() {
        loadSceneManager = LoadSceneManager.Instance;
        gameManager = GameManager.Instance;
    }

    private void OnEnable() {
        loadSceneManager.OnStart += StartLoading;
        loadSceneManager.OnLoad += Loading;

    }
    
    private void OnDisable() {
        loadSceneManager.OnStart -= StartLoading;
        loadSceneManager.OnLoad -= Loading;
    }

    private void Loading(float value)
    {
        loadingBar.value = value;
    }

    private void StartLoading()
    {
        windowLoading.SetActive(true);
    }

    public void BackToMenu()
    {
        gameManager.ResumeGame();
        loadSceneManager.LoadScene(0);
    }

    public void Play(int level)
    {
        loadSceneManager.LoadScene(level);
    }
}
