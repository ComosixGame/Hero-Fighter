using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class SkillDetailCard : MonoBehaviour
{
    public int id;
    [SerializeField] private Image thumnail;
    [SerializeField] private TextMeshProUGUI titleSkill;
    [SerializeField] private TextMeshProUGUI skillLevel;
    [SerializeField] private TextMeshProUGUI price;
    [SerializeField] private Button updrageBtn;

    private PlayerCharacter playerCharacter;

    private GameManager gameManager;
    private SoundManager soundManager;
    public AudioClip soundBtn;

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


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void ClickButton()
    {
        if (gameManager.BuySkill(playerCharacter, id))
        {
            SetDataLevel(id);
        }
    }

    public void SetDataCard(PlayerCharacter playerCharacter, int skillStateId, string skillLevel)
    {
        this.playerCharacter = playerCharacter;
        this.thumnail.sprite = playerCharacter.skillStates[skillStateId].sprite;
        this.titleSkill.text = playerCharacter.skillStates[skillStateId].nameSkill;
        this.skillLevel.text = skillLevel;
        this.price.text = playerCharacter.skillStates[skillStateId].price+"";
    }

    private void SetDataLevel(int index)
    {
        PlayerData playerData = PlayerData.Load();
        int id =  playerData.characters.FindIndex(x => x.keyID == playerCharacter.keyID);
        this.skillLevel.text = "Level Skill: " + playerData.characters[id].levelSkills[index];  
    }
}
