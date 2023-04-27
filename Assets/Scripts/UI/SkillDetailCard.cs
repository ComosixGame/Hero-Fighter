using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillDetailCard : MonoBehaviour
{
    public int id;
    [SerializeField] private Image thumnail;
    [SerializeField] private TextMeshProUGUI titleSkill;
    [SerializeField] private TextMeshProUGUI skillLevel;
    [SerializeField] private TextMeshProUGUI price;
    [SerializeField] private Button updrageBtn;
    private PlayerCharacter playerCharacter;
    private PlayerData playerData;
    private SkillState skillState;
    private GameManager gameManager;
    private SoundManager soundManager;
    public AudioClip soundBtn;
    private int index;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        soundManager = SoundManager.Instance;
    }

    public void PlaySound(){
        soundManager.PlaySound(soundBtn);
    }
    private void OnEnable()
    {
        updrageBtn.onClick.AddListener(ClickButton);
    }

    public void RenderInfor()
    {
        playerData = PlayerData.Load();
        if (playerData.characters[index].levelSkills[id] == 5)
        {
            updrageBtn.gameObject.SetActive(false);
            this.skillLevel.text = "Max Level";
        }
        else
        {
            this.skillLevel.text = "Level Skill: " + playerData.characters[index].levelSkills[id];
            updrageBtn.gameObject.SetActive(true);
        }
    }

    private void ClickButton()
    {
        playerData = PlayerData.Load();
        index = playerData.characters.FindIndex(x => x.keyID == playerCharacter.keyID);
        if (gameManager.BuySkill(playerCharacter, id))
        {
            SetDataLevel();
        }

        if (playerData.characters[index].levelSkills[id] >= 4)
        {
            this.skillLevel.text = "Max Level";
            updrageBtn.gameObject.SetActive(false);
        }

    }

    public void SetDataCard(PlayerCharacter playerCharacter, int skillStateId, string skillLevel, int id)
    {
        this.index = id;
        this.playerCharacter = playerCharacter;
        this.thumnail.sprite = playerCharacter.skillStates[skillStateId].sprite;
        this.titleSkill.text = playerCharacter.skillStates[skillStateId].nameSkill;
        this.skillLevel.text = skillLevel;
        this.price.text = playerCharacter.skillStates[skillStateId].price + "";
        RenderInfor();
    }

    private void SetDataLevel()
    {
        PlayerData playerData = PlayerData.Load();
        int index = playerData.characters.FindIndex(x => x.keyID == playerCharacter.keyID);
        this.skillLevel.text = "Level Skill: " + playerData.characters[index].levelSkills[id];
    }
}
