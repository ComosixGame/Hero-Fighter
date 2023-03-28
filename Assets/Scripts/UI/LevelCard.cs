using UnityEngine;
using UnityEngine.UI;

public class LevelCard : MonoBehaviour
{
    public int id;
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
        // Debug.Log(123);
        // gameManager.loadingScreen.Play(1);
        gameManager.SetLevel(id);
    }
}
