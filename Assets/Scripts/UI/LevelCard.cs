using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelCard : MonoBehaviour
{
    public int id;
    public int chapter;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private GameObject lockIcon;
    private Button button;
    private GameManager gameManager;
    private LoadingScreen loadingScreen;

    private void Awake()
    {
        button = GetComponent<Button>();
        gameManager = GameManager.Instance;
        loadingScreen = FindObjectOfType<LoadingScreen>();
    }

    private void OnEnable()
    {
        gameManager.OnSelectChapter += SelectChapter;
        button.onClick.AddListener(ClickButton);
    }

    private void SelectChapter(int index)
    {
        chapter = index;
    }
    private void ClickButton()
    {
        gameManager.SetLevel(id);
        loadingScreen.Play(chapter);
    }

    public void Unlock(bool unlock)
    {
        lockIcon.SetActive(!unlock);
        title.gameObject.SetActive(unlock);
        button.interactable = unlock;
    }
}
