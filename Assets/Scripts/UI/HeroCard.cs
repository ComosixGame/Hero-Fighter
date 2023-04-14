using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HeroCard : MonoBehaviour
{
    private PlayerCharacter playerCharacter;
    [SerializeField] private Image thumbnail;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI priceTxt;
    [SerializeField] private TextMeshProUGUI evaluateTxt;
    [SerializeField] private GameObject startGo;
    [SerializeField] private UnityEvent OnBuySuccess;
    private GameManager gameManager;
    private SoundManager soundManager;
    public AudioClip soundBtn;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        soundManager = SoundManager.Instance;
        button = GetComponentInChildren<Button>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(ClickButton);
    }

    private void ClickButton()
    {
        if (gameManager.BuyHero(playerCharacter))
        {
            button.interactable = false;
            OnBuySuccess?.Invoke();
        }
    }

    public void PlaySound(){
        soundManager.PlaySound(soundBtn);
    }

    public void SetDataCard(PlayerCharacter playerCharacter, bool owned)
    {
        this.playerCharacter = playerCharacter;
        thumbnail.sprite = playerCharacter.thumbnail;
        this.title.text = playerCharacter.name;
        evaluateTxt.text = playerCharacter.evaluate;
        priceTxt.text = playerCharacter.price.ToString();
        if (owned)
        {
            button.interactable = false;
            OnBuySuccess?.Invoke();
        }
        for (int i = 0; i < playerCharacter.start; i++)
        {
            startGo.transform.GetChild(i).gameObject.SetActive(true);
        }

    }
}
