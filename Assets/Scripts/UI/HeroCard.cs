using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroCard : MonoBehaviour
{
    public int id;
    [SerializeField] private Image thumbnail;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI priceTxt;
    [SerializeField] private TextMeshProUGUI evaluateTxt;
    [SerializeField] private GameObject startGo;

    //
    public GameObject windowSkill;
    private GameManager gameManager;

    private void Awake()
    {
        button = GetComponentInChildren<Button>();
    }

    private void OnEnable() {
        button.onClick.AddListener(ClickButton);
    }

    private void ClickButton()
    {
        windowSkill.SetActive(true);
    }

    public void SetDataCard(Sprite thumbnailSprite, string title, float price, int start, string evaluate)
    {
        thumbnail.sprite = thumbnailSprite;
        this.title.text = title;
        evaluateTxt.text = evaluate;
        priceTxt.text = price+"";
        for(int i = 0; i < start; i++)
        {
            startGo.transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void SetDataCard(Sprite thumbnailSprite, string title, int start)
    {
        thumbnail.sprite = thumbnailSprite;
        this.title.text = title;
        for(int i = 0; i < start; i++)
        {
            startGo.transform.GetChild(i).gameObject.SetActive(true);
        }
    }
}
