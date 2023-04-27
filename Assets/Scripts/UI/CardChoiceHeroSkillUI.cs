using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardChoiceHeroSkillUI : MonoBehaviour
{

    private Sprite selectedBoder;
    private Sprite normalBoder;
    private TextMeshProUGUI title = new TextMeshProUGUI();
    private Sprite heroSprite;
    private Button button;
    private SkillTreeUI skillTreeUI;
    private string keyId;
    private GameManager gameManager;
    private PlayerCharacter playerCharacter;


    private void Awake()
    {
        button = GetComponent<Button>();
        gameManager = GameManager.Instance;

    }   

    private void OnEnable() {
        button.onClick.AddListener(ClickButton);
        gameManager.OnSelectHeroSkill += CheckSelect;
    }

    private void OnDisable() {
        button.onClick.RemoveListener(ClickButton);
        gameManager.OnSelectHeroSkill -= CheckSelect;
    }

    private void ClickButton()
    {
        skillTreeUI.ChangeInforCard(heroSprite, title.text);
        gameManager.selectHeroSkillUpgrade(playerCharacter);
        skillTreeUI.ClearSkillCardData();
        skillTreeUI.RenderSkillDetail(keyId);
    }

    public void SetDataButton(bool selected, Sprite selectedBoder, Sprite normalBoder, PlayerCharacter playerCharacter, SkillTreeUI skillTreeUI)
    {
        this.playerCharacter = playerCharacter;
        this.title.text = playerCharacter.name;
        this.heroSprite = playerCharacter.thumbnail;
        this.selectedBoder = selectedBoder;
        this.normalBoder = normalBoder;
        this.skillTreeUI = skillTreeUI;
        this.keyId = playerCharacter.keyID;
        if (selected)
        {
            button.interactable = false;
            button.GetComponent<Image>().sprite = selectedBoder;
        }else
        {
            button.interactable = true;
            button.GetComponent<Image>().sprite = normalBoder;
        }
    }

    public void CheckSelect(string keyID)
    {
        if (keyID == playerCharacter.keyID)
        {
            button.interactable = false;
            button.GetComponent<Image>().sprite = selectedBoder;
        } else {
            button.interactable = true;
            button.GetComponent<Image>().sprite = normalBoder;
        }
    }

}
