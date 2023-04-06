using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class HeroCardSkill : MonoBehaviour
{
    public int id;
    [SerializeField] private Image thumbnail;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private Button button;
    [SerializeField] private GameObject startGo;
    [SerializeField] private Sprite selectBoder;
    [SerializeField] private Sprite normalBoder;
    private PlayerCharacter playerCharacter;
    public GameObject windowSkill;
    public Animator UIMenu;
    public SkillTreeUI skillTreeUI;
    private GameManager gameManager;
    private void Awake()
    {
        button = GetComponentInChildren<Button>();
        gameManager = GameManager.Instance;
    }

    private void OnEnable()
    {
        button.onClick.AddListener(ClickButton);
        gameManager.OnSelectCharacter += CheckSelect;
    }

    private void ClickButton()
    {
        // windowSkill.SetActive(true);
        // skillTreeUI.RenderSkillCard(id);
        if (gameManager.selectedHero(playerCharacter))
        {
            button.interactable = false;
            GetComponent<Image>().sprite = selectBoder;
        }
    }

    public void SetDataCard(PlayerCharacter playerCharacter, bool selected)
    {
        this.playerCharacter = playerCharacter;
        thumbnail.sprite = playerCharacter.thumbnail;
        this.title.text = playerCharacter.name;
        if (selected)
        {
            button.interactable = false;
            GetComponent<Image>().sprite = selectBoder;
        }
        for (int i = 0; i < playerCharacter.start; i++)
        {
            startGo.transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void CheckSelect(string keyID)
    {
        if (keyID == playerCharacter.keyID)
        {
            button.interactable = false;
            GetComponent<Image>().sprite = selectBoder;
        } else {
            button.interactable = true;
            GetComponent<Image>().sprite = normalBoder;
        }
    }
}
