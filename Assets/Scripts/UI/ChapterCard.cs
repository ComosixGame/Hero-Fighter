using UnityEngine;
using UnityEngine.UI;

public class ChapterCard : MonoBehaviour
{
    public int id;
    public GameObject windowSelectLevel;
    private Button button;
    private GameManager gameManager;
    private void Awake() {
        button = GetComponent<Button>();
        gameManager = GameManager.Instance;
    }

    private void OnEnable() {
        button.onClick.AddListener(ClickButton);
    }
    
    private void ClickButton()
    {
        windowSelectLevel.SetActive(true);
        gameManager.SetChapter(id);
    }


}
