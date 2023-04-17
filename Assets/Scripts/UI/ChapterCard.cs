using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChapterCard : MonoBehaviour
{
    public int id;
    [SerializeField] private Image thumbnail;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private GameObject lockIcon;
    [HideInInspector] public GameObject windowSelectLevel;
    private Button button;
    private GameManager gameManager;
    private SoundManager soundManager;
    public AudioClip soundBtn;
    private void Awake()
    {
        button = GetComponent<Button>();
        gameManager = GameManager.Instance;
        soundManager = SoundManager.Instance;
    }

    private void OnEnable()
    {
        button.onClick.AddListener(ClickButton);
    }
    public void PlaySound(){
        soundManager.PlaySound(soundBtn);
    }

    private void ClickButton()
    {
        gameManager.SetChapter(id);
        windowSelectLevel.SetActive(true);
    }

    public void SetDataCard(Sprite thumbnailSprite, string title)
    {
        thumbnail.sprite = thumbnailSprite;
        this.title.text = title;
    }

    public void Unlock(bool unlock)
    {
        lockIcon.SetActive(!unlock);
        button.interactable = unlock;
        Color origin = thumbnail.color;
        origin.a = unlock ? origin.a : 0.5f;
        thumbnail.color = origin;
    }
}
